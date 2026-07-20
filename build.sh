#!/usr/bin/env bash
set -euo pipefail
shopt -s globstar

echo -e "\e[1;94m====   BUILD   ====\e[0m"
dotnet build -c Release -o build
echo -e "\e[1;94m====    ZIP    ====\e[0m"
zip "release.zip" -jMM "manifest.json" "icon.png" "README.md" build/**.dll

echo -e "\e[1;94m====   CLEAN   ====\e[0m"
rm -rvf build
mv -v README.md README-CL.md # theres probably an easier way of doing this but i'm lazy
mv -v README-SV.md README.md

echo -e "\e[1;94m==== SV  BUILD ====\e[0m"
dotnet build -c Release -o build '-p:DefineConstants=SV'
echo -e "\e[1;94m====  SV  ZIP  ====\e[0m"
zip "release-sv.zip" -jMM "manifest.json" "icon.png" "README.md" build/**.dll

echo -e "\e[1;94m====  FINALIZE  ====\e[0m"
mv -v README.md README-SV.md
mv -v README-CL.md README.md
echo "Creating release folder..."
mkdir release
mv release.zip release-sv.zip release
echo -e "\e[1;94m====================\nRelease files created at \e[32m\"release\"\e[0m"