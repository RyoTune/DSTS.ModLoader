# Set Working Directory
Split-Path $MyInvocation.MyCommand.Path | Push-Location
[Environment]::CurrentDirectory = $PWD

Remove-Item "$env:RELOADEDIIMODS/DSTS.ModLoader/*" -Force -Recurse
dotnet publish "./DSTS.ModLoader.csproj" -c Release -o "$env:RELOADEDIIMODS/DSTS.ModLoader" /p:OutputPath="./bin/Release" /p:ReloadedILLink="true"

# Restore Working Directory
Pop-Location