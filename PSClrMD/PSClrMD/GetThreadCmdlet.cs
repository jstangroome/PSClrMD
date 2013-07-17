using System.Management.Automation;
using Microsoft.Diagnostics.Runtime;

namespace PSClrMD
{
    [Cmdlet(VerbsCommon.Get, Context.DefaultCommandPrefix + "Thread")]
    [OutputType(typeof(ClrThread))]
    public class GetThreadCmdlet : ClrMDRuntimeCmdlet
    {
        protected override void ProcessRecord()
        {
            var runtime = GetRuntime();

            WriteObject(runtime.Threads, enumerateCollection: true);
        }
    }
}