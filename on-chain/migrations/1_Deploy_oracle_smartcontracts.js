const GoldPriceResolver = artifacts.require("GoldPriceResolver");
const Rounds = artifacts.require("Rounds");
const Voters = artifacts.require("Votes");
const GoldOracleToken = artifacts.require("GoldOracleToken");
const Stakeholders = artifacts.require("Stakeholders");

module.exports = async function (deployer) {
    deployer.then(async () => {
        await deployer.deploy(GoldOracleToken);
        await deployer.deploy(Stakeholders);

        await deployer.deploy(Rounds);
        await deployer.deploy(Voters);
        await deployer.deploy(GoldPriceResolver);
    });
};