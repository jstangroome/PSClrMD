using System.Diagnostics;
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

        protected TestScope PowerShellWithClrMDModuleConnectedToProcess(Process process)
        {
            var shell = PowerShellWithClrMDModule();
            shell.AddCommand<ConnectTargetCmdlet>(
                s => s.AddParameter(c => c.ProcessId, process.Id)
                );
            shell.Invoke();
            shell.Commands.Clear();

            var testScope = new TestScope(shell, s =>
                                                 {
                                                     s.Commands.Clear();
                                                     s.AddCommand<DisconnectTargetCmdlet>();
                                                     s.Invoke();
                                                 });
            return testScope;
        }

    }
}