using QuantLibrary.DiscountCurves;
using QuantLibrary.Utilities;
using QuantLibrary.Products.IR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantLibrary.Valuation
{
    public class ForwardRateAgreementValuation : ValuationBase
    {
        public static double Price(DateTime valuationDate, ForwardRateAgreement product, DiscountCurve discountCurve, DiscountCurve forecastCurve)
        {
            if (valuationDate > product.MaturityDate)
            {
                throw new Exception("FRA maturity date is after the valuation date");
            }

            double forwardRate = product.ImpliedForwardPeriodRate(valuationDate, forecastCurve);

            int payReceiveFlag = product.PayReceive == PayRecieve.Pay ? 1 : -1;
            double t = (product.MaturityDate - valuationDate).TotalDays / 365.0;

            return payReceiveFlag * product.Notional * (forwardRate - product.FixedRate) * discountCurve.Df(t);
        }

        public static double Price(DateTime valuationDate, ForwardRateAgreement product, DiscountCurve discountCurve)
        {
            return Price(valuationDate, product, discountCurve, discountCurve);
        }
    }
}
