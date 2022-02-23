pragma solidity ^0.8.0;

import "../resolvers/GoldPriceResolver.sol";
import "../utils/Rounds.sol";

contract Timer {
    uint256 constant UTC_TIMESTAMP_ONE_MINUTE = 60;
    GoldPriceResolver _resolver;
    uint256 immutable _initialTimeStamp;

    constructor(address resolverAddress_) {
        _initialTimeStamp = block.timestamp;
        _resolver = GoldPriceResolver(resolverAddress_);
    }

    function ping(uint256 utcTimeStamped_) public {
        emit CheckTimeStamped(utcTimeStamped_);
    }

    function getInitialTimeStamps() public view returns (uint256) {
        return _initialTimeStamp;
    }

    function tryStartNewRound(uint256 price_) public {
        //check if round should be created!
        _resolver.startNewPriceRound(msg.sender, price_);
    }

    event CheckTimeStamped(uint256 utcTimeStamped_);
}
