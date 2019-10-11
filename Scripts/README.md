# Polkadot-Scripts
Scripts to automate Polkadot (and Kusama) node setup on Ubuntu 18.04. This does not include any security measures related to Ubuntu - always be sure to harden your setup.


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

To keep your Ubuntu installation updated with the latest packages run this command (it will also remove unused packages):

`sudo apt update -y && sudo apt full-upgrade -y && sudo apt autoremove -y && sudo apt clean -y && sudo apt autoclean -y`

This software is release as open source under the Apache license version 2.0. Use at own risk.