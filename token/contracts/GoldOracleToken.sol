pragma solidity ^0.8.0;

import "@openzeppelin/contracts/token/ERC20/ERC20.sol";
import "./Interfaces/IStakable.sol";

contract GoldOracleToken is IStakable, ERC20 {
    struct Stakeholder {
        address user;
        uint256 totalAmount;
        uint256 ownedAmount;
        uint256 nominatedAmount;
        uint256 nominatorsCount;
        mapping(address => uint256) nominatorsBalances;
        bool canValidate;
    }

    uint256 constant _initialSupply = 200000000 * 10**18;
    address private _owner;
    Stakeholder[] private _stakeholders;
    mapping(address => uint256) private _stakeholdersIndexes;

    modifier notNullAddress(address address_) {
        require(address_ != address(0));
        _;
    }

    constructor() ERC20("GoldOracleToken", "GOT") {
        _mint(msg.sender, _initialSupply);
        _owner = msg.sender;
    }

    function getOwnerAddress() public view returns (address) {
        return _owner;
    }

    function getStakedAmount(address address_)
        external
        view
        override
        notNullAddress(address_)
        returns (uint256)
    {
        uint256 stakeholderIndex = _stakeholdersIndexes[address_];
        if (stakeholderIndex == 0) return 0;
        require(
            stakeholderIndex <= _stakeholders.length - 1,
            "Invalid index for stakeholders collection"
        );
        Stakeholder storage stakeholder = _stakeholders[stakeholderIndex];
        return stakeholder.totalAmount;
    }

    function stake(uint256 amount_) external override returns (bool) {}

    function unstake(uint256 amount_) external override returns (bool) {}
}
