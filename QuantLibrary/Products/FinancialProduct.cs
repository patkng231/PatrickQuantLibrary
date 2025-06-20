using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuantLibrary.Utilities;

namespace QuantLibrary.Products
{
    public abstract class FinancialProduct
    {
        public string Name;
        public AssetClass AssetClass;

        public FinancialProduct(string name, AssetClass assetClass)
        {
            Name = name;
            AssetClass = assetClass;
        }
    }

    public abstract class IRFinancialProduct : FinancialProduct
    {
        public IRFinancialProduct(string name) : base(name, AssetClass.IR)
        {

        }
    }
}


