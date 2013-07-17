using System;
using System.Management.Automation;

namespace PSClrMD
{
    [Cmdlet(VerbsCommon.Get, Context.DefaultCommandPrefix + "ClrVersion")]
    [OutputType(typeof(Version))]
    public class GetClrVersionCmdlet : ClrMDTargetCmdlet
    {
        protected override void ProcessRecord()
        {
            var target = GetTarget();

            foreach (var info in target.ClrVersions)
            {
                WriteObject(Context.ConvertToVersion(info.Version));
            }
        }
    }
}