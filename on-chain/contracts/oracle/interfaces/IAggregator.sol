pragma solidity ^0.8.0;

//this is the interface that's gonna be implemented in the 3d party smartcontracts that are gonna get data from oracle
interface IAggregator {
    function getPrice()
        external
        view
        returns (
            uint256 price,
            string memory assetCode,
            string memory currencyCode
        );
}
