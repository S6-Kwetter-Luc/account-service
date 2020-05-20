using System;
using System.Globalization;

namespace account_service.Exceptions
{
    public class AlreadyInUseException : Exception
    {
        public AlreadyInUseException() : base() {}

        public AlreadyInUseException(string message) : base(message) { }

        public AlreadyInUseException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}