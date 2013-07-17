using System;
using Microsoft.Diagnostics.Runtime;

namespace PSClrMD
{
    static class Context
    {
        public const string DefaultCommandPrefix = "ClrMD";

        public static DataTarget DefaultDataTarget { get; set; }
        public static ClrRuntime DefaultClrRuntime { get; set; }

        public static Version ConvertToVersion(VersionInfo versionInfo)
        {
            return new Version(versionInfo.Major, versionInfo.Minor, versionInfo.Revision, versionInfo.Patch);
        }
    }
}