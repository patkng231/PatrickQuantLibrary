using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuantLibrary.DiscountCurves;
using QuantLibrary.Products;
using QuantLibrary.Utilities;
using QuantLibrary.Utilities.DateSchedules;
using QuantLibrary.Utilities.Resets;

namespace QuantLibrary.Products.IR
{
    public class ForwardRateAgreement : IRFinancialProduct
    {
        public readonly double Notional;
        public readonly DateTime FixingDate;
        public readonly ResetSchedule ResetPeriod;
        public readonly DateTime MaturityDate;
        public readonly double FixedRate;
        public readonly double DayCountFraction;
        public readonly PayRecieve PayReceive;

        public ForwardRateAgreement(string name, double notional, DateTime fixingDate, DateTime maturityDate, Frequency frequency,
            double fixedRate, PayRecieve payReceive, Dictionary<DateTime, double> knownRates) : base(name)
        {
            Notional = notional;
            FixingDate = fixingDate;
            MaturityDate = maturityDate;
            FixedRate = fixedRate;

            ResetPeriod = DateSchedulesGenerator.GenerateResetSchedule(fixingDate, maturityDate, frequency, knownRates);

            DayCountFraction = (MaturityDate - FixingDate).TotalDays / 365.0;
            PayReceive = payReceive;
        }

        public double ImpliedForwardPeriodRate(DateTime valuationDate, DiscountCurve discountCurve)
        {
            double simpleCompoundedRate = 1.0;
            foreach (SingleReset reset in ResetPeriod.SingleResets)
            {
                double resetRate;
                if (valuationDate > reset.ResetDate)
                {
                    if (reset.KnownRate is null)
                    {
                        throw new Exception($"Known Rate is not defined for {reset.ResetDate} which is prior to {valuationDate}");
                    }
                    simpleCompoundedRate *= (1.0 + reset.KnownRate.Value * reset.DayCountFraction);
                }
                else
                {
                    double t0 = (reset.ResetDate - valuationDate).TotalDays / 365.0;
                    double t1 = (reset.EndDate - valuationDate).TotalDays / 365.0;
                    double forwardRate = (discountCurve.Df(t0) / discountCurve.Df(t1) - 1.0) / reset.DayCountFraction;
                    simpleCompoundedRate *= (1.0 + forwardRate * reset.DayCountFraction);
                }

            }

            double finalForwardRate = (simpleCompoundedRate - 1.0) / ResetPeriod.ResetPeriodDayCountFraction;

            return finalForwardRate;
        }

    }
}
