pragma solidity ^0.8.0;

import "../Interfaces/IAggregator.sol";
import "../resolvers/interfaces/IBaseResolver.sol";
import "../utils/Rounds.sol";

contract GoldPriceAggregator is IAggregator {
    IBaseResolver _baseResolver;

    //address must be the address of GoldPriceResolver

    constructor(address baseResolverAddress) {
        _baseResolver = IBaseResolver(baseResolverAddress);
    }

    function getPrice()
        external
        view
        override
        returns (
            uint256 price,
            string memory assetCode,
            string memory currencyCode
        )
    {
        Rounds.Round memory lastRound = _baseResolver.getLatestValidData();
        return (lastRound.price, lastRound.assetCode, lastRound.currencyCode);
    }
}
