using System;
using System.Linq;
using System.Management.Automation;
using Microsoft.Diagnostics.Runtime;

namespace PSClrMD
{
    [Cmdlet(VerbsCommunications.Connect, Context.DefaultCommandPrefix + "Runtime")]
    [OutputType(typeof(ClrRuntime))] // TODO wrap this instead of exposing raw object
    public class ConnectRuntimeCmdlet : ClrMDTargetCmdlet
    {
        [Parameter]
        public SwitchParameter PassThru { get; set; }

        [Parameter]
        public Version ClrVersion { get; set; }

        protected override void ProcessRecord()
        {
            var target = GetTarget();

            if (!PassThru.IsPresent)
            {
                // TODO require a Disconnect-Runtime cmdlet?
                if (target != Context.DefaultDataTarget)
                {
                    throw new PSClrMDException("Target is not the default. Use the default or specify PassThru instead.");
                }
            }

            if (target.ClrVersions.Count == 0)
            {
                throw new PSClrMDException("Target does not have the CLR loaded.");
            }

            var clrInfo = target.ClrVersions[0];
            if (ClrVersion != null)
            {
                clrInfo = target.ClrVersions.FirstOrDefault(i => Context.ConvertToVersion(i.Version) == ClrVersion);
                if (clrInfo == null)
                {
                    throw new PSClrMDException(string.Format("Target does not have CLR version '{0}' loaded.", ClrVersion));
                }
            }
            WriteVerbose(string.Format("Using CLR version '{0}'.", clrInfo.Version));

            var dacLocation = clrInfo.TryGetDacLocation();
            if (string.IsNullOrEmpty(dacLocation))
            {
                dacLocation = clrInfo.DacInfo.FileName;
            }
            WriteVerbose(string.Format("Using CLR data access component '{0}'.", dacLocation));

            var runtime = target.CreateRuntime(dacLocation);

            if (PassThru.IsPresent)
            {
                WriteObject(runtime);
            }
            else
            {
                Context.DefaultClrRuntime = runtime;
            }

        }
    }
}
