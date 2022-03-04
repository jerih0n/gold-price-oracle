pragma solidity ^0.8.0;

library Eras {
    struct Era {
        bytes32 id;
        uint256 colectedFeesAmount;
        uint256 startDate;
        uint256 endDate;
        address chairman;
    }
}
