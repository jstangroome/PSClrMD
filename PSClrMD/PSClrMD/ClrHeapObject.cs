using System;
using Microsoft.Diagnostics.Runtime;

namespace PSClrMD
{
    public class ClrHeapObject
    {
        public ClrHeapObject(ulong address, ClrType type, ulong size, int generation, object simpleValue, ClrException exceptionValue)
        {
            Address = address;
            Type = type;
            Size = size;
            Generation = generation;
            SimpleValue = simpleValue;
            ExceptionValue = exceptionValue;
        }

        public ulong Address { get; private set; }
        public ClrType Type { get; private set; }
        public ulong Size { get; private set; }
        public int Generation { get; private set; }
        public object SimpleValue { get; private set; }
        public ClrException ExceptionValue { get; private set; }

        private string[] TypeNameParts()
        {
            var name = Type.Name;
            var lastDot = name.LastIndexOf(".", StringComparison.Ordinal);
            if (lastDot < 0) return new[] {"", name};
            return new[] {name.Substring(0, lastDot), name.Substring(lastDot + 1)};
        }

        public string TypeClassName
        {
            get { return TypeNameParts()[1]; }
        }

        public string TypeNamespace
        {
            get { return TypeNameParts()[0]; }
        }

    }
}