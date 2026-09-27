param(
    [string] $Team = 'ValheimSuperMods',
    [string[]] $Plugin = @(),
    [string] $TokenFile = (Join-Path $env:USERPROFILE '.thunderstore_token'),
    [string] $TcliPath = (Join-Path $PSScriptRoot '.tools\tcli.exe'),
    [switch] $Publish
)

$ErrorActionPreference = 'Stop'
if ($Team -notmatch '^[A-Za-z0-9_]+$') { throw 'Team name contains invalid characters.' }
if (-not (Test-Path -LiteralPath $TcliPath -PathType Leaf)) {
    throw "tcli is missing. Install it with: dotnet tool install tcli --tool-path .tools --version 0.2.4"
}
$projectRoot = Join-Path $PSScriptRoot 'ValheimMods'
$dist = Join-Path $PSScriptRoot 'dist'
$projects = @(Get-ChildItem -LiteralPath $projectRoot -Directory | Where-Object {
    Test-Path -LiteralPath (Join-Path $_.FullName 'manifest.json')
})
if ($Plugin.Count -gt 0) {
    $unknown = @($Plugin | Where-Object { $_ -notin $projects.Name })
    if ($unknown.Count -gt 0) { throw "Unknown plugin(s): $($unknown -join ', ')" }
    $projects = @($projects | Where-Object { $_.Name -in $Plugin })
}
if ($projects.Count -eq 0) { throw 'No plugin projects were selected.' }

if ($Publish) {
    if (-not (Test-Path -LiteralPath $TokenFile -PathType Leaf)) { throw "Token file not found: $TokenFile" }
    $env:TCLI_AUTH_TOKEN = [IO.File]::ReadAllText($TokenFile).Trim()
    if (-not $env:TCLI_AUTH_TOKEN) { throw 'Thunderstore token file is empty.' }
}

try {
    foreach ($project in $projects) {
        $manifest = Get-Content -LiteralPath (Join-Path $project.FullName 'manifest.json') -Raw | ConvertFrom-Json
        $zipPath = Join-Path $dist "$($manifest.name)-$($manifest.version_number).zip"
        if (-not (Test-Path -LiteralPath $zipPath -PathType Leaf)) { throw "Package not found: $zipPath" }
        $configPath = Join-Path $dist "$($manifest.name).thunderstore.toml"
        $categories = if ($manifest.name -eq 'DevTools') {
            '"mods", "tools", "client-side", "ai-generated"'
        } else {
            '"mods", "tweaks", "client-side", "ai-generated"'
        }
        $config = @"
[config]
schemaVersion = "0.0.1"

[package]
namespace = "$Team"
name = "$($manifest.name)"
versionNumber = "$($manifest.version_number)"
description = "$($manifest.description)"
websiteUrl = "$($manifest.website_url)"
containsNsfwContent = false

[package.dependencies]
denikson-BepInExPack_Valheim = "5.4.2351"

[build]
icon = "../ValheimMods/Publishing/icon.png"
readme = "../ValheimMods/$($manifest.name)/README.md"
outdir = "./build"

[publish]
repository = "https://thunderstore.io"
communities = ["valheim"]

[publish.categories]
valheim = [$categories]
"@
        [IO.File]::WriteAllText($configPath, $config, [Text.UTF8Encoding]::new($false))
        if ($Publish) {
            Write-Output "Publishing $($manifest.name) $($manifest.version_number) to $Team on Thunderstore"
            & $TcliPath publish --file $zipPath --config-path $configPath
            if ($LASTEXITCODE -ne 0) { throw "Publishing $($manifest.name) failed with exit code $LASTEXITCODE." }
        } else {
            Write-Output "Prepared $configPath for $zipPath"
        }
    }
} finally {
    if ($Publish) { Remove-Item Env:TCLI_AUTH_TOKEN -ErrorAction SilentlyContinue }
}
