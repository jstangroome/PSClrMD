using System;
using Microsoft.Diagnostics.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PSClrMD.Tests
{
    [TestClass]
    public class DemoTargetTests : ClrMDModuleTests
    {
        [TestMethod]
        public void CLRMDModule_should_get_CLR_version()
        {
            Assert.AreEqual(4, IntPtr.Size, "Should be in a 32-bit test runner process.");

            using (var demoScope = new DemoProcessScope())
            using (var shell = PowerShellWithClrMDModule())
            {
                shell.AddCommand<ConnectTargetCmdlet>(
                    s => s.AddParameter(c => c.ProcessId, demoScope.Process.Id)
                    );
                shell.Invoke();
                shell.Commands.Clear();

                shell.AddCommand<GetClrVersionCmdlet>();
                var output = shell.Invoke();
                shell.Commands.Clear();
                
                shell.AddCommand<DisconnectTargetCmdlet>();
                shell.Invoke();
                
                Assert.AreEqual(1, output.Count);

                var clrInfo = output[0].BaseObject as ClrInfo;
                Assert.AreEqual(4, clrInfo.Version.Major);

                Assert.AreEqual(0, shell.Streams.Error.Count);
            }
        }
    }
}
