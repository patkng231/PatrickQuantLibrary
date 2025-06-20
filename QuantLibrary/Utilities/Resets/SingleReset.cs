using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantLibrary.Utilities.Resets
{
    public struct SingleReset
    {
        public DateTime ResetDate;
        public DateTime EndDate;
        public double? KnownRate;
        public readonly double DayCountFraction => (EndDate - ResetDate).TotalDays / 365.0;

    }
}
