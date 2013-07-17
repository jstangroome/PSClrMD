#requires -version 3.0
[CmdletBinding()]
param (
    [Parameter(Mandatory)]
    [string]
    $PSClrMDModulePath
)

Import-Module -Name $PSClrMDModulePath

$PowerShellPath = (Get-Process -Id $PID).Path

$NewProcess = Start-Process -FilePath $PowerShellPath -PassThru -WindowStyle Minimized

# wait for PowerShell to load the CLR so we can inspect it
while (-not ($NewProcess.Modules | Where-Object { $_.ModuleName -eq 'clr.dll' })) {
    Start-Sleep -Milliseconds 500
    $NewProcess.Refresh()
}

Connect-ClrMDTarget -ProcessId $NewProcess.Id -AttachFlag NonInvasive

"`n * Get-ClrMDClrVersion"
Get-ClrMDClrVersion

Connect-ClrMDRuntime

"`n * Get-ClrMDThread"
Get-ClrMDThread | Out-Default

Disconnect-ClrMDTarget 

$NewProcess | Stop-Process -Force