pragma solidity ^0.8.0;

import "../resolvers/GoldPriceResolver.sol";
import "../utils/Rounds.sol";
import "../../token/Interfaces/IErasMonitor.sol";

contract Timer {
    uint256 constant UTC_TIMESTAMP_ONE_MINUTE = 60;
    uint256 immutable _timeStepForNewPriceRound = 3 * UTC_TIMESTAMP_ONE_MINUTE;
    uint256 immutable _timeStepForNewEra = 48 * 60 * UTC_TIMESTAMP_ONE_MINUTE; // new era on each 48 hours. should be much lower for testing
    GoldPriceResolver private immutable _resolver;
    IErasMonitor private immutable _proofOfStake;
    uint256 immutable _initialTimeStamp;
    uint256 private lastRoundsCount = 0;
    uint256 private lastEraCount = 0;

    event StarNewPriceRound(uint256 utcTimeStamp);
    event StartNewEra(uint256 utcTimeStamp_);

    constructor(
        address resolverAddress_,
        address proofOfStakeContract,
        uint256 initialTimeStamp
    ) {
        _initialTimeStamp = initialTimeStamp;
        _resolver = GoldPriceResolver(resolverAddress_);
        _proofOfStake = IErasMonitor(proofOfStakeContract);
    }

    function newPriceRoundTimeElapsed(uint256 utcTimeStamp_) public {
        uint256 roundsCount = _resolver.getRoundCount();
        if (
            roundsCount * _timeStepForNewPriceRound >= utcTimeStamp_ &&
            lastRoundsCount != roundsCount
        ) {
            //new price round start
            emit StarNewPriceRound(utcTimeStamp_);
        }
        if (lastRoundsCount == roundsCount + 1) {
            lastRoundsCount++;
        }
    }

    function newEraTimeElapsed(uint256 utcTimeStamp_) public {
        uint256 erasCount = _proofOfStake.getErasCount();
        if (
            erasCount * _timeStepForNewEra > utcTimeStamp_ &&
            lastEraCount != erasCount
        ) {
            //emit event
            emit StartNewEra(utcTimeStamp_);
        }
        if (lastEraCount == erasCount + 1) {
            lastEraCount++;
        }
    }
}
