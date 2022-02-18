const GoldOracleToken = artifacts.require("GoldOracleToken");

module.exports = function (deployer) {
  deployer.deploy(GoldOracleToken);
};
