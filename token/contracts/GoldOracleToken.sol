pragma solidity ^0.8.0;

import "@openzeppelin/contracts/token/ERC20/ERC20.sol";

contract GoldOracleToken is ERC20 {
    uint256 constant _initialSupply = 200000000 * 10**18;
    address private _owner;

    constructor() ERC20("GoldOracleToken", "GOT") {
        _mint(msg.sender, _initialSupply);
        _owner = msg.sender;
    }

    function getOwnerAddress() public view returns (address) {
        return _owner;
    }
}
