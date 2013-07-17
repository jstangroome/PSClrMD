using System.Management.Automation;

namespace PSClrMD.Tests
{
    public abstract class ClrMDModuleTests
    {
        protected PowerShell PowerShellWithClrMDModule()
        {
            var moduleName = typeof (ConnectTargetCmdlet).Assembly.Location;
            var shell = PowerShell.Create();
            shell.AddCommand("Import-Module").AddParameter("Name", moduleName);
            shell.Invoke();
            shell.Commands.Clear();
            return shell;
        }
    }
}