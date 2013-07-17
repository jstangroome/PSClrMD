using System.Management.Automation;
using Microsoft.Diagnostics.Runtime;

namespace PSClrMD
{
    [Cmdlet(VerbsCommon.Get, Context.DefaultCommandPrefix + "ClrVersion")]
    [OutputType(typeof(VersionInfo))]
    public class GetClrVersionCmdlet : PSCmdlet
    {
        [Parameter]
        public DataTarget Target { get; set; }

        protected DataTarget GetTarget()
        {
            if (Target != null) return Target;

            var target = Context.DefaultDataTarget;
            if (target != null) return target;

            throw new PSClrMDException("Not connected.");
        }

        protected override void ProcessRecord()
        {
            var target = GetTarget();

            foreach (var info in target.ClrVersions)
            {
                WriteObject(info.Version);
            }
        }
    }
}