pragma solidity ^0.8.0;

import "../resolvers/interfaces/IBaseResolver.sol";
import "../utils/Rounds.sol";
import "../../token/Interfaces/IErasMonitor.sol";
import "../../token/utils/Eras.sol";

contract Timer {
    uint256 constant UTC_TIMESTAMP_ONE_MINUTE = 60;
    uint256 immutable _timeStepForNewPriceRound = 3 * UTC_TIMESTAMP_ONE_MINUTE;
    uint256 immutable _timeStepForNewEra = 48 * 60 * UTC_TIMESTAMP_ONE_MINUTE; // new era on each 48 hours. should be much lower for testing
    IBaseResolver private immutable _resolver;
    IErasMonitor private immutable _erasMonitor;
    uint256 immutable _initialTimeStamp;
    uint256 private lastRoundsCount = 0;
    uint256 private lastEraCount = 0;
    address private _owner;

    event StarNewPriceRound(uint256 utcTimeStamp);
    event StartNewEra(uint256 utcTimeStamp_, bytes32 newEraId_);

    constructor(
        address resolverAddress_,
        address erasMonitor_,
        uint256 initialTimeStamp_
    ) {
        _initialTimeStamp = initialTimeStamp_;
        _resolver = IBaseResolver(resolverAddress_);
        _erasMonitor = IErasMonitor(erasMonitor_);
        _owner = msg.sender;
    }

    modifier onlyOwner() {
        require(msg.sender == _owner, "Access Denied");
        _;
    }

    function newPriceRoundTimeElapsed(uint256 utcTimeStamp_) public onlyOwner {
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

    function newEraTimeElapsed(uint256 utcTimeStamp_) public onlyOwner {
        uint256 erasCount = _erasMonitor.getErasCount();
        if (erasCount == 0) {
            bytes32 initialEraId = keccak256(
                abi.encodePacked(msg.sender, utcTimeStamp_, msg.sender)
            );
            emit StartNewEra(utcTimeStamp_, initialEraId);
            return;
        }
        if (
            erasCount * _timeStepForNewEra > utcTimeStamp_ &&
            lastEraCount != erasCount
        ) {
            //emit event
            Eras.Era memory currentEra = _erasMonitor.getCurrentEra();

            bytes32 newEraId = keccak256(
                abi.encodePacked(
                    currentEra.id,
                    currentEra.startDate,
                    currentEra.chairman
                )
            );
            emit StartNewEra(utcTimeStamp_, newEraId);
        }
        if (lastEraCount == erasCount + 1) {
            lastEraCount++;
        }
    }

    function testStartNewEra() public onlyOwner {
        uint256 erasCount = _erasMonitor.getErasCount();
        if (erasCount == 0) {
            bytes32 initialEraId = keccak256(
                abi.encodePacked(msg.sender, "3123123123", msg.sender)
            );
            emit StartNewEra(3123123123, initialEraId);
            return;
        }
    }

    function testStartNewPriceRound() public onlyOwner {
        emit StarNewPriceRound(123213123);
    }
}
