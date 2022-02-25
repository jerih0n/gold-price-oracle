pragma solidity ^0.8.0;

import "../utils/Rounds.sol";
import "../utils/Votes.sol";
import "./base/BasePriceResolver.sol";

//PoC. Big Refactoring will happened!
contract GoldPriceResolver is BasePriceResolver {
    uint256 constant MAXIMUM_ALLOWED_PRICE_DEVIATION_IN_PERCENTS = 3;
    string constant GOLD_SYMBOL = "XUA"; //TODO; move to separate contract when refactor
    string constant USDT_SYMBOL = "USDT";

    constructor()
        BasePriceResolver(
            GOLD_SYMBOL,
            USDT_SYMBOL,
            MAXIMUM_ALLOWED_PRICE_DEVIATION_IN_PERCENTS
        )
    {}
}
