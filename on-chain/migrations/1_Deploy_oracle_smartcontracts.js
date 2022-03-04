const GoldPriceResolver = artifacts.require("GoldPriceResolver");
const Rounds = artifacts.require("Rounds");
const Voters = artifacts.require("Votes");
const GoldOracleToken = artifacts.require("GoldOracleToken");
const Stakeholders = artifacts.require("Stakeholders");
const GoldPriceAggregator = artifacts.require("GoldPriceAggregator");
const TimerContract = artifacts.require("Timer");

module.exports = async function (deployer) {
    deployer.then(async () => {
        await deployer.deploy(GoldOracleToken);
        await deployer.deploy(Stakeholders);

        await deployer.deploy(Rounds);
        await deployer.deploy(Voters);
        await deployer.link(Rounds, GoldPriceResolver);
        await deployer.link(Voters, GoldPriceResolver);
        await deployer.deploy(GoldPriceResolver);

        await deployer.deploy(GoldPriceAggregator, GoldPriceResolver.address);
        //do not depoloy for now
        //await deployer.deploy(TimerContract, GoldPriceResolver.address, GoldOracleToken.address, 213123123);
    });
};