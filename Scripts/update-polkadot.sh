#!/bin/bash
set -euo pipefail

echo "Changing to home directory..."
cd /home

echo "Stopping polkadot-node service..."
sudo systemctl stop polkadot-node

echo "Pulling newest polkadot source..."
cd polkadot
git pull

#In case we are still in terminal first time.
echo "Adding rust path manually (to avoid having to re-login into terminal."
source $HOME/.cargo/env

echo "Building source..."
./scripts/init.sh
./scripts/build.sh
cargo build --release

echo "Starting polkadot-node service..."
sudo systemctl start polkadot-node

echo "Polkadot update complete!"

sudo systemctl status polkadot-node