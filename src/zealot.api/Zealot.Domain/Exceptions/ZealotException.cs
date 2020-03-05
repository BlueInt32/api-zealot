using System;

namespace Zealot.Domain.Exceptions
{
    public class ZealotException : Exception
    {
        public ZealotException(string message) : base(message)
        {
        }

        public ZealotException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
