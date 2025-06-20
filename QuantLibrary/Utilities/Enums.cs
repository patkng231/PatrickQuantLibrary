using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantLibrary.Utilities
{
    public enum AssetClass
    {
        None,
        FX,
        EQ,
        IR,
        CR
    }

    public enum PayRecieve
    {
        Pay,
        Receive
    }

    public enum Frequency
    {
        _1D,
        _3M,
        _6M
    }

    public enum SwapTenors
    {
        _1D,
        _1W,
        _2W,
        _3W,
        _1M,
        _2M,
        _3M,
        _6M,
        _9M,
        _1Y,
        _2Y,
        _3Y,
        _4Y,
        _5Y,
        _7Y,
        _10Y
    }

    public enum CurveInterpolationType
    {
        LogLinearDf,
        CubicSplineDf,
        ConstantRate
    }

}
