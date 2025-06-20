using QuantLibrary.DiscountCurves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantLibrary.Utilities.Resets
{
    public struct ResetSchedule
    {
        public readonly List<SingleReset> SingleResets;
        public readonly DateTime ResetPeriodStart => SingleResets.First().ResetDate;
        public readonly DateTime ResetPeriodEnd => SingleResets.Last().EndDate;

        public readonly double ResetPeriodDayCountFraction => (ResetPeriodEnd - ResetPeriodStart).TotalDays / 365.0;

        public ResetSchedule(IEnumerable<SingleReset> singleResets) 
        {
            SingleResets = [.. singleResets];
        }

    }
}
