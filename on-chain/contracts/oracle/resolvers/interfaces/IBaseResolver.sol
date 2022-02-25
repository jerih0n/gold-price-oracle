pragma solidity ^0.8.0;

import "../../utils/Rounds.sol";

interface IBaseResolver {
    function getLatestValidData()
        external
        view
        returns (Rounds.Round memory round);
}
