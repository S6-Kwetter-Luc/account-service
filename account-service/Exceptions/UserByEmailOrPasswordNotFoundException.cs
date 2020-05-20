using System;
using System.Globalization;

namespace account_service.Exceptions
{
    public class UserByEmailOrPasswordNotFoundException : Exception
    {
        public UserByEmailOrPasswordNotFoundException() : base() {}

        public UserByEmailOrPasswordNotFoundException(string message) : base(message) { }

        public UserByEmailOrPasswordNotFoundException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}