pragma solidity ^0.8.0;

import "../Interfaces/IAggregator.sol";

contract GoldPriceAggregator is IAggregator {
    constructor(string memory assetCode, string memory currencyCode) {}

    function getPrice()
        external
        override
        returns (
            uint256 price,
            bytes memory assetCode,
            bytes memory currencyCode
        )
    {
        //TODO:
        return (1800 * 10**18, "XAU", "USDT");
    }
}
