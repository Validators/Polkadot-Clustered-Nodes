# Infrastructure high volume stress test

There are several ways to test your infrastructure. With a public node setup for RPC calls coming via HTTP or a Websocket connection it is reasonable to perform a "Request" based stress test.

Several applications can be used to do HTTP tests while tests for websockets are less supported. We have however found a couple.

First, for HTTP tests we suggest using open source solutions such as [Artillery.io](https://artillery.io/docs/getting-started/), https://k6.io/, https://github.com/codesenberg/bombardier, or https://locust.io/ for Python programmers. 

Secondly, for Websocket tests the available open source tools are more limited. We have found that Artillery.io also supports websockets and for instance https://github.com/observing/thor as a good suggestion.

Specifically for Polkadot (and Kusama), there is a project by Mixbytes called Tank that automates the process of testing Polkadot nodes directly: https://github.com/mixbytes/tank. 

We are however, looking to test our entire infrastructure from outside our load balancers all the way through to the blockchain nodes. Here is a small example using Artillery.io:

## Example of using Artillery.io load test

 Follow the instructions in their ["Getting started"](https://artillery.io/docs/getting-started/) section. Then copy our `polkadot-load-test.yml` file in this repository to your test machine and run it:

 `artillery run polkadot-load-test.yml`

This will produce 1,200 calls to get the metadata of the node.

```
Started phase 0, duration: 60s @ 11:04:34(+0200) 2019-10-01
Report @ 11:04:44(+0200) 2019-10-01
Elapsed time: 10 seconds
  Scenarios launched:  199
  Scenarios completed: 197
  Requests completed:  197
  RPS sent: 20.18
  Request latency:
    min: 79.9
    max: 216.9
    median: 103.8
    p95: 153.7
    p99: 186
  Codes:
    200: 197

Report @ 11:04:54(+0200) 2019-10-01
Elapsed time: 20 seconds
  Scenarios launched:  201
  Scenarios completed: 201
  Requests completed:  201
  RPS sent: 20.08
  Request latency:
    min: 77.9
    max: 216.2
    median: 99.3
    p95: 138.1
    p99: 161.4
  Codes:
    200: 201

etc ...

Report @ 11:05:34(+0200) 2019-10-01
Elapsed time: 1 minute, 0 seconds
  Scenarios launched:  199
  Scenarios completed: 201
  Requests completed:  201
  RPS sent: 19.92
  Request latency:
    min: 75.8
    max: 259.7
    median: 89.7
    p95: 136.8
    p99: 190.8
  Codes:
    200: 201

Report @ 11:05:34(+0200) 2019-10-01
Elapsed time: 1 minute, 0 seconds
  Scenarios launched:  0
  Scenarios completed: 0
  Requests completed:  0
  RPS sent: NaN
  Request latency:
    min: NaN
    max: NaN
    median: NaN
    p95: NaN
    p99: NaN

All virtual users finished
Summary report @ 11:05:34(+0200) 2019-10-01
  Scenarios launched:  1200
  Scenarios completed: 1200
  Requests completed:  1200
  RPS sent: 19.87
  Request latency:
    min: 75.8
    max: 265.5
    median: 94.6
    p95: 142.3
    p99: 181.1
  Scenario counts:
    0: 1200 (100%)
  Codes:
    200: 1200
```

##
Need data from the Kusama or Polkadot blockchain? Goto [nnode.io](https://nnode.io) to register for our [free public nodes](https://nnode.io).

This software is release as open source under the Apache license version 2.0. Use at own risk.