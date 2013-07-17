using System;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PSClrMD.Tests
{
    [TestClass]
    public class CrashDumpTests : ClrMDModuleScenario
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void CLRMDModule_should_get_CLR_version_from_crash_dump()
        {
            var dumpPath = Path.Combine(TestContext.TestRunResultsDirectory, "powershell.dmp");

            var process = Process.Start("powershell.exe");
            while (!process.HasExited &&
                !process.Modules.Cast<ProcessModule>().Any(m => m.ModuleName.Equals("clr.dll", StringComparison.OrdinalIgnoreCase)))
            {
                // wait for powershell to load CLR
                Thread.Sleep(500);
                process.Refresh();
            }
            MiniDump.CreateDump(process, dumpPath);
            process.Kill();

            using (var testScope = PowerShellWithClrMDModuleConnectedToCrashDump(dumpPath))
            {
                var shell = testScope.Shell;
                
                shell.AddCommand<GetClrVersionCmdlet>();
                var output = shell.Invoke();
                shell.Commands.Clear();
                
                Assert.AreEqual(1, output.Count);

                var version = (Version)output[0].BaseObject;
                Assert.AreEqual(4, version.Major);

                Assert.AreEqual(0, shell.Streams.Error.Count);
            }
        }
    }
}
