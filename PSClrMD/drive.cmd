@setlocal
@set binaries=%~dp0..\Binaries

%SYSTEMROOT%\system32\WindowsPowerShell\v1.0\powershell.exe -noprofile -executionpolicy remotesigned -command "%~dpn0.ps1" "%binaries%\PSClrMD"
