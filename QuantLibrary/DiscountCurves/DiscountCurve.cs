
namespace QuantLibrary.DiscountCurves
{

    public abstract class DiscountCurve
    {
        public List<double> Times;
        public List<double> DiscountFactors;

        public int N;
        public DiscountCurve(IEnumerable<double> times, IEnumerable<double> discountFactors)
        {
            Times = [.. times];
            DiscountFactors = [.. discountFactors];
            N = times.Count();
            
            if (Times[0] != 0)
            {
                throw new Exception("Time's first element must be 0");
            }
            if (DiscountFactors[0] != 1.0) 
            {
                throw new Exception("Discount Factor's first element must be 1.0");
            }
        }

        public abstract double Df(double t);

        public virtual void SetDfAtIndex(int index, double df)
        {
            DiscountFactors[index] = df;
        }

        public abstract void UpdateDfs(IEnumerable<double> times, IEnumerable<double> discountFactors);
        public List<double> GetZeroRates()
        {
            return [.. Times.Zip(DiscountFactors, (t, df) => -Math.Log(df) / t).Skip(1)];
        }
    }
}
