# Patrick Quant Library
This repo is aiming to implement various quantitative models commonly used in quantitative finance to price and risk financial instruments.

# Swap Curve Construction
Probably one of the most important concepts of quantitative finance is curve construction. This is because almost all financial derivatives require some sort of discount curve (yield curve) to price (ex. swaps, CDS, FX swaps, bonds, options, futures, etc.). Often when learning financial mathematics, we assume that the a risk-free discount curve is provided to us but in the reality, it must be inferred or bootstrapped from market quotes.

Arguely, the most important "risk-free" discount curve is the USD SOFR swap curve. This is the discount curve that is implied (bootstrapped) from quoted USD SOFR swap rates for various expiries (1M, 1Y, 2Y, 3Y, etc.). Other currencies have their 
