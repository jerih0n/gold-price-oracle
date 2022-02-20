pragma solidity ^0.8.0;

library Stakeholders {
    struct Stakeholder {
        address user;
        uint256 totalAmount;
        uint256 ownedAmount;
        uint256 nominatedAmount;
        uint256 nominatorsCount;
        bool canValidate;
    }
}
