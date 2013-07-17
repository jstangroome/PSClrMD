using System.Management.Automation;
using Microsoft.Diagnostics.Runtime;

namespace PSClrMD
{
    public abstract class ClrMDRuntimeCmdlet : PSCmdlet
    {
        [Parameter]
        public ClrRuntime Runtime { get; set; }

        protected ClrRuntime GetRuntime()
        {
            if (Runtime != null) return Runtime; // TODO check PSBoundParameters for explicitly null Runtime parameter?

            var runtime = Context.DefaultClrRuntime;
            if (runtime != null) return runtime;

            throw new PSClrMDException("Connect to a default Runtime or specify Runtime explicitly.");
        }
    }
}