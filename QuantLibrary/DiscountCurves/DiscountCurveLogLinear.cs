using MathNet.Numerics.Interpolation;

namespace QuantLibrary.DiscountCurves
{
    public class DiscountCurveLogLinear : DiscountCurve
    {
        public LinearSpline Interpolator;
        public DiscountCurveLogLinear(IEnumerable<double> times, IEnumerable<double> values) : base(times, values) 
        {
            Interpolator = LinearSpline.Interpolate(times, values);
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
            Interpolator = LinearSpline.Interpolate(Times, DiscountFactors);
        }
    }
}
