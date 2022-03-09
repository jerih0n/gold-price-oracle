pragma solidity ^0.8.0;

import "../utils/Eras.sol";
import "../utils/NominatingStakeholders.sol";

interface IProofOfStake {
    function getValidatorsCount() external view returns (uint256);

    function voteForProposedEra(bytes32 eraId, bool approves) external;

    function endEra(uint256 timestamp) external;

    function proposeNewEra(
        bytes32 eraId_,
        address chairman_,
        address[] memory coucil_,
        uint256 utcTimeStamp_,
        uint256 calculatedSeed_
    ) external;
}
