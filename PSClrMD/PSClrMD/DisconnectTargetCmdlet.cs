using System.Management.Automation;
using Microsoft.Diagnostics.Runtime;

namespace PSClrMD
{
    [Cmdlet(VerbsCommunications.Disconnect, Context.DefaultCommandPrefix + "Target")]
    public class DisconnectTargetCmdlet : PSCmdlet
    {
        [Parameter(ValueFromPipeline = true)]
        public DataTarget[] Target { get; set; }

        protected override void ProcessRecord()
        {
            if (MyInvocation.BoundParameters.ContainsKey("Target"))
            {
                if (Target != null)
                {
                    foreach (var singleTarget in Target)
                    {
                        singleTarget.Dispose();
                    }
                }
            }
            else
            {
                var target = Context.DefaultDataTarget;
                Context.DefaultDataTarget = null;
                Context.DefaultClrRuntime = null;
                if (target == null)
                {
                    WriteError(new ErrorRecord(new PSClrMDException("Not connected."), "NotConnected", ErrorCategory.InvalidOperation, null));
                    return;
                }
                target.Dispose();
            }
        }
    }
}