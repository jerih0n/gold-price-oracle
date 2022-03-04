pragma solidity ^0.8.0;

import "../../utils/Rounds.sol";

interface IBaseResolver {
    function getLatestValidData()
        external
        view
        returns (Rounds.Round memory round);

    function votePriceForRound(bytes32 roundId_, uint256 price_) external;

    function startNewPriceRound(address nodeAddress_, uint256 price_) external;

    function getRoundCount() external view returns (uint256);
}
