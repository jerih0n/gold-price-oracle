pragma solidity ^0.8.0;

import "../utils/Rounds.sol";

//PoC. Big Refactoring will happened!
contract GoldPriceResolver {
    uint256 constant MINIMUM_QUORUM_REQUURED_FOR_VALIDATION_IN_PERCENTS = 51;
    string constant GOLD_SYMBOL = "XUA"; //TODO; move to separate contract when refactor
    string constant USDT_SYMBOL = "USDT";

    Rounds.Round[] private _rounds;

    //TODO: this all will come form PoS. For now they are hardcoded
    uint256 validatorNodesCount = 1;
    address electedNodeAddress =
        address(0x5B38Da6a701c568545dCfcB03FcB875f56beddC4); //TODO: this will come as a result of PoS. Hardcoded for now

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
            isQuorumReached: false
        });

        //the elected node kick off the new round. and
        _rounds.push(round);
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
        if (roundsLenght != 0) return _rounds[roundsLenght - 1];
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
}
