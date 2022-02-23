pragma solidity ^0.8.0;

import "../resolvers/GoldPriceResolver.sol";
import "../utils/Rounds.sol";

contract Timer {
    uint256 constant _utcTimestampOneMinute = 60;
    GoldPriceResolver _resolver;
    uint256 immutable _initialTimeStamp;

    constructor(uint256 initialTimeStamp_, address resolverAddress_) {
        _initialTimeStamp = initialTimeStamp_;
        _resolver = GoldPriceResolver(resolverAddress_);
    }

    function ping(uint256 utcTimeStamped_) public {
        emit CheckTimeStamped(utcTimeStamped_);
    }

    function getInitialTimeStamps() public view returns (uint256) {
        return _initialTimeStamp;
    }

    function tryStartNewRound(uint256 price_) public {

        Rounds.Round memory = _resolver.getLatestRoundData();
        //check if round should be created!
        _resolver.startNewPriceRound(msg.sender, price_);
    }

    event CheckTimeStamped(uint256 utcTimeStamped_);
}
