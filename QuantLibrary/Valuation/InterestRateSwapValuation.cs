using QuantLibrary.DiscountCurves;
using QuantLibrary.Products.IR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantLibrary.Valuation
{
    public class InterestRateSwapValuation : ValuationBase
    {
        public static double Price(DateTime valuationDate, InterestRateSwap product, DiscountCurve discountCurve, DiscountCurve forecastCurve)
        {
            List<ForwardRateAgreement> remainingFRAs = product.RemainingFRAs(valuationDate);

            double value = 0.0;

            foreach (ForwardRateAgreement fra in remainingFRAs)
            {
                value += ForwardRateAgreementValuation.Price(valuationDate, fra, discountCurve, forecastCurve);
            }

            return value;
        }

        public static double Price(DateTime valuationDate, InterestRateSwap product, DiscountCurve discountCurve)
        {
            return Price(valuationDate, product, discountCurve, discountCurve);
        }

    }
}
