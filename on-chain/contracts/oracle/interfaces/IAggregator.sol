pragma solidity ^0.8.0;

interface IAggregator {
    function getPrice()
        external
        returns (
            uint256 price,
            bytes memory assetCode,
            bytes memory currencyCode
        );
}
