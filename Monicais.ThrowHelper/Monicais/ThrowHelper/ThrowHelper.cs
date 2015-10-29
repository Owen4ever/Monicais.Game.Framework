
using System;

namespace Monicais.ThrowHelper
{

    public static class AlreadyContain
    {
        public static void Throw()
        {
            throw new AlreadyContainException();
        }

        public static void Throw(string msg)
        {
            throw new AlreadyContainException(msg);
        }

        public static void Throw(string msg, Exception innerException)
        {
            throw new AlreadyContainException(msg, innerException);
        }
    }

    public static class Argument
    {
        public static void Throw()
        {
            throw new ArgumentException();
        }

        public static void Throw(string msg)
        {
            throw new ArgumentException(msg);
        }

        public static void Throw(string msg, Exception e)
        {
            throw new ArgumentException(msg, e);
        }

        public static void Throw(string msg, string paramName)
        {
            throw new ArgumentException(msg, paramName);
        }
    }

    public static class ArgumentNull
    {
        public static void Throw()
        {
            throw new ArgumentNullException();
        }

        public static void Throw(string paramName)
        {
            throw new ArgumentNullException(paramName);
        }

        public static void Throw(string msg, Exception innerException)
        {
            throw new ArgumentNullException(msg, innerException);
        }

        public static void Throw(string paramName, string msg)
        {
            throw new ArgumentNullException(paramName, msg);
        }
    }

    public static class ArgumentOutOfRange
    {
        public static void Throw()
        {
            throw new ArgumentOutOfRangeException();
        }

        public static void Throw(string paramName)
        {
            throw new ArgumentOutOfRangeException(paramName);
        }

        public static void Throw(string msg, Exception innerException)
        {
            throw new ArgumentOutOfRangeException(msg, innerException);
        }

        public static void Throw(string paramName, string msg)
        {
            throw new ArgumentOutOfRangeException(paramName, msg);
        }
    }

    public static class IndexOutOfRange
    {
        public static void Throw()
        {
            throw new IndexOutOfRangeException();
        }

        public static void Throw(string msg)
        {
            throw new IndexOutOfRangeException(msg);
        }

        public static void Throw(string msg, Exception innerException)
        {
            throw new IndexOutOfRangeException(msg, innerException);
        }
    }

    public static class NotSupported
    {
        public static void Throw()
        {
            throw new NotSupportedException();
        }

        public static void Throw(string msg)
        {
            throw new NotSupportedException(msg);
        }

        public static void Throw(string msg, Exception innerException)
        {
            throw new NotSupportedException(msg, innerException);
        }
    }
}
