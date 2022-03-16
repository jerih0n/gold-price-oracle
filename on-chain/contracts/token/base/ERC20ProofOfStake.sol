pragma solidity ^0.8.0;

import "./ERC20Stakable.sol";
import "../utils/NominatingStakeholders.sol";
import "../utils/Eras.sol";
import "../Interfaces/IProofOfStake.sol";
import "../Interfaces/IErasMonitor.sol";
import "../utils/ErasVotes.sol";

contract ERC20ProofOfStake is IErasMonitor, IProofOfStake, ERC20Stakable {
    uint256 private constant MAX_COUICIL_MEMBERS_COUNT = 10;
    uint256
        private constant MINIMUM_QUORUM_REQUURED_FOR_VALIDATION_IN_PERCENTS =
        51;
    uint256 immutable _rewardForValidatorsAfterEachEra;
    mapping(bytes32 => Eras.Era) internal _eras;
    mapping(bytes32 => mapping(address => ErasVotes.ErasVote))
        internal _eraValidatorVotes;
    mapping(bytes32 => address[]) internal _erasCouncil;
    bytes32 internal _activeEraId;
    bytes32 internal _previousEraId;
    bytes32 internal _lastApprovedEra;

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
        uint256 validatorsCount,
        uint256 calculatedSeed
    );

    event NewEraElectionComplited(Eras.Era era);

    event EndEraByNewElectedChairman(
        bytes32 eraId,
        address chairman,
        uint256 timestap
    );

    modifier onlyValidatorAction() {
        uint256 validatorNodeIndex = _validatorsStakeholderIndexes[msg.sender];
        require(validatorNodeIndex != 0, "Voting forbidden for non-validators");
        _;
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
        return _eras[_activeEraId];
    }

    function getErasCount() external view override returns (uint256) {
        return _erasCount;
    }

    function endEra(uint256 timestamp) external override {
        address caller = msg.sender;
        Eras.Era memory futureEra = _eras[_lastApprovedEra];

        //TODO: validation for time elapsed
        require(
            caller == futureEra.chairman,
            "Only chairman can end current era"
        );
        if (_previousEraId == 0 && _activeEraId == 0) {
            //corner case . The only time when this will be executed
            //is on the very firs era
            //no fees for distribution
            //and auto set previous era as current era
            _previousEraId = _lastApprovedEra;
        } else {
            _payReward(
                futureEra.colectedFeesAmount + _rewardForValidatorsAfterEachEra,
                caller
            );
            _previousEraId = _activeEraId;
        }
        _activeEraId = _lastApprovedEra;

        emit EndEraByNewElectedChairman(_activeEraId, caller, timestamp);
        //more complex stuff - add validation for timestamp by nodes, with another vote and
        //if proposed timestamp is wrong, and consensus is reached - punish the node
        //that execute this code. For now this will not be implemented
    }

    function proposeNewEra(
        bytes32 eraId_,
        address chairman_,
        address[] memory coucil_,
        uint256 utcTimeStamp_,
        uint256 calculatedSeed_
    ) external override {
        address caller = msg.sender;
        Eras.Era memory currentEra = _getCurrentEraInternal();
        if (currentEra.chairman != address(0)) {
            //this is not the first era. Only previous chairman can propose new election
            require(
                caller == currentEra.chairman,
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
        //TODO: after testing new proposed era should come with one possitive vote
        _eras[eraId_] = Eras.Era({
            id: eraId_,
            colectedFeesAmount: 0,
            startDate: utcTimeStamp_,
            endDate: 0,
            chairman: chairman_,
            requiredQuorum: _calculateMinimumNodesCountForReachingQuorum(), //51% of all validators
            isQuorumReached: false,
            possitiveVotes: 0,
            negativeVotes: 0,
            accepted: false,
            ended: false
        });

        emit NewEraElecationProposal(
            eraId_,
            chairman_,
            coucil_,
            _validatorsCount,
            calculatedSeed_
        );
    }

    function voteForProposedEra(bytes32 eraId, bool approves)
        external
        override
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
        if (approves == true) {
            era.possitiveVotes += 1;
            if (era.possitiveVotes == era.requiredQuorum) {
                //era election is approved!
                era.isQuorumReached = true;
                era.accepted = true;
                _lastApprovedEra = eraId;
            }
        } else {
            era.negativeVotes += 1;
            if (era.negativeVotes == era.requiredQuorum) {
                //era election is denied!
                era.isQuorumReached = true;
                era.accepted = false;
                //TODO: initialized a revote and new proposal!
                //TODO: punish the era proposal node (slash)
            }
        }

        if (era.isQuorumReached == true) {
            emit NewEraElectionComplited(era);
        }
    }

    function _getCurrentEraInternal()
        internal
        view
        returns (Eras.Era memory era)
    {
        return _eras[_activeEraId];
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

    function _calculateMinimumNodesCountForReachingQuorum()
        private
        view
        returns (uint256)
    {
        //TODO get from PoSS
        if (_validatorsCount == 1) {
            return 1;
        }

        uint256 nodesRequiredForQuorum = (_validatorsCount *
            MINIMUM_QUORUM_REQUURED_FOR_VALIDATION_IN_PERCENTS) / 100;

        return nodesRequiredForQuorum;
    }
}
