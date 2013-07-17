#requires -version 3.0
[CmdletBinding()]
param (
    [Parameter(Mandatory)]
    [string]
    $PSClrMDModulePath
)

Import-Module -Name $PSClrMDModulePath

Connect-ClrMDTarget -ProcessId $PID

"`n * Get-ClrMDClrVersion"
Get-ClrMDClrVersion

Connect-ClrMDRuntime

"`n * Get-ClrMDThread"
Get-ClrMDThread | Out-Default

Disconnect-ClrMDTarget 