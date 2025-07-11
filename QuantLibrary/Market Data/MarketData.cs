using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantLibrary.Market_Data
{
    public abstract class MarketData
    {
        public string Name;

        public MarketData(string name)
        {
            Name = name;
        }
    }
}
