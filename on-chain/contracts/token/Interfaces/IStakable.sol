pragma solidity ^0.8.0;

import "../utils/NominatingStakeholders.sol";

interface IStakable {
    function getStakedAmount(address address_) external view returns (uint256);

    function stake(uint256 amount_) external;

    function unstake(uint256 amount_) external;

    function getStakeholderInformation(address address_)
        external
        view
        returns (
            uint256,
            uint256,
            uint256,
            uint256,
            bool
        );

    function getStakeholders()
        external
        view
        returns (Stakeholders.Stakeholder[] memory stakeholders);
}
