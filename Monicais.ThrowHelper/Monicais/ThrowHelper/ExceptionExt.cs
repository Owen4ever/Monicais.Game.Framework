
using System;

namespace Monicais.ThrowHelper
{

    public class AlreadyContainException : Exception
    {
        public AlreadyContainException() { }

        public AlreadyContainException(string msg) : base(msg) { }

        public AlreadyContainException(string msg, Exception innerException) : base(msg, innerException) { }
    }
}

