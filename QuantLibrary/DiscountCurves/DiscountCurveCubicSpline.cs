using QuantLibrary.DiscountCurves;
using MathNet.Numerics.Interpolation;

namespace QuantLibrary.DiscountCurves
{
    public class DiscountCurveCubicSpline : DiscountCurve
    {
        public CubicSpline Interpolator;
        public DiscountCurveCubicSpline(IEnumerable<double> times, IEnumerable<double> values) : base(times, values)
        {
            Interpolator = CubicSpline.InterpolateAkima(times, values);
        }
        public override double Df(double t)
        {
            if (t < 0)
            {
                throw new Exception("Cannot get discount factor for a time less than 0");
            }

            return Interpolator.Interpolate(t);
        }

        public override void UpdateDfs(IEnumerable<double> times, IEnumerable<double> discountFactors)
        {
            Times = [.. times];
            DiscountFactors = [.. discountFactors];
            Interpolator = CubicSpline.InterpolateAkima(Times, DiscountFactors);
        }
    }
}
