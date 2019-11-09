# Reverse proxies for Polkadot node cluster

This project is based on the ProxyKit library created by Damian Hickey (https://github.com/damianh/ProxyKit).

Its a .Net Core Kestrel solution that can be deployed on Linux, Mac, and Windows.

## Setup instructions
The following explains how to compile the project, change configurations, and setup the application as a service on Ubuntu.

### Compile project
Open the project in Visual Studio 2017 or later to compile the project. Or use the command line (dotnet CLI) with the following command:

`dotnet publish Nnode.Proxy\\Nnode.Proxy.csproj -c release -o c:\\YourFolder /p:EnvironmentName=Production`

### Configurations

There are one configuration file called *appsettings.Production.json* in the root of the project that contains the following:

```
{
	"Logging": {
		"LogLevel": {
			"Default": "Warning"
		}
	},
	"AllowedHosts": "*",
	"AppSettings": {
		"BlockchainId": "1", //Polkadot (the ID from your Developer Portal database)
	}
}
```
Be sure to change the "BlockchainId" to correspond with your your developer portal blockchain ID (from database). 

### Install on Ubuntu as a Service

After you have compiled the project and made the appropriate changes to the appsettings.Production.json file the project can be deployed to a Ubuntu machine that has dotnet core installed. It is common to setup the application as a systemd service in Ubuntu.

The following script can be used to create the service. It shows the creating of a kusama proxy. All traffic to the port 80 is redirected to port 5000 (http) and all traffic to port 443 is redirected to port 5001 (https) of the running proxy application:

```
#!/bin/bash
set -euo pipefail

APP_DIR=kusama.nnode.io
APP_USER=polkauser 

echo "Giving ownership to polkauser"
sudo -S chown -R polkauser:polkauser /var/www/$APP_DIR/web

echo "Setting permission of web root"
sudo chmod -R 755 ~/var/www/$APP_DIR

echo "Registering Kusama as Systemd service..."
sudo systemctl stop kusama-proxy
sudo systemctl disable kusama-proxy
sudo rm /etc/systemd/system/kusama-proxy.service  || true
sudo bash -c "cat >> /etc/systemd/system/kusama-proxy.service" <<EOL
# The Kusama Proxy service
[Unit]
Description      = Kusama Proxy Service
[Service]
User             = ${APP_USER}
WorkingDirectory = /home/${APP_USER}/var/www/${APP_DIR}/web
ExecStart        = /usr/bin/dotnet /home/${APP_USER}/var/www/${APP_DIR}/web/Node.Proxy.dll --urls "http://0.0.0.0:5000;https://0.0.0.0:5001"

Restart=always
# Restart service after 10 seconds if the dotnet service crashes:
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=ProxyV1.1
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

StartLimitBurst  = 4
# Restart, but not more than once every 2 minutes
StartLimitInterval = 120
# Restart, but not more than once every 30s (for testing purposes)
StartLimitInterval = 30
[Install]
WantedBy         = multi-user.target
EOL

sudo systemctl daemon-reload

echo "Enabling service so it auto-starts after server boot..."
sudo systemctl enable kusama-proxy

echo "Starting service..."
sudo systemctl start kusama-proxy

#echo "Adding redirect ports to iptables"
sudo iptables -t nat -C PREROUTING -p tcp --dport 80 -j REDIRECT --to-ports 5000
sudo iptables -t nat -C PREROUTING -p tcp --dport 443 -j REDIRECT --to-ports 5001

echo "Installation complete!"
sudo systemctl status kusama-proxy

```
**Please notice!** Other requirements such as Ubuntu user roles, firewall rules, SSL setup, and other security measures is not explained here as it is expected to be general knowledge of an Ubuntu devops role. It can be implemented in different ways depending on the requirements and capabilities of the overall setup, or deployment procedure (CI).


##

If you think its too much of a hassle to run your own cluster of nodes then please consider [nnode.io](https://nnode.io) to register for our public nodes (free & paid available).
##
*This software is released as open source under the Apache license version 2.0. Use at own risk.*