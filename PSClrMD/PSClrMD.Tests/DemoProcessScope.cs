using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PSClrMD.Tests
{
    class DemoProcessScope : IDisposable
    {
        private Process _demoProcess;

        public DemoProcessScope()
        {
            var demoAssembly = typeof(DemoTarget.Form1).Assembly.Location;
            _demoProcess = Process.Start(demoAssembly);
            Assert.IsNotNull(_demoProcess, "Demo process should have started.");
        }

        public Process Process
        {
            get { return _demoProcess; }
        }

        public void Dispose()
        {
            var process = Interlocked.Exchange(ref _demoProcess, null);
            if (process == null) return;
            if (!process.HasExited) process.Kill();
        }
    }
}