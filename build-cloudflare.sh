#!/bin/sh
curl -sSL https://dot.net/v1/dotnet-install.sh > dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh -c 7.0 -InstallDir ./dotnet
./dotnet/dotnet --version
./dotnet/dotnet publish -c Release -o output
./dotnet/dotnet ./output/charts-site-generator.dll ./output/wwwroot
npx tailwindcss -i ./style.css -o ./output/wwwroot/style.css
cp ./script.js ./output/wwwroot
cp ./favicon.ico ./output/wwwroot
