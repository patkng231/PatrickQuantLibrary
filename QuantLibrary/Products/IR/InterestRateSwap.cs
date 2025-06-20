using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuantLibrary.Products;
using QuantLibrary.Utilities;

namespace QuantLibrary.Products.IR
{
    public class InterestRateSwap : IRFinancialProduct
    {
        public readonly List<ForwardRateAgreement> ForwardRateAgreements = [];
        public readonly Frequency Frequency;
        public readonly PayRecieve PayReceive;
        public readonly double SwapRate;

        public readonly DateTime SwapStartDate;
        public readonly DateTime SwapEndDate;

        public List<ForwardRateAgreement> RemainingFRAs(DateTime date)
        {
            return [.. ForwardRateAgreements.Where(fra => fra.MaturityDate >= date)];
        }

        public InterestRateSwap(string name, IEnumerable<double> notional, IEnumerable<DateTime> fixingDates, IEnumerable<DateTime> paymentDates, Frequency frequency,
            double swapRate, PayRecieve payRecieve, Dictionary<DateTime, double> knownRatesDict) : base(name)
        {
            if (fixingDates.Count() != paymentDates.Count())
            {
                throw new Exception("Fixing and Payment date counts do not match");
            }
            if (fixingDates.Count() != notional.Count())
            {
                throw new Exception("Fixing and Notional counts do not match");
            }

            SwapRate = swapRate;

            SwapStartDate = fixingDates.First();
            SwapEndDate = paymentDates.Last();
            PayReceive = payRecieve;
            Frequency = frequency;

            int i = 0;

            foreach (var (fix, pay) in fixingDates.Zip(paymentDates))
            {
                if (fix > pay)
                {
                    throw new Exception("Fixing date is after payment date");
                }

                ForwardRateAgreements.Add(new ForwardRateAgreement(name + $"_FRA_{i}", notional.ElementAt(i), fix, pay, frequency, swapRate, PayReceive, knownRatesDict));
                i++;
            }
        }

        public InterestRateSwap(string name, double notional, IEnumerable<DateTime> fixingDates, IEnumerable<DateTime> paymentDates, Frequency frequency,
            double swapRate, PayRecieve payRecieve, Dictionary<DateTime, double> knownRatesDict)
            : this(name, [.. Enumerable.Repeat(notional, fixingDates.Count())], fixingDates, paymentDates,
                  frequency, swapRate, payRecieve, knownRatesDict)
        {
        }
    }
}
