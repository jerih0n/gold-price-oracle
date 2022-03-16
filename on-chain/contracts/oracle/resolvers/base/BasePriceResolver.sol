pragma solidity ^0.8.0;

import "../../utils/Rounds.sol";
import "../../utils/Votes.sol";
import "../interfaces/IBaseResolver.sol";

abstract contract BasePriceResolver is IBaseResolver {
    uint256
        private constant MINIMUM_QUORUM_REQUURED_FOR_VALIDATION_IN_PERCENTS =
        51;
    uint256 private immutable _maxPriceDerivationInPercents;
    string private _assetCode;
    string private _currencyCode;

    Rounds.Round[] private _rounds;
    mapping(bytes32 => uint256) private _roundIndexs;
    mapping(bytes32 => Votes.Vote) _votes;

    constructor(
        string memory assetCode_,
        string memory currencyCode_,
        uint256 maxPriceDeviationInPercents_
    ) {
        _maxPriceDerivationInPercents = maxPriceDeviationInPercents_;
        _assetCode = assetCode_;
        _currencyCode = currencyCode_;

        //iniatial round to take 0 index;
        _initEmptyRound();
    }

    //TODO: this all will come form PoS. For now they are hardcoded
    uint256 validatorNodesCount = 4;
    address electedNodeAddress =
        address(0x5B38Da6a701c568545dCfcB03FcB875f56beddC4); //TODO: this will come as a result of PoS. Hardcoded for now

    event NewPriceVote(
        string assetSymbol,
        string currencySymbol,
        bytes32 roundId,
        address proposalEmiterAddress,
        uint256 price
    );

    function getRoundCount() external view override returns (uint256) {
        return _rounds.length;
    }

    //valid round is considered every round with reached Quorum
    function getLatestValidData()
        external
        view
        override
        returns (Rounds.Round memory round)
    {
        uint256 roundsLenght = _rounds.length;
        if (roundsLenght > 1) {
            uint256 lastRoundIndex = roundsLenght - 1;
            Rounds.Round memory lastRound = _rounds[lastRoundIndex];
            if (lastRound.isQuorumReached) {
                return lastRound;
            }

            for (uint256 i = lastRoundIndex - 1; i < 0; i--) {
                if (_rounds[i].isQuorumReached) {
                    return _rounds[i];
                }
            }
        }
    }

    function startNewPriceRound(address nodeAddress_, uint256 price_)
        external
        override
    {
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

        Rounds.Round memory lastRound = _getLatestRoundData();

        //if there is already las
        if (lastRound.nonce != 0) {
            nonce = lastRound.nonce;
        } // this is the initalRound;

        nonce++;

        uint256 requiredMinimumQuorum = _calculateMinimumNodesCountForReachingQuorum();

        Rounds.Round memory round = Rounds.Round({
            roundId: keccak256(abi.encodePacked(nodeAddress_, nonce, price_)),
            nonce: nonce,
            node: nodeAddress_,
            price: price_,
            assetCode: _assetCode,
            currencyCode: _currencyCode,
            requiredQuorum: requiredMinimumQuorum,
            isQuorumReached: 1 >= requiredMinimumQuorum, //only in case that there is only one node!
            acceptVotes: 1,
            refuseVotes: 0
        });

        //the elected node kick off the new round
        _rounds.push(round);

        //add mapping for roundId and nonce for easy search on callback
        _roundIndexs[round.roundId] = _rounds.length - 1;

        emit NewPriceVote(
            _assetCode,
            _currencyCode,
            round.roundId,
            round.node,
            round.price
        );

        bytes32 voterIdentity = _createVoterId(round.roundId, nodeAddress_);

        _votes[voterIdentity] = Votes.Vote({
            voter: nodeAddress_,
            price: price_,
            approves: true,
            percentageDiff: 0
        });
    }

    function votePriceForRound(bytes32 roundId_, uint256 price_)
        external
        override
    {
        address callerAddress = msg.sender;
        //check if caller is an elected member for current era
        //TODO:
        //required check
        //find correct round record by roundId
        uint256 validRoundIndex = _tryGetValidRoundIndexByRequestedRoundId(
            roundId_
        );
        Rounds.Round storage round = _rounds[validRoundIndex];
        //unique identifier of the vode - composition key of round id and voter address
        bytes32 voteIdentity = _createVoterId(roundId_, callerAddress);

        require(!round.isQuorumReached, "Quorum reached for that round");
        //if voter address != 0 then this voter already voted
        require(
            _votes[voteIdentity].voter == address(0),
            "Caller already voted!"
        );

        //validate the presented data
        uint256 percentageDiffBetweenPrices = _calculatePriceDeviation(
            round.price,
            price_
        );

        bool isApproved = percentageDiffBetweenPrices <
            _maxPriceDerivationInPercents;
        //if the difference between proposed price and the validator price is more than 3%
        //the validation is not passed
        if (isApproved) {
            round.acceptVotes = round.acceptVotes + 1;
        } else {
            //validation is passed
            round.refuseVotes = round.refuseVotes + 1;
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
        if (round.refuseVotes >= round.requiredQuorum) {
            //TODO: the proposition is refuced byt 51%.
            //new round should be created with new proposal node!
            //the proposal node that proposed refuced by 51% value should be punished
            //
        }
    }

    function _calculateMinimumNodesCountForReachingQuorum()
        private
        view
        returns (uint256)
    {
        //TODO get from PoSS
        if (validatorNodesCount == 1) {
            return 1;
        }

        uint256 nodesRequiredForQuorum = (validatorNodesCount *
            MINIMUM_QUORUM_REQUURED_FOR_VALIDATION_IN_PERCENTS) / 100;

        return nodesRequiredForQuorum;
    }

    //valid round is considered every round

    //performing calculation to find the absolute difference between sugested node and the other validators nodes prices
    //if deviation bigger than 3% is found the proposition is declined by caller validator
    //if deviation is less than 3% the proposition is accepted
    function _calculatePriceDeviation(
        uint256 suggestedPrice_,
        uint256 referentPrice_
    ) internal pure returns (uint256) {
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

    function _tryGetValidRoundIndexByRequestedRoundId(bytes32 roundId_)
        internal
        view
        returns (uint256 index)
    {
        //get round index by roundId
        uint256 roundIndex = _roundIndexs[roundId_];
        //if no roundId exist revert
        require(roundIndex != 0, "Invalid round id ");
        //get round to validate by round Index
        Rounds.Round memory roundByIndex = _rounds[roundIndex];
        //get last round that should be the current that needs to be validated
        Rounds.Round memory lastRound = _getLatestRoundData();
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
        return roundIndex;
    }

    function _getLatestRoundData()
        internal
        view
        returns (Rounds.Round memory round)
    {
        uint256 roundsLenght = _rounds.length;
        if (roundsLenght > 1) {
            return _rounds[roundsLenght - 1];
        }
    }

    function _createVoterId(bytes32 roundId_, address callerAddress)
        private
        pure
        returns (bytes32)
    {
        bytes32 voteIdentity = keccak256(
            abi.encodePacked(roundId_, callerAddress)
        );
        return voteIdentity;
    }

    function _initEmptyRound() private {
        bytes32 defaultNullAddressRoundId = keccak256(
            abi.encodePacked(address(0), "0", "0")
        );
        _rounds.push(
            Rounds.Round({
                roundId: defaultNullAddressRoundId,
                nonce: 0,
                node: address(0),
                price: 0,
                assetCode: _assetCode,
                currencyCode: _currencyCode,
                requiredQuorum: 0,
                isQuorumReached: false,
                acceptVotes: 0,
                refuseVotes: 0
            })
        );
    }
}
