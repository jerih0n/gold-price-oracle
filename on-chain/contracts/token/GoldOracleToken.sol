pragma solidity ^0.8.0;

import "./base/ERC20ProofOfStake.sol";

contract GoldOracleToken is ERC20ProofOfStake {
    uint256 private constant MIN_STAKED_AMOUNT_REQUIRED_FOR_VALIDATION =
        20000 * 10**18;
    uint256 private constant REWARD_AMOUNT_IN_TOKENS_AFTER_EACH_ERA =
        1000 * 10**18;
    address private _owner;

    constructor()
        ERC20ProofOfStake(
            "GoldOracleToken",
            "GOT",
            MIN_STAKED_AMOUNT_REQUIRED_FOR_VALIDATION,
            REWARD_AMOUNT_IN_TOKENS_AFTER_EACH_ERA
        )
    {
        _owner = msg.sender;
        _mint(msg.sender, MIN_STAKED_AMOUNT_REQUIRED_FOR_VALIDATION * 10); // for testing
    }

    function getOwnerAddress() public view returns (address) {
        return _owner;
    }
}
