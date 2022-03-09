pragma solidity ^0.8.0;

import "./ERC20Stakable.sol";
import "../utils/NominatingStakeholders.sol";
import "../utils/Eras.sol";
import "../Interfaces/IProofOfStake.sol";
import "../Interfaces/IErasMonitor.sol";
import "../utils/ErasVotes.sol";

contract ERC20ProofOfStake is IErasMonitor, IProofOfStake, ERC20Stakable {
    uint256 constant MAX_COUICIL_MEMBERS_COUNT = 10;

    uint256 immutable _rewardForValidatorsAfterEachEra;
    mapping(bytes32 => Eras.Era) internal _eras;
    mapping(bytes32 => mapping(address => ErasVotes.ErasVote))
        internal _eraValidatorVotes;
    mapping(bytes32 => address[]) internal _erasCouncil;
    Eras.Era internal _currentEra;

    constructor(
        string memory name_,
        string memory symbol_,
        uint256 minStakedAmountForValidation_,
        uint256 rewardForValidatorsAfterEachEra_
    ) ERC20Stakable(name_, symbol_, minStakedAmountForValidation_) {
        _rewardForValidatorsAfterEachEra = rewardForValidatorsAfterEachEra_;
    }

    event NewEraElecationProposal(
        bytes32 eraId,
        address chairman,
        address[] council,
        uint256 validatorsCount
    );

    event NewEraElectionComplited(Eras.Era era);

    modifier onlyValidatorAction() {
        uint256 validatorNodeIndex = _validatorsStakeholderIndexes[msg.sender];
        require(validatorNodeIndex != 0, "Voting forbidden for non-validators");
        _;
    }

    //** for INTERFACE !!! */
    function getValidatorsCount() external view override returns (uint256) {
        return _validatorsCount;
    }

    function getStakeholders()
        external
        view
        override
        returns (Stakeholders.Stakeholder[] memory stakeholders)
    {
        return _stakeholders;
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
