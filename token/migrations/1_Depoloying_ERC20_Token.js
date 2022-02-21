const GoldOracleToken = artifacts.require("GoldOracleToken");
const Stakeholders = artifacts.require("Stakeholders");

module.exports = function (deployer) {
  deployer.deploy(Stakeholders);
  deployer.deploy(GoldOracleToken);
};
