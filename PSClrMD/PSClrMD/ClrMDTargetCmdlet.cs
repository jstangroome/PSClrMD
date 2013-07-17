using System.Management.Automation;
using Microsoft.Diagnostics.Runtime;

namespace PSClrMD
{
    public abstract class ClrMDTargetCmdlet : PSCmdlet
    {
        [Parameter]
        public DataTarget Target { get; set; }

        protected DataTarget GetTarget()
        {
            if (Target != null) return Target; // TODO check PSBoundParameters for explicitly null Target parameter?

            var target = Context.DefaultDataTarget;
            if (target != null) return target;

            throw new PSClrMDException("Not connected.");
        }
    }
}