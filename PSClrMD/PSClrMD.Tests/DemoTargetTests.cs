using System;
using System.Linq;
using Microsoft.Diagnostics.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PSClrMD.Tests
{
    [TestClass]
    public class DemoTargetTests : ClrMDModuleScenario
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

                var version = (Version)output[0].BaseObject;
                Assert.AreEqual(4, version.Major);

                Assert.AreEqual(0, shell.Streams.Error.Count);
            }
        }

        [TestMethod]
        public void CLRMDModule_should_get_threads()
        {
            using (var demoScope = new DemoProcessScope())
            using (var testScope = PowerShellWithClrMDModuleConnectedToProcess(demoScope.Process))
            {
                var shell = testScope.Shell;

                shell.AddCommand<ConnectRuntimeCmdlet>();
                shell.Invoke();
                shell.Commands.Clear();

                shell.AddCommand<GetThreadCmdlet>();
                var output = shell.Invoke();
                shell.Commands.Clear();

                var clrThreads = output.Select(o => o.BaseObject).OfType<ClrThread>();

                Assert.AreNotEqual(0, clrThreads.Count(), "Should have threads.");

                Assert.AreEqual(0, shell.Streams.Error.Count);
            }
        }

        [TestMethod]
        public void CLRMDModule_should_get_heap_objects()
        {
            using (var demoScope = new DemoProcessScope())
            using (var testScope = PowerShellWithClrMDModuleConnectedToProcess(demoScope.Process))
            {
                var shell = testScope.Shell;

                shell.AddCommand<ConnectRuntimeCmdlet>();
                shell.Invoke();
                shell.Commands.Clear();

                shell.AddCommand<GetHeapObjectCmdlet>();
                var output = shell.Invoke();
                shell.Commands.Clear();

                var heapObjects = output.Select(o => o.BaseObject).OfType<ClrHeapObject>();

                Assert.AreNotEqual(0, heapObjects.Count(), "Should have heap objects.");

                Assert.AreEqual(0, shell.Streams.Error.Count);
            }
        }

    }
}
