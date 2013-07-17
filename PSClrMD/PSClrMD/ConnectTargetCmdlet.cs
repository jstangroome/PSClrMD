using System;
using System.Management.Automation;
using Microsoft.Diagnostics.Runtime;

namespace PSClrMD
{
    [Cmdlet(VerbsCommunications.Connect, Context.DefaultCommandPrefix + "Target")]
    [OutputType(typeof(DataTarget))] // TODO wrap this instead of exposing raw object
    public class ConnectTargetCmdlet : PSCmdlet
    {
        [Parameter(Mandatory = true)]
        public int ProcessId { get; set; }

        [Parameter]
        public AttachFlag AttachFlag { get; set; }

        [Parameter]
        public TimeSpan Timeout { get; set; }

        [Parameter]
        public SwitchParameter PassThru { get; set; }

        public ConnectTargetCmdlet()
        {
            AttachFlag = AttachFlag.Passive;
            Timeout = TimeSpan.FromSeconds(10);
        }

        protected override void ProcessRecord()
        {
            if (!PassThru.IsPresent && Context.DefaultDataTarget != null)
            {
                WriteError(new ErrorRecord(new PSClrMDException("Already connected. Disconnect first or use the PassThru parameter."), "AlreadyConnected", ErrorCategory.InvalidOperation, null));
                return;
            }
            
            var target = DataTarget.AttachToProcess(ProcessId, (uint) Timeout.TotalMilliseconds, AttachFlag);

            if (PassThru.IsPresent)
            {
                WriteObject(target);
            }
            else
            {
                Context.DefaultDataTarget = target;
            }
        }
    }
}
