#!/bin/bash

set -euo pipefail # Exit script if any errors occur during individual commands

echo "Changing to home directory..."
cd /home

echo "Installing Rust..."
curl https://sh.rustup.rs -sSf | sh -s -- -y

echo "Adding rust path manually (to avoid having to re-login into terminal."
source $HOME/.cargo/env

echo "Fix for v0.4 issue with hex-literal. This is temporary. Can be deleted when this version is released."
rustup toolchain install nightly-2019-07-14
rustup default nightly-2019-07-14
rustup target add wasm32-unknown-unknown --toolchain nightly-2019-07-14

echo "Installing Polkadot build dependencies..."
sudo apt install -y make clang pkg-config libssl-dev

echo "Cloning Polkadot source from Github..."
git clone --branch v0.4 https://github.com/paritytech/polkadot.git

echo "Building Polkadot..."
cd polkadot
./scripts/init.sh
./scripts/build.sh
cargo build --release

echo "Registering Polkadot as Systemd service..."
sudo rm /etc/systemd/system/polkadot-node.service  || true
sudo bash -c "cat >> /etc/systemd/system/polkadot-node.service" <<EOL
# The Polkadot Node service

[Unit]
Description      = Polkadot Node Service

[Service]
User             = $(whoami)
ExecStart        = /home/polkadot/target/release/polkadot --port 30333 --rpc-external --rpc-port 9933 --ws-external --ws-port 9944
Restart          = on-failure
StartLimitBurst  = 4
# Restart, but not more than once every 2 minutes
StartLimitInterval = 120
# Restart, but not more than once every 30s (for testing purposes)
StartLimitInterval = 30

[Install]
WantedBy         = multi-user.target
EOL

sudo systemctl daemon-reload

echo "Enabling Polkadot service so it auto-starts after server boot..."
sudo systemctl enable polkadot-node

echo "Starting Polkadot service..."
sudo systemctl start polkadot-node

echo "Polkadot installation complete!"

sudo systemctl status polkadot-node

echo "Remember to open port 30333 (P2P), 9933 (HTTP), and 9944 (WS) in the firewall"