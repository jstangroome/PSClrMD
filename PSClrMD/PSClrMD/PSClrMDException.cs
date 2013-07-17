using System;

namespace PSClrMD
{
    public class PSClrMDException : Exception
    {
        public PSClrMDException(string message) : base(message)
        {}

        public PSClrMDException(string message, Exception innerException)
            : base(message, innerException)
        { }

    }
}