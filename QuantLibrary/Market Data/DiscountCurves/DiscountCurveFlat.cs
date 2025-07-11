using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantLibrary.DiscountCurves
{
    public class DiscountCurveFlat : DiscountCurve
    {
        public readonly double Rate;
        public DiscountCurveFlat(string name, double rate) : base(name, [0], [1])
        {
            Rate = rate;
        }

        public override double Df(double t)
        {
            return Math.Exp(-Rate * t);
        }

        public override void UpdateDfs(IEnumerable<double> times, IEnumerable<double> discountFactors)
        {
            throw new NotImplementedException();
        }
    }
}
