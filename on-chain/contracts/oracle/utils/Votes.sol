pragma solidity ^0.8.0;

library Votes {
    struct Vote {
        address voter;
        uint256 price;
        bool approves;
        uint256 percentageDiff;
    }
}
