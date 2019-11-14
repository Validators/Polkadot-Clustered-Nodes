# Clustered-Polkadot-nodes

If you want to setup your own clusted polkadot or kusama nodes we hereby give you a guide that has enabled the clusted node service found on nnode.io.

The setup consists of these main parts:

1. Developer portal for API-Key creation and transaction analytics
2. Running blockchain nodes
3. Reverse proxies with a node load balancer and API authentication

## 1 Developer portal
This step is not mandatory but gives you the capability to register several projects with their invidual API-Keys that can be used for transaction limits, load allocation, and analytics. If you only have one single project you can generate an API-Key manually by using the `RandomKeyGenerator.GetUniqueKey(128)` method found in the `Utilities` folder in the project to create the **Api Secret**, and just create a new Guid for the **Api Key**.

### Option 1: Manually creating one API-Key set:
```
var apiKey = Guid.NewGuid();
var apiSecret = RandomKeyGenerator.GetUniqueKey(128);
```

### Option 2: Setup developer portal for multiple users
Please follow the instructions from the repository to setup the developer portal: https://github.com/Validators/Developer-Portal

## 2 Running blockchain nodes
This is the actual **Node Cluster** that is part of the blockchain. When running several nodes behind a proxy you are able to perform node and OS updates with zero downtime. Please follow the excellent tutorial found at the main repository of Polkadot to familiaze yourself with running a node:

https://github.com/paritytech/polkadot

Then goto our `Scripts` repository that shows you how to run blockchain nodes as a service on Ubuntu along with different scripts to automate upgrades:

https://github.com/Validators/Polkadot-Infrastructure

You now have your API-Keys ready along with a couple of nodes running. Its time to setup the reverse proxy layer.

## 3 Reverse proxies
The **proxy layer** consists of custom implementations of API Authentication, Caching, and Node load balancers. It uses the ProxyKit library created by Damian Hickey (https://github.com/damianh/ProxyKit).

Its a .Net Core Kestrel solution that can be deployed on Linux, Mac, and Windows.

Please goto the main repository for instructions on how to get started: 

##
Please goto [nnode.io](https://nnode.io) to register for our free public nodes.
##
*This software is released as open source under the Apache license version 2.0. Use at own risk.*


