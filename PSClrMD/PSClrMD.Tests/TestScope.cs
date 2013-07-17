using System;
using System.Management.Automation;
using System.Threading;

namespace PSClrMD.Tests
{
    public class TestScope : IDisposable
    {
        private PowerShell _shell;
        private readonly Action<PowerShell> _onShellDispose;

        public TestScope(PowerShell shell, Action<PowerShell> onShellDispose)
        {
            _shell = shell;
            _onShellDispose = onShellDispose;
        }

        public PowerShell Shell { get { return _shell; } }

        public void Dispose()
        {
            var shell = Interlocked.Exchange(ref _shell, null);
            if (shell != null)
            {
                _onShellDispose(shell);
            }
        }
    }
}