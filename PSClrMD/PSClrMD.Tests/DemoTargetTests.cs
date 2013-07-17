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
            using (var testScope = PowerShellWithClrMDModuleConnectedToProcess(demoScope.Process))
            {
                var shell = testScope.Shell;
                
                shell.AddCommand<GetClrVersionCmdlet>();
                var output = shell.Invoke();
                shell.Commands.Clear();
                
                Assert.AreEqual(1, output.Count);

                var version = (VersionInfo)output[0].BaseObject;
                Assert.AreEqual(4, version.Major);

                Assert.AreEqual(0, shell.Streams.Error.Count);
            }
        }
    }
}
