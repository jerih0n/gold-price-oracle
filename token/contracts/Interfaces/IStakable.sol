pragma solidity ^0.8.0;

interface IStakable {
    function getStakedAmount(address address_) external view returns (uint256);

    function stake(uint256 amount_) external returns (bool);

    function unstake(uint256 amount_) external returns (bool);
}
