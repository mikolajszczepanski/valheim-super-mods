param(
    [Parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [string[]] $Plugin,
    [ValidateSet('Debug', 'Release')]
    [string] $Configuration = 'Debug',
    [string] $GaleProfile,
    [string] $ValheimPluginsPath
)

$ErrorActionPreference = 'Stop'
$solutionRoot = Join-Path $PSScriptRoot 'ValheimMods'
$projects = @(Get-ChildItem -LiteralPath $solutionRoot -Directory | Where-Object {
    Test-Path -LiteralPath (Join-Path $_.FullName ($_.Name + '.csproj'))
})
$selected = @($Plugin | Select-Object -Unique)
$unknown = @($selected | Where-Object { $_ -notin $projects.Name })
if ($unknown.Count -gt 0) { throw "Unknown plugin(s): $($unknown -join ', ')" }
if ($selected.Count -eq 0) { throw 'Select at least one plugin to copy.' }

# Check all requested outputs before copying any plugin. This script never builds.
foreach ($name in $selected) {
    $dllPath = Join-Path $solutionRoot "$name\bin\$Configuration\netstandard2.1\$name.dll"
    if (-not (Test-Path -LiteralPath $dllPath -PathType Leaf)) {
        throw "Build output not found: $dllPath. Build the plugin in $Configuration configuration first."
    }
}

foreach ($name in $selected) {
    $project = $projects | Where-Object Name -eq $name
    $copyArgs = @(
        'msbuild', (Join-Path $project.FullName ($project.Name + '.csproj')),
        '-target:CopyValheimPluginDlls', '-verbosity:minimal',
        "-property:Configuration=$Configuration", "-property:ValheimPluginsToCopy=$($project.Name)"
    )
    if ($GaleProfile) { $copyArgs += "-property:GaleProfile=$GaleProfile" }
    if ($ValheimPluginsPath) { $copyArgs += "-property:ValheimPluginsPath=$ValheimPluginsPath" }
    & dotnet @copyArgs
    if ($LASTEXITCODE -ne 0) { throw "Copying $name failed with exit code $LASTEXITCODE." }
}
