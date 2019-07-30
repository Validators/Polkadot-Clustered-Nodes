# Polkadot-Scripts
Scripts to automate polkadot node setup on Ubuntu 18.04.

It installs the primary requirement (Rust) and registers the polkadot node as a deamon (service) that automatically starts, also after a server reboot.



1. Copy the install-polkadot.sh file into your Ubuntu distro home directory.
2. Run this command to allow execution: `chmod +x install-polkadot.sh`
3. Then run the script: `./install-polkadot.sh`



To keep your Ubuntu updated with latest packages run:


`apt-get update`

`apt-get upgrade`

