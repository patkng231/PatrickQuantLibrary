using QuantLibrary.DiscountCurves;
using QuantLibrary.Utilities;
using QuantLibrary.Valuation;
using MathNet.Numerics.Optimization;
using MathNet.Numerics.RootFinding;
using QuantLibrary.Products.IR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace QuantLibrary.Curve_Construction
{
    public class SwapCurveConstruction
    {
        public readonly List<InterestRateSwap> SwapInstruments;
        public readonly List<DateTime> InstrumentPoints;
        public readonly DateTime ValuationDate;
        public double tolerance = 1e-6;

        private DiscountCurve DiscountCurveCalibrate = null;

        public SwapCurveConstruction(DateTime valuationDate, List<InterestRateSwap> swapInstruments)
        {
            ValuationDate = valuationDate;
            SwapInstruments = swapInstruments;
            InstrumentPoints = [.. swapInstruments.Select(swap => swap.SwapEndDate)];

        }

        private void SwapSolverOneStep(InterestRateSwap swap, int index)
        {
            // Value swap instrument adjusting the discount factor for the appropriate curve point (index)
            double valueSwap(double discountFactor)
            {
                List<double> timesNew = DiscountCurveCalibrate.Times;
                List<double> dfsNew = DiscountCurveCalibrate.DiscountFactors;

                dfsNew[index + 1] = discountFactor;

                DiscountCurveCalibrate.UpdateDfs(timesNew, dfsNew);

                return InterestRateSwapValuation.Price(ValuationDate, swap, DiscountCurveCalibrate);
            }

            double df = Brent.FindRoot(valueSwap, 0.01, 2.0);
            // Set Curve 
            DiscountCurveCalibrate.SetDfAtIndex(index + 1, df);
        }

        private void SetInitialDiscountCurve(CurveInterpolationType curveInterpolationType)
        {
            List<double> times = [.. InstrumentPoints.Select(x => (x - ValuationDate).TotalDays / 365.0)];
            times.Insert(0, 0.0);
            List<double> dfs = [.. times.Select(x => Math.Exp(-x * 0.04))];

            switch (curveInterpolationType)
            {
                case CurveInterpolationType.LogLinearDf: DiscountCurveCalibrate = new DiscountCurveLogLinear(times, dfs);
                    break;
                case CurveInterpolationType.CubicSplineDf:
                    DiscountCurveCalibrate = new DiscountCurveCubicSpline(times, dfs);
                    break;
                default:
                    throw new Exception("Interpolation type not supported");
            }
        }

        public DiscountCurve BuildCurve(CurveInterpolationType curveInterpolationType)
        {
            SetInitialDiscountCurve(curveInterpolationType);

            // Build curve one swap instrument at a time
            for(int i = 0; i < SwapInstruments.Count; i++)
            {
                InterestRateSwap swap = SwapInstruments[i];
                SwapSolverOneStep(swap, i);
            }

            return DiscountCurveCalibrate;
        }
    }
}
