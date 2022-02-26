# gold-price-oracle

The goal of this application is to build a decentralized NPoS (Nominated Proof Of Stake) oracle for getting and feeding into the EVM competible blockchains the price  gold 
in USDT.
The system supports its own ERC20 token, and network of smartcontract written in Solidity for getting, voting (acception or refusing price proposition), 
electing validators and giving the abbility of token owners, that don't have required token amount for becoming validators to nominate existing validators. 
Also rewarding mechanism should be implemented - elected validators will get rewards as transaction fees in ERC20 token and will mint new tokens after each era.

# languages and tech

# off-chain part
Of chain part of the oracle is the node logic itself. In most of the part it look like a normal non-decentralized application. The node will be implemented with
  1.C#, .net 5.0
  2.MS SQL database with EF core Code First
  3. Using nugget packages like NBitcoin and Nethereum
  4. Ui will be implemented with ReactJs
  
# on-chain part   
The on-chain part of the oracle is logic that implements that NPoS, election, round price date consolidation, rewards, ERC20 token etc. In short this is the place for all
blockchain interactions. On-Chain pars is implemented with network of smartcontract written in Solidity v 0.8.0. Usage of some oppenzeppelin smartcontract are also presented
  1.Solidity
  2.Truffle
