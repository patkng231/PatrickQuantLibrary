using QuantLibrary.Curve_Construction;
using QuantLibrary.DiscountCurves;
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


        }
    }

}
