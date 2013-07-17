using Microsoft.Diagnostics.Runtime;

namespace PSClrMD
{
    static class Context
    {
        public const string DefaultCommandPrefix = "ClrMD";

        public static DataTarget DefaultDataTarget { get; set; }
    }
}