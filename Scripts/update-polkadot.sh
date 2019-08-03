#!/bin/bash
set -euo pipefail

echo "Changing to root directory..."
cd ~/

echo "Stopping polkadot-node service..."
sudo systemctl stop polkadot-node

echo "Pulling newest polkadot source..."
cd ~/polkadot
git pull

echo "Building source..."
./scripts/init.sh
./scripts/build.sh
cargo build --release

echo "Starting polkadot-node service..."
sudo systemctl start polkadot-node

echo "Polkadot update complete!"

sudo systemctl status polkadot-node