# Polkadot-Scripts
Scripts to automate polkadot node setup on Ubuntu 18.04.

## Temporary first step:
First install Rust. (Will eventually be included in script when reboot is not required.)

`curl https://sh.rustup.rs -sSf | sh -s -- -y`

then reboot: `sudo reboot`

Then you can use the install script.

## install-polkadot.sh

This script retrieves the latest Polkadot build (currently v0.4), and registers the polkadot node as a deamon (service) that automatically starts, also after a server reboot.

1. Copy the [install-polkadot.sh](install-polkadot.sh) file into your Ubuntu distro. A folder called "polkadot" will automatically be created during the installation.
2. Run this command to allow execution: `chmod +x install-polkadot.sh`
3. Then run the script: `./install-polkadot.sh`

After installation you will need to open port 30333 for P2P synchronization, 9933 for HTTP JSON-RPC endpoints, and 9944 for web socket access in the firewall, e.g. with ufw

`sudo ufw allow 30333`

`sudo ufw allow 9933`

`sudo ufw allow 9944`

## update-polkadot.sh

Updates the client to the latest version of Polkadot. Be sure to take a backup of the 'polkadot' folder first if this is critical to your operation.

1. Copy the [update-polkadot.sh](update-polkadot.sh) file into your Ubuntu distro.
2. Run this command to allow execution: `chmod +x update-polkadot.sh`
3. Then run the script: `./update-polkadot.sh`



## ubuntu update

To keep your Ubuntu installation updated with the latest packages run these two commands:

`apt-get update`

`apt-get upgrade`

