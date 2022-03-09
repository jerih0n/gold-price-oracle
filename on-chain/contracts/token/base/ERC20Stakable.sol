pragma solidity ^0.8.0;

import "@openzeppelin/contracts/token/ERC20/ERC20.sol";
import "../Interfaces/IStakable.sol";
import "../utils/NominatingStakeholders.sol";
import "../utils/Eras.sol";
import "../Interfaces/IProofOfStake.sol";
import "../Interfaces/IErasMonitor.sol";
import "../utils/ErasVotes.sol";

contract ERC20Stakable is IErasMonitor, IProofOfStake, IStakable, ERC20 {
    uint256 constant MAX_COUICIL_MEMBERS_COUNT = 10;

    uint256 immutable _rewardForValidatorsAfterEachEra;
    uint256 immutable _minStakedAmountForValidation;
    uint256 private _validatorsCount;
    uint256 private _erasCount;
    Stakeholders.Stakeholder[] private _stakeholders;
    mapping(address => uint256) private _validatorsStakeholderIndexes;
    mapping(address => uint256) private _stakeholdersIndexes;
    mapping(bytes32 => Eras.Era) private _eras;
    mapping(bytes32 => mapping(address => ErasVotes.ErasVote)) _eraValidatorVotes;
    mapping(bytes32 => address[]) private _erasCouncil;
    Eras.Era private _currentEra;

    constructor(
        string memory name_,
        string memory symbol_,
        uint256 minStakedAmountForValidation_,
        uint256 rewardForValidatorsAfterEachEra_
    ) ERC20(name_, symbol_) {
        _minStakedAmountForValidation = minStakedAmountForValidation_;
        _validatorsCount = 0;
        _erasCount = 0;
        _rewardForValidatorsAfterEachEra = rewardForValidatorsAfterEachEra_;
    }

    event NewEraElecationProposal(
        bytes32 eraId,
        address chairman,
        address[] council,
        uint256 validatorsCount
    );

    event NewEraElectionComplited(Eras.Era era);

    modifier notNullAddress(address address_) {
        require(address_ != address(0));
        _;
    }

    modifier onlyValidatorAction() {
        uint256 validatorNodeIndex = _validatorsStakeholderIndexes[msg.sender];
        require(validatorNodeIndex != 0, "Voting forbidden for non-validators");
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

    function getStakeholders()
        external
        view
        override
        returns (Stakeholders.Stakeholder[] memory stakeholders)
    {
        return _stakeholders;
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
                    canValidate: false,
                    notCollectedTokenRewards: 0
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
                canValidate: canValidate,
                notCollectedTokenRewards: 0
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
        return _currentEra;
    }

    function getErasCount() external view override returns (uint256) {
        return _erasCount;
    }

    function endCurrentEra() external {
        address caller = msg.sender;
        Eras.Era memory lastEra = _getCurrentEraInternal();
        //TODO: validation for time elapsed
        require(
            caller == lastEra.chairman,
            "Only chairman can end current era"
        );
        _payReward(
            lastEra.colectedFeesAmount + _rewardForValidatorsAfterEachEra,
            caller
        );
    }

    function proposeNewEra(
        bytes32 eraId_,
        address chairman_,
        address[] memory coucil_,
        uint256 utcTimeStamp_
    ) external {
        address caller = msg.sender;
        if (_currentEra.chairman != address(0)) {
            //this is not the first era. Only previous chairman can propose new election
            require(
                caller == _currentEra.chairman,
                "Caller not previous era chairman. Access denied"
            );
        }
        //check if era with the same eraId already exist!
        Eras.Era storage eraWithGivenId = _eras[eraId_];
        require(
            //eraId is unique. If era with that Id is found, then the proposal is not valid
            eraWithGivenId.chairman == address(0),
            "Era with the same ID already proposed"
        );

        _eras[eraId_] = Eras.Era({
            id: eraId_,
            colectedFeesAmount: 0,
            startDate: utcTimeStamp_,
            endDate: 0,
            chairman: chairman_,
            requiredQuorum: 0, //51% of all validators
            isQuorumReached: false,
            possitiveVotes: 0,
            negativeVotes: 0,
            accepted: false
        });

        emit NewEraElecationProposal(
            eraId_,
            chairman_,
            coucil_,
            _validatorsCount
        );
    }

    function voteForProposedEra(bytes32 eraId, bool approves)
        external
        onlyValidatorAction
    {
        Eras.Era storage era = _eras[eraId];

        require(
            era.chairman != address(0),
            "Era with passed Id does not exist"
        );

        require(
            era.isQuorumReached == false,
            "Vote already ended for that era"
        );

        address caller = msg.sender;

        ErasVotes.ErasVote storage eraVote = _eraValidatorVotes[eraId][caller];

        require(eraVote.voter == address(0), "Validator already voted");

        //register vote
        _eraValidatorVotes[eraId][caller] = ErasVotes.ErasVote({
            voter: caller,
            approve: approves
        });

        //check required quorum is reached
        if (approves) {
            era.possitiveVotes++;
            if (era.possitiveVotes >= era.requiredQuorum) {
                //era election is approved!
                era.isQuorumReached = true;
                era.accepted = true;
            }
        } else {
            era.negativeVotes++;
            if (era.negativeVotes >= era.requiredQuorum) {
                //era election is denied!
                era.isQuorumReached = true;
                era.accepted = false;
            }
        }

        if (era.isQuorumReached) {
            emit NewEraElectionComplited(era);
        }
    }

    function startNewEra() external returns (bytes32) {}

    function _getCurrentEraInternal()
        internal
        view
        returns (Eras.Era memory era)
    {
        return _currentEra;
    }

    function _payReward(uint256 tokenAmountForDistribution_, address chainman_)
        internal
        virtual
    {
        //simple scheme withoud nominators
        //this reward scheme can be ovveriden for much complex one
        //for simple reward distribution
        //chainrman gets 50% of all fees
        uint256 chairmainsReward = tokenAmountForDistribution_ / 2;
        uint256 stakeholderIndex = _stakeholdersIndexes[chainman_];
        Stakeholders.Stakeholder storage chairman = _stakeholders[
            stakeholderIndex
        ];

        require(chairman.user != address(0), "Chairman address does not exist");

        chairman.notCollectedTokenRewards += chairmainsReward;

        uint256 remainingAmountForDistribution = tokenAmountForDistribution_ -
            chairmainsReward;

        //TODO: calculate remaining rewards for the rest of the voting council for this era
    }
}
