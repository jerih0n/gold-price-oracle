pragma solidity ^0.8.0;

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
}
