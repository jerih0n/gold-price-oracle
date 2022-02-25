pragma solidity ^0.8.0;

library Rounds {
    struct Round {
        bytes32 roundId;
        uint256 nonce;
        address node;
        uint256 price;
        string assetCode;
        string currencyCode;
        uint256 requiredQuorum;
        bool isQuorumReached;
        uint256 acceptVotes;
        uint256 refuseVotes;
    }
}
