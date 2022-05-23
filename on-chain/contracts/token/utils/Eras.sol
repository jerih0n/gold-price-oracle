pragma solidity ^0.8.0;

library Eras {
    struct Era {
        bytes32 id;
        bytes32 previousEraId;
        uint256 colectedFeesAmount;
        uint256 startDate;
        uint256 endDate;
        address chairman;
        uint256 requiredQuorum;
        bool isQuorumReached;
        uint256 possitiveVotes;
        uint256 negativeVotes;
        bool accepted;
        bool ended;
        uint256 totalWeightOfVotingCoucil;
    }
}
