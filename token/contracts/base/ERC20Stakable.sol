pragma solidity ^0.8.0;

import "@openzeppelin/contracts/token/ERC20/ERC20.sol";
import "../Interfaces/IStakable.sol";
import "../utils/NominatingStakeholders.sol";

contract ERC20Stakable is IStakable, ERC20 {
    uint256 immutable _minStakedAmountForValidation;
    Stakeholders.Stakeholder[] private _stakeholders;
    mapping(address => uint256) private _stakeholdersIndexes;

    constructor(
        string memory name_,
        string memory symbol_,
        uint256 minStakedAmountForValidation_
    ) ERC20(name_, symbol_) {
        _minStakedAmountForValidation = minStakedAmountForValidation_;
    }

    modifier notNullAddress(address address_) {
        require(address_ != address(0));
        _;
    }

    function getStakedAmount(address address_)
        external
        view
        override
        notNullAddress(address_)
        returns (uint256)
    {
        uint256 stakeholderIndex = _getStakeholderIndex(address_);
        Stakeholders.Stakeholder storage stakeholder = _stakeholders[
            stakeholderIndex
        ];
        return stakeholder.totalAmount;
    }

    function stake(uint256 amount_) external override {
        address stakeholderAddress = msg.sender;

        //fisrst check if the caller has at least amount_ tokens
        uint256 balance = this.balanceOf(stakeholderAddress);

        require(balance >= amount_, "Insufficient balance for staking");
        //second check if stakeholder exist
        uint256 stakeholderIndex = _getStakeholderIndex(stakeholderAddress);
        if (stakeholderIndex == 0) {
            //stakeholder does not exists. Create new one and stake the amout
            _createNewStakeholder(stakeholderAddress, amount_);
        } else {
            _stakeholderUpdateOwnStakedAmount(stakeholderIndex, amount_);
        }

        //burn tokens from balance
        _burn(stakeholderAddress, amount_);
    }

    function unstake(uint256 amount_) external override {}

    function _getStakeholderIndex(address address_)
        internal
        view
        virtual
        notNullAddress(address_)
        returns (uint256)
    {
        uint256 stakeholderIndex = _stakeholdersIndexes[address_];
        if (stakeholderIndex == 0) return 0;
        require(
            stakeholderIndex <= _stakeholders.length - 1,
            "Invalid index for stakeholders collection"
        );

        return stakeholderIndex;
    }

    function _createNewStakeholder(address address_, uint256 amount_)
        internal
        virtual
    {
        bool canValidate = amount_ >= _minStakedAmountForValidation;
        uint256 newStakeholderIndex = _stakeholders.length;

        if (newStakeholderIndex == 0) {
            // this is very first stakeholder. Create a fake one at index 0.
            _stakeholders.push(
                Stakeholders.Stakeholder({
                    user: address(0),
                    totalAmount: 0,
                    ownedAmount: 0,
                    nominatedAmount: 0,
                    nominatorsCount: 0,
                    canValidate: false
                })
            );
        }

        newStakeholderIndex += 1;

        _stakeholders.push(
            Stakeholders.Stakeholder({
                user: address_,
                totalAmount: amount_,
                ownedAmount: amount_,
                nominatedAmount: 0,
                nominatorsCount: 0,
                canValidate: canValidate
            })
        );

        _stakeholdersIndexes[address_] = newStakeholderIndex;
    }

    function _stakeholderUpdateOwnStakedAmount(
        uint256 stakeholderIndex_,
        uint256 amount_
    ) internal virtual {
        Stakeholders.Stakeholder storage stakeHolder = _stakeholders[
            stakeholderIndex_
        ];

        stakeHolder.totalAmount += amount_;
        stakeHolder.ownedAmount += amount_;

        if (!stakeHolder.canValidate) {
            //check if he can now
            bool canValidate = stakeHolder.totalAmount >=
                _minStakedAmountForValidation;

            if (canValidate) {
                stakeHolder.canValidate = true;
            }
        }
    }
}
