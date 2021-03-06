# Polkadot-Infrastructure

If you want to setup your own polkadot or kusama nodes we hereby give you some inspiration with a description of our own infrastructure in combination with a couple of automated scripts that makes it easier to get started.

The automated scripts are in the `Scripts` folder in this repository.

Instead of running your own [Polkadot node](https://polkadot.nnode.io) or [Kusama node](https://kusama.nnode.io), you are welcome to use our [public blockchain nodes](https://nnode.io) at nnode.io.

## Infrastructure backup and recovery mechanism - take one


Our infrastructure consists of two parts: the first is our Proxy Layer that contains API Authentication, Caching, and a Node load balancer. The second is the actual Node Cluster that is part of the blockchain. With this setup we are able to perform node and OS updates with zero or minimum downtime. It also ensures that any issues with a particular hosting provider are mitigated to a relevant extend.

The **node cluster** consists of multiple blockchain clients (nodes) spread geographically in Europe, and eventually in USA, and Asia. Each node does not require any backup as the recovery mechanish is built in the way a blockchain node syncronizes with its peers. If one node goes down all its traffic will automatically be redirected to an active node by the Node load balancer and a new node will be started with the automatic node install script. Its IP will then be added to the whitelist of nodes in the load balancer. Although each nodes are constantly synchronizing with each other it is important for any decentralized application (Dapp) to be connected to the same node during its active session in order to show consistent blockchain data (or state) to the user of the application. This is the primary role of the "same node" load balancers performed by the proxy layer.

The **proxy layer** consists of custom implementations of API Authentication, Caching, and Node load balancers. To lower complexity everything is run on a single powerful server as idempotent transactions and mirrored in hardware to a secondary server, along with a fallback server at a different hosting provider. In case of OS updates or server failures the second server can takeover until the primary proxy is back online. In order to facilitate instant change of the active proxy server, the primary IP is setup as a floating network IP from the primary cloud hosting provider. Internal communication between the two servers are setup as a private network without public internet connectivity. 

In the case of issues with our primary hosting provider, the fallback server can be redirected to at the DNS level with a 5 minutes (Cloudflare) propagation time. Individual DNS cache on the internet may take longer to update. In such an extreeme situation we can currently not guarantee same node response because the fallback server has its own set of polkadot nodes.

The setup is illustrated at a high level in the image below.


![Polkadot Public node Infrastructure](https://raw.githubusercontent.com/Validators/Polkadot-Infrastructure/master/Polkadot-Public-Nodes-Architechture-1.0.png)

In conclusion this allows for Node and OS updates with zero downtime of the public node service. 

Please goto [nnode.io](https://nnode.io) to register for our free public nodes.

This software is release as open source under the Apache license version 2.0. Use at own risk.