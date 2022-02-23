pragma solidity ^0.8.0;

import "../utils/Rounds.sol";
import "../utils/Votes.sol";

//PoC. Big Refactoring will happened!
contract GoldPriceResolver {
    uint256 constant MINIMUM_QUORUM_REQUURED_FOR_VALIDATION_IN_PERCENTS = 51;
    uint256 constant MAXIMUM_ALLOWED_PRICE_DEVIATION_IN_PERCENTS = 3;
    string constant GOLD_SYMBOL = "XUA"; //TODO; move to separate contract when refactor
    string constant USDT_SYMBOL = "USDT";
    Rounds.Round[] private _rounds;
    mapping(bytes32 => uint256) private _roundIndexs;
    mapping(bytes32 => Votes.Vote) _votes;

    constructor() {}

    //TODO: this all will come form PoS. For now they are hardcoded
    uint256 validatorNodesCount = 1;
    address electedNodeAddress =
        address(0x5B38Da6a701c568545dCfcB03FcB875f56beddC4); //TODO: this will come as a result of PoS. Hardcoded for now

    event NewPriceVote(
        string assetSymbol,
        string currencySymbol,
        bytes32 roundId,
        address proposalEmiterAddress,
        uint256 price
    );

    function startNewPriceRound(address nodeAddress_, uint256 price_) public {
        //call from elected node. If not revert.
        //Only elected node should be able to start new round of price change
        //after the price is set need to be validated - at least 51% of active nodes must approve the value
        //after the quorum is reached the value is finaly set and become valid price
        //only valid (validated) prices should be return to aggregator
        require(nodeAddress_ != address(0), "Null address is invalid");
        //if check and then slash - we can perform a penalty for trying to update without permission;
        //for now it will be required check
        require(
            nodeAddress_ == electedNodeAddress,
            "Given address not elected for validation"
        );

        //default nonce is 0;
        uint256 nonce = 0;

        //get last valid round;

        Rounds.Round memory lastRound = getLatestRoundData();

        //if there is already las
        if (lastRound.nonce != 0) {
            nonce = lastRound.nonce;
        } // this is the initalRound;

        nonce++;

        Rounds.Round memory round = Rounds.Round({
            roundId: keccak256(abi.encodePacked(nodeAddress_, nonce, price_)),
            nonce: nonce,
            node: nodeAddress_,
            price: price_,
            assetCode: GOLD_SYMBOL,
            currencyCode: USDT_SYMBOL,
            requiredQuorum: calculateMinimumNodesCountForReachingQuorum(),
            isQuorumReached: false,
            acceptVotes: 0,
            refuceVotes: 0
        });

        //the elected node kick off the new round
        _rounds.push(round);

        //add mapping for roundId and nonce for easy search on callback
        _roundIndexs[round.roundId] = nonce - 1;

        emit NewPriceVote(
            GOLD_SYMBOL,
            USDT_SYMBOL,
            round.roundId,
            round.node,
            round.price
        );
    }

    function votePriceForRound(bytes32 roundId_, uint256 price_) public {
        address callerAddress = msg.sender;
        //check if caller is an elected member for current era
        //TODO:
        //required check
        //find correct round record by roundId
        Rounds.Round storage round = tryGetRoundForValidationByRequestedRoundId(
            roundId_
        );
        //unique identifier of the vode - cobination of round id and voter address
        bytes32 voteIdentity = keccak256(
            abi.encodePacked(roundId_, callerAddress)
        );

        require(!round.isQuorumReached, "Quorum reached for that round");
        //if voter address != 0 then this voter already voted
        require(
            _votes[voteIdentity].voter == address(0),
            "Caller already voted!"
        );

        //validate the presented data
        uint256 percentageDiffBetweenPrices = calculatePriceDeviation(
            round.price,
            price_
        );

        bool isApproved = percentageDiffBetweenPrices >
            MAXIMUM_ALLOWED_PRICE_DEVIATION_IN_PERCENTS;
        //if the difference between proposed price and the validator price is more than 3%
        //the validation is not passed
        if (isApproved) {
            round.acceptVotes++;
        } else {
            //validation is passed
            round.refuceVotes++;
        }

        //add new unique vote record
        _votes[voteIdentity] = Votes.Vote({
            voter: callerAddress,
            price: price_,
            approves: isApproved,
            percentageDiff: percentageDiffBetweenPrices
        });

        if (round.acceptVotes >= round.requiredQuorum) {
            round.isQuorumReached = true;
            //finaly consensus is reached!
            //this round becames the latest valid round
        }
        if (round.refuceVotes >= round.requiredQuorum) {
            //TODO: the proposition is refuced byt 51%.
            //new round should be created with new proposal node!
            //the proposal node that proposed refuced by 51% value should be punished
            //
        }
    }

    function calculateMinimumNodesCountForReachingQuorum()
        internal
        view
        returns (uint256)
    {
        if (validatorNodesCount == 1) {
            return 1;
        }

        uint256 nodesRequiredForQuorum = (validatorNodesCount *
            MINIMUM_QUORUM_REQUURED_FOR_VALIDATION_IN_PERCENTS) / 100;

        return nodesRequiredForQuorum;
    }

    //valid round is considered every round
    function getLatestRoundData()
        public
        view
        returns (Rounds.Round memory round)
    {
        uint256 roundsLenght = _rounds.length;
        if (roundsLenght != 0) {
            return _rounds[roundsLenght - 1];
        }
    }

    //valid round is considered every round with reached Quorum
    function getLatestValidData()
        public
        view
        returns (Rounds.Round memory round)
    {
        uint256 roundsCount = _rounds.length;
        if (roundsCount != 0) {
            uint256 lastRoundIndex = roundsCount - 1;
            Rounds.Round memory lastRound = _rounds[lastRoundIndex];
            if (round.isQuorumReached) {
                return lastRound;
            }

            for (uint256 i = lastRoundIndex - 1; i < 0; i--) {
                if (_rounds[i].isQuorumReached) {
                    return _rounds[i];
                }
            }
        }
    }

    //performing calculation to find the absolute difference between sugested node and the other validators nodes prices
    //if deviation bigger than 3% is found the proposition is declined by caller validator
    //if deviation is less than 3% the proposition is accepted
    function calculatePriceDeviation(
        uint256 suggestedPrice_,
        uint256 referentPrice_
    ) public pure returns (uint256) {
        uint256 priceDifferenceAbs = 0;

        if (suggestedPrice_ > referentPrice_) {
            priceDifferenceAbs = suggestedPrice_ - referentPrice_;
        } else {
            priceDifferenceAbs = referentPrice_ - suggestedPrice_;
        }
        if (priceDifferenceAbs == 0) {
            return 0;
        }

        uint256 percentageDiff = (priceDifferenceAbs * 100) / suggestedPrice_;

        return percentageDiff;
    }

    function tryGetRoundForValidationByRequestedRoundId(bytes32 roundId_)
        internal
        view
        returns (Rounds.Round storage roundToValidate)
    {
        //get round index by roundId
        uint256 roundIndex = _roundIndexs[roundId_];
        //if no roundId exist revert
        require(roundIndex > 0, "Invalid round id ");
        //get round to validate by round Index
        Rounds.Round storage roundByIndex = _rounds[roundIndex];
        //get last round that should be the current that needs to be validated
        Rounds.Round memory lastRound = getLatestRoundData();
        //check if requested round is actially the last one that needs to be validated
        require(
            roundByIndex.roundId == lastRound.roundId,
            "Voting for old round. The roundId belongs to previous round"
        );
        require(
            lastRound.nonce > 0,
            "Cannot validate round. No rounds are presented"
        );
        //return pointer to the last round
        return roundByIndex;
    }
}
