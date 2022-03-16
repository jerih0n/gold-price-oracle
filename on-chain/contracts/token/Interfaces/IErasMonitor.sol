pragma solidity ^0.8.0;

import "../utils/Eras.sol";

interface IErasMonitor {
    function getErasCount() external view returns (uint256);

    function getCurrentEra() external view returns (Eras.Era memory currentEra);

    function getEraById(bytes32 eraId_)
        external
        view
        returns (Eras.Era memory era);
}
