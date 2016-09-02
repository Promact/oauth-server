########################
# THE BUILD!
########################
Push-Location $PSScriptRoot
Import-Module $PSScriptRoot\Build\Autofac.Build.psd1 -Force

$artifactsPath = "$PSScriptRoot\artifacts"
$packagesPath = "$artifactsPath\packages"

Install-DotNetCli

# Set build number
$env:DOTNET_BUILD_VERSION = @{ $true = $env:APPVEYOR_BUILD_NUMBER; $false = 1}[$env:APPVEYOR_BUILD_NUMBER -ne $NULL];
Write-Host "Build number:" $env:DOTNET_BUILD_VERSION

# Clean
if(Test-Path $artifactsPath) { Remove-Item $artifactsPath -Force -Recurse }

# Package restore
Get-DotNetProjectDirectory -RootPath $PSScriptRoot | Restore-DependencyPackages

# Build/package
Get-DotNetProjectDirectory -RootPath $PSScriptRoot | Invoke-DotNetBuild

# Test
Invoke-Test $PSScriptRoot\Promact.Oauth.Server\src\Promact.Oauth.Tests

Pop-Location
