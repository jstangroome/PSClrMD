using System;
using System.Management.Automation;
using Microsoft.Diagnostics.Runtime;

namespace PSClrMD
{
    [Cmdlet(VerbsCommunications.Connect, Context.DefaultCommandPrefix + "Target", DefaultParameterSetName = AttachParameterSet)]
    [OutputType(typeof(DataTarget))] // TODO wrap this instead of exposing raw object
    public class ConnectTargetCmdlet : PSCmdlet
    {
        private const string AttachParameterSet = "Attach";
        private const string DumpParameterSet = "Dump";

        [Parameter(Mandatory = true, ParameterSetName = AttachParameterSet)]
        public int ProcessId { get; set; }

        [Parameter(ParameterSetName = AttachParameterSet)]
        public AttachFlag AttachFlag { get; set; }

        [Parameter(ParameterSetName = AttachParameterSet)]
        public TimeSpan Timeout { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = DumpParameterSet)]
        public string CrashDumpPath { get; set; }

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

            DataTarget target;
            if (ParameterSetName == AttachParameterSet)
            {
                target = DataTarget.AttachToProcess(ProcessId, (uint) Timeout.TotalMilliseconds, AttachFlag);
            }
            else
            {
                target = DataTarget.LoadCrashDump(CrashDumpPath);
            }

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
