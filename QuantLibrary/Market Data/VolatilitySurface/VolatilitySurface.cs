using MathNet.Numerics.Interpolation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantLibrary.Market_Data.VolatilitySurface
{
    public abstract class VolatilitySurface : MarketData
    {

        public readonly DateTime ValuationDate;
        protected List<VolatilitySmile> VolatilitySmiles = [];
        protected bool Calibrated = false;

        public readonly List<DateTime> OptionExpiries;
        public readonly List<double> OptionExpiriesTime;
        public readonly List<List<double>> Strikes;
        public readonly List<List<double>> ImpliedVolatilities;  
        public VolatilitySurface(
            string name,
            DateTime valuationDate,
            IEnumerable<DateTime> expiries,
            IEnumerable<IEnumerable<double>> strikes,
            IEnumerable<IEnumerable<double>> impliedVols) : base(name)
        {
            ValuationDate = valuationDate;
            OptionExpiries = [.. expiries];
            OptionExpiriesTime = [.. expiries.Select(x => (x - ValuationDate).TotalDays / 365.0)];
            Strikes = [.. strikes.Select(x => x.ToList())];
            ImpliedVolatilities = [.. impliedVols.Select(x => x.ToList())];

        }

        public virtual void Calibrate()
        {
            VolatilitySmiles.ForEach(v => v.Calibrate());
            Calibrated = true;
        }

        public virtual double ImpliedVol(DateTime expiry, double strike)
        {
            if (!Calibrated)
            {
                throw new Exception("Vol Surface has not been calibrated. Run Calibrate() method first");
            }
            if (expiry <= ValuationDate)
            {
                throw new Exception("Expiry must be after the valuation date");
            }

            if (expiry <= OptionExpiries.First())
            {
                return VolatilitySmiles[0].ImpliedVol(strike);
            }
            if (expiry >= OptionExpiries.Last())
            {
                return VolatilitySmiles.Last().ImpliedVol(strike);
            }

            int idx = ~OptionExpiries.BinarySearch(expiry);

            double expiry1 = OptionExpiriesTime[idx - 1];
            double expiry2 = OptionExpiriesTime[idx];

            double vol1 = VolatilitySmiles[idx - 1].ImpliedVol(strike);
            double vol2 = VolatilitySmiles[idx].ImpliedVol(strike);

            LinearSpline linearInterp = LinearSpline.Interpolate([expiry1, expiry2],
                [vol1 * vol1 * expiry1, vol2 * vol2 * expiry2]);

            double timeToExpiry = (expiry - ValuationDate).TotalDays / 365.0;
            double varInterp = linearInterp.Interpolate(timeToExpiry);
            double vol = Math.Sqrt(varInterp / timeToExpiry);

            return vol;
        }
    }

    public abstract class VolatilitySmile
    {
        public readonly DateTime ValuationDate;
        public readonly DateTime OptionExpiry;
        public readonly List<double> Strikes;
        public readonly List<double> MarketVolatilities;
        public VolatilitySmile(DateTime valuationDate, DateTime optionExpiry, IEnumerable<double> strikes, IEnumerable<double> volatilities)
        {
            ValuationDate = valuationDate;
            OptionExpiry = optionExpiry;
            Strikes = [.. strikes];
            MarketVolatilities = [.. volatilities];
        }

        public abstract double ImpliedVol(double strike);

        public abstract void Calibrate();

    }

}
