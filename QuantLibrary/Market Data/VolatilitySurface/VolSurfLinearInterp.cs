using MathNet.Numerics.Interpolation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantLibrary.Market_Data.VolatilitySurface
{
    public class VolSurfLinearInterp : VolatilitySurface
    {
        public VolSurfLinearInterp(
            string name,
            DateTime valuationDate, 
            IEnumerable<DateTime> expiries, 
            IEnumerable<IEnumerable<double>> strikes, 
            IEnumerable<IEnumerable<double>> impliedVols) : 
            base(name, valuationDate, expiries, strikes, impliedVols)
        {
            for(int i = 0; i < expiries.Count(); i++)
            {
                VolatilitySmiles.Add(
                    new VolSmileLinearInterp(
                        ValuationDate,
                        expiries.ElementAt(i),
                        strikes.ElementAt(i),
                        impliedVols.ElementAt(i))
                    );
            }
        }
    }

    public class VolSmileLinearInterp : VolatilitySmile
    {
        public LinearSpline LinearInterpolator;
        public VolSmileLinearInterp(
            DateTime valuationDate, 
            DateTime optionExpiry, 
            IEnumerable<double> strikes, 
            IEnumerable<double> volatilities) : 
            base(valuationDate, optionExpiry, strikes, volatilities)
        {
        }

        public override void Calibrate()
        {
            LinearInterpolator = LinearSpline.Interpolate(Strikes, MarketVolatilities);
        }

        public override double ImpliedVol(double strike)
        {
            return LinearInterpolator.Interpolate(strike);
        }
    }
}
