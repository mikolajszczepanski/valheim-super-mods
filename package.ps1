param(
    [string[]] $Plugin = @(),
    [string] $Configuration = 'Release',
    [switch] $SkipBuild,
    [string] $OutputDirectory,
    [string] $GaleProfile,
    [string] $ValheimInstall,
    [string] $ValheimPluginsPath
)

$ErrorActionPreference = 'Stop'
$repoRoot = $PSScriptRoot
$solutionRoot = Join-Path $repoRoot 'ValheimMods'
$solution = Join-Path $solutionRoot 'ValheimMods.sln'
$icon = Join-Path $solutionRoot 'Publishing\icon.png'
$license = Join-Path $repoRoot 'LICENSE'
$dist = if ($OutputDirectory) { [IO.Path]::GetFullPath($OutputDirectory) } else { Join-Path $repoRoot 'dist' }

$projects = @(Get-ChildItem -LiteralPath $solutionRoot -Directory | Where-Object {
    Test-Path -LiteralPath (Join-Path $_.FullName ($_.Name + '.csproj'))
})
if ($Plugin.Count -gt 0) {
    $unknown = @($Plugin | Where-Object { $_ -notin $projects.Name })
    if ($unknown.Count -gt 0) { throw "Unknown plugin(s): $($unknown -join ', ')" }
    $projects = @($projects | Where-Object { $_.Name -in $Plugin })
}
if ($projects.Count -eq 0) { throw 'No plugin projects were selected.' }

if (-not $SkipBuild) {
    $buildArgs = @('build', $solution, '-c', $Configuration)
    if ($GaleProfile) { $buildArgs += "-p:GaleProfile=$GaleProfile" }
    if ($ValheimInstall) { $buildArgs += "-p:ValheimInstall=$ValheimInstall" }
    if ($ValheimPluginsPath) { $buildArgs += "-p:ValheimPluginsPath=$ValheimPluginsPath" }
    & dotnet @buildArgs
    if ($LASTEXITCODE -ne 0) { throw "dotnet build failed with exit code $LASTEXITCODE." }
}

$iconBytes = [IO.File]::ReadAllBytes($icon)
if (-not (Test-Path -LiteralPath $license -PathType Leaf)) { throw "Missing license file: $license" }
$pngSignature = @(137, 80, 78, 71, 13, 10, 26, 10)
for ($i = 0; $i -lt $pngSignature.Count; $i++) {
    if ($iconBytes[$i] -ne $pngSignature[$i]) { throw 'Publishing icon is not a PNG.' }
}
$iconWidth = [BitConverter]::ToInt32(@($iconBytes[19], $iconBytes[18], $iconBytes[17], $iconBytes[16]), 0)
$iconHeight = [BitConverter]::ToInt32(@($iconBytes[23], $iconBytes[22], $iconBytes[21], $iconBytes[20]), 0)
if ($iconWidth -ne 256 -or $iconHeight -ne 256) { throw 'Publishing icon must be 256x256.' }

New-Item -ItemType Directory -Force -Path $dist | Out-Null
Add-Type -AssemblyName System.IO.Compression
Add-Type -AssemblyName System.IO.Compression.FileSystem

foreach ($project in $projects) {
    $name = $project.Name
    $projectDir = $project.FullName
    $manifestPath = Join-Path $projectDir 'manifest.json'
    $readmePath = Join-Path $projectDir 'README.md'
    $pluginSource = Join-Path $projectDir 'Plugin.cs'
    $dllPath = Join-Path $projectDir "bin\$Configuration\netstandard2.1\$name.dll"
    foreach ($path in @($manifestPath, $readmePath, $pluginSource, $dllPath)) {
        if (-not (Test-Path -LiteralPath $path -PathType Leaf)) { throw "Missing required file: $path" }
    }

    $manifest = Get-Content -LiteralPath $manifestPath -Raw | ConvertFrom-Json
    if ($manifest.name -cne $name) { throw "$name manifest name must match the project directory." }
    if ($manifest.version_number -notmatch '^\d+\.\d+\.\d+$') { throw "$name has an invalid Thunderstore version." }
    if ($manifest.description.Length -gt 250) { throw "$name description is longer than 250 characters." }
    if ($manifest.dependencies -notcontains 'denikson-BepInExPack_Valheim-5.4.2351') {
        throw "$name must declare the Valheim BepInEx pack dependency."
    }
    $source = Get-Content -LiteralPath $pluginSource -Raw
    $match = [regex]::Match($source, '\[BepInPlugin\([^\r\n]*,\s*"[^"]+",\s*"(?<version>[^"]+)"\)\]')
    if (-not $match.Success) { throw "Could not read the BepInPlugin version for $name." }
    if ($match.Groups['version'].Value -cne $manifest.version_number) {
        throw "$name BepInPlugin version and manifest version differ."
    }

    $zipPath = Join-Path $dist "$name-$($manifest.version_number).zip"
    if (Test-Path -LiteralPath $zipPath) { throw "Package already exists: $zipPath" }
    $archive = [IO.Compression.ZipFile]::Open($zipPath, [IO.Compression.ZipArchiveMode]::Create)
    try {
        [IO.Compression.ZipFileExtensions]::CreateEntryFromFile($archive, $manifestPath, 'manifest.json') | Out-Null
        [IO.Compression.ZipFileExtensions]::CreateEntryFromFile($archive, $readmePath, 'README.md') | Out-Null
        [IO.Compression.ZipFileExtensions]::CreateEntryFromFile($archive, $icon, 'icon.png') | Out-Null
        [IO.Compression.ZipFileExtensions]::CreateEntryFromFile($archive, $license, 'LICENSE') | Out-Null
        [IO.Compression.ZipFileExtensions]::CreateEntryFromFile($archive, $dllPath, "$name.dll") | Out-Null
    } finally {
        $archive.Dispose()
    }
    Write-Output "Packaged $zipPath"
}
