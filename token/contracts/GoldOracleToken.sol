pragma solidity ^0.8.0;

import "./base/ERC20Stakable.sol";

contract GoldOracleToken is ERC20Stakable {
    uint256 private constant _minAmountStakedForValidation = 20000 * 10**18;
    address private _owner;

    constructor()
        ERC20Stakable("GoldOracleToken", "GOT", _minAmountStakedForValidation)
    {
        _owner = msg.sender;
    }

    function getOwnerAddress() public view returns (address) {
        return _owner;
    }
}
