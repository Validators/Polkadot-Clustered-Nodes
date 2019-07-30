#!/bin/bash

set -euo pipefail # Exit script if any errors occur during individual commands

echo "Changing to home directory..."
cd ~/

echo "Installing Rust..."
curl https://sh.rustup.rs -sSf | sh -s -- -y

echo "Initialize Rust - newer version currently breaks hex-literal."
rustup update

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
sudo rm /etc/systemd/system/polkadot-node.service
sudo bash -c "cat >> /etc/systemd/system/polkadot-node.service" <<EOL
# The Polkadot Node service

[Unit]
Description      = Polkadot Node Service

[Service]
User             = $(whoami)
WorkingDirectory = /home/$(whoami)/
ExecStart        = /home/$(whoami)/polkadot/target/release/polkadot --port 30333 --rpc-external --rpc-port 9933 --ws-external --ws-port 9944
Restart          = on-failure

[Install]
WantedBy         = multi-user.target
EOL

sudo systemctl daemon-reload

echo "Enabling Polkadot service so it auto-starts after server boot..."
sudo systemctl enable polkadot-node

echo "Starting Polkadot service..."
sudo systemctl start polkadot-node

echo "Polkadot installation complete!"
echo "Remember to open port 30333 in the firewall, e.g. with ufw: 'sudo ufw allow 30333'"