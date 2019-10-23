#!/usr/bin/env pwsh

Set-StrictMode -Version latest
$ErrorActionPreference = "Stop"

$component = Get-Content -Path "component.json" | ConvertFrom-Json
[xml]$xml = Get-Content -Path Source/Interface/Interface.csproj
$version = $xml.Project.PropertyGroup.Version

if ($component.version -ne $version) {
    throw "Versions in component.json and Interface.csproj do not match"
}

[xml]$xml2 = Get-Content -Path Source/Client/Client.csproj
$version = $xml2.Project.PropertyGroup.Version

if ($component.version -ne $version) {
    throw "Versions in component.json and Client.csproj do not match"
}

# # Build Interface package
# dotnet build Source/Interface/Interface.csproj -c Release
# dotnet pack Source/Interface/Interface.csproj -c Release -o ../../dist

# # Build Client package
# dotnet build Source/Client/Client.csproj -c Release
# dotnet pack Source/Client/Client.csproj -c Release -o ../../dist

$packages = (Get-ChildItem -Path "dist/*.$version.nupkg")

foreach ($package in $packages)
{
    $packagePath = $package.FullName
    Write-Output $packagePath

    # Push to nuget repo
    dotnet nuget push $packagePath -k key -s https://pkgs.dev.azure.com/wexxle/_packaging/default/nuget/v3/index.json /p:ConfigFile=nuget.config 
}
