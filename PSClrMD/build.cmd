@setlocal
@set binaries=%~dp0..\Binaries
rmdir /s /q "%binaries%"
%SYSTEMROOT%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe "%~dp0PSClrMD.sln" /p:OutDir="%binaries%" /p:GenerateProjectSpecificOutputFolder=true
exit /b %errorlevel%