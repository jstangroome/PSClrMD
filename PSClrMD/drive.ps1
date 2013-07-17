#requires -version 3.0
[CmdletBinding()]
param (
    [Parameter(Mandatory)]
    [string]
    $PSClrMDModulePath
)

Import-Module -Name $PSClrMDModulePath

"`n * Get-Command -Module PSClrMD"
Get-Command -Module PSClrMD

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

"`n * Get-ClrMDHeapObject"
Get-ClrMDHeapObject | Select-Object -First 20 | Out-Default

"`n * Get-ClrMDHeapObject | Where-Object { $_.IsString }"
Get-ClrMDHeapObject | 
    Where-Object { $_.IsString } | 
    Select-Object -Unique -First 20 -Property SimpleValue |
    Out-Default

"`n * Get-ClrMDHeapObject | Group-Object TypeName"
Get-ClrMDHeapObject | 
    Select-Object -First 1000 |
    Group-Object -Property TypeName |
    Select-Object -Property Count, @{
        N = 'TotalSize' 
        E = { ($_.Group | Measure-Object -Property Size -Sum).Sum }
    }, Name |
    Sort-Object -Property TotalSize -Descending |
    Select-Object -First 20 |
    Format-Table -AutoSize

Disconnect-ClrMDTarget 

$NewProcess | Stop-Process -Force