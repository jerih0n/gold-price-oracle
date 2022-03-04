pragma solidity ^0.8.0;

import "@openzeppelin/contracts/token/ERC20/ERC20.sol";
import "../Interfaces/IStakable.sol";
import "../utils/NominatingStakeholders.sol";
import "../utils/Eras.sol";
import "../Interfaces/IProofOfStake.sol";
import "../Interfaces/IErasMonitor.sol";

contract ERC20Stakable is IErasMonitor, IProofOfStake, IStakable, ERC20 {
    uint256 immutable _minStakedAmountForValidation;
    uint256 private _validatorsCount;
    uint256 private _erasCount;
    Stakeholders.Stakeholder[] private _stakeholders;
    mapping(address => uint256) private _validatorsStakeholderIndexes;
    mapping(address => uint256) private _stakeholdersIndexes;
    mapping(bytes32 => Eras.Era) private _eras;
    Eras.Era private _currentErra;

    constructor(
        string memory name_,
        string memory symbol_,
        uint256 minStakedAmountForValidation_
    ) ERC20(name_, symbol_) {
        _minStakedAmountForValidation = minStakedAmountForValidation_;
        _validatorsCount = 0;
        _erasCount = 0;
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
        if (stakeholderIndex == 0) {
            //no stakeholder registered
            return 0;
        }
        Stakeholders.Stakeholder storage stakeholder = _stakeholders[
            stakeholderIndex
        ];
        return stakeholder.totalAmount;
    }

    function getStakeholderInformation(address address_)
        external
        view
        override
        returns (
            uint256,
            uint256,
            uint256,
            uint256,
            bool
        )
    {
        uint256 stakeholderIndex = _getStakeholderIndex(address_);

        require(stakeholderIndex > 0, "Address not linked to any stakeholder");

        Stakeholders.Stakeholder storage stakeholder = _stakeholders[
            stakeholderIndex
        ];

        return (
            stakeholder.totalAmount,
            stakeholder.ownedAmount,
            stakeholder.nominatedAmount,
            stakeholder.nominatorsCount,
            stakeholder.canValidate
        );
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

    function unstake(uint256 amount_) external override {
        address stakeholderAddress = msg.sender;
        uint256 stakeholderIndex = _stakeholdersIndexes[stakeholderAddress];
        if (stakeholderIndex == 0) {
            // this user no longer have any staked ammount
            return;
        }

        Stakeholders.Stakeholder storage stakeholder = _stakeholders[
            stakeholderIndex
        ];
        require(
            stakeholder.totalAmount >= amount_,
            "Insufficient balance for unstaking"
        );

        uint256 newAmount = stakeholder.totalAmount -= amount_;

        stakeholder.totalAmount = newAmount;
        stakeholder.ownedAmount = newAmount;

        if (newAmount >= _minStakedAmountForValidation) {
            stakeholder.canValidate = true;
        } else {
            stakeholder.canValidate = false;
            _validatorsCount--;
        }

        //mint amount_ tokes to address
        _mint(stakeholderAddress, amount_);
    }

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
            newStakeholderIndex++;
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
        _tryRecordStakeholderAsNewValidator(
            canValidate,
            address_,
            newStakeholderIndex
        );
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

                _tryRecordStakeholderAsNewValidator(
                    canValidate,
                    stakeHolder.user,
                    stakeholderIndex_
                );
            }
        }
    }

    function _tryRecordStakeholderAsNewValidator(
        bool canValidate_,
        address address_,
        uint256 validatorIndex_
    ) internal returns (bool) {
        if (!canValidate_) {
            return false;
        }
        uint256 validatorIndex = _validatorsStakeholderIndexes[address_];
        if (validatorIndex == 0) {
            //we don't have any valid validator
            _validatorsStakeholderIndexes[address_] = validatorIndex_;
            _validatorsCount++;
        }
        //else validator already exist in records
        return true;
    }

    //** for INTERFACE !!! */
    function getValidatorsCount() external view override returns (uint256) {
        return _validatorsCount;
    }

    function getCurrentEra()
        external
        view
        override
        returns (Eras.Era memory currentEra)
    {
        return _currentErra;
    }

    function getErasCount() external view override returns (uint256) {
        return _erasCount;
    }

    function electNewChairman() external returns (address) {}

    function startNewEra() external returns (bytes32) {}
}
