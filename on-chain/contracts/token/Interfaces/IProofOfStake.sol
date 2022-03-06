pragma solidity ^0.8.0;

import "../utils/Eras.sol";
import "../utils/NominatingStakeholders.sol";

interface IProofOfStake {
    function getValidatorsCount() external view returns (uint256);

    function getStakeholders()
        external
        view
        returns (Stakeholders.Stakeholder[] memory stakeholders);
}
