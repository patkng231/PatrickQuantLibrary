using QuantLibrary.Curve_Construction;
using QuantLibrary.DiscountCurves;
using QuantLibrary.Market_Data.VolatilitySurface;
using QuantLibrary.Products.IR;
using QuantLibrary.Utilities;
namespace Test
{
    public class Program
    {
        static void Main(string[] args)
        {
            DateTime startDate = new DateTime(2020, 05, 03);

            List<InterestRateSwap> swapInstruments = [];
            for (int i = 1; i < 20; i++)
            {
                List<DateTime> fixingDates = [.. Enumerable.Range(0, i).Select(x => startDate.AddMonths(x * 3))];
                List<DateTime> paymentDates = [.. Enumerable.Range(1, i).Select(x => startDate.AddMonths(x * 3))];
                InterestRateSwap swap = new InterestRateSwap("mySwap" + i, 1.0, fixingDates, paymentDates, Frequency._1D, 0.05 + 0.005 * i, PayRecieve.Pay, []);
                swapInstruments.Add(swap);

            }

            SwapCurveConstruction curveConstructor = new SwapCurveConstruction(startDate, swapInstruments);
            DiscountCurve df1 = curveConstructor.BuildCurve(CurveInterpolationType.LogLinearDf);
            DiscountCurve df2 = curveConstructor.BuildCurve(CurveInterpolationType.CubicSplineDf);

            List<DateTime> optionExpiries = [.. Enumerable.Range(0, 5).Select(x => startDate.AddMonths(3 * (x + 1)))];
            List<List<double>> strikes = [.. Enumerable.Repeat((List<double>)[70, 80, 90, 100, 110, 120], 5)];
            List<List<double>> vols = [];
            vols.Add([0.7, 0.65, 0.6, 0.55, 0.5, 0.45]);
            vols.Add([0.65, 0.6, 0.55, 0.5, 0.45, 0.40]);
            vols.Add([0.6, 0.55, 0.5, 0.45, 0.40, 0.35]);
            vols.Add([0.55, 0.5, 0.45, 0.40, 0.35, 0.3]);
            vols.Add([0.5, 0.45, 0.40, 0.35, 0.3, 0.2]);

            VolSurfLinearInterp volSurface = new VolSurfLinearInterp("vol1", startDate, optionExpiries, strikes, vols);
            volSurface.Calibrate();
            double vol = volSurface.ImpliedVol(new DateTime(2020, 08, 03), 105);
        }
    }
}
