using System;
using System.Globalization;

namespace account_service.Exceptions
{
    public class AccountByEmailOrPasswordNotFoundException : Exception
    {
        public AccountByEmailOrPasswordNotFoundException() : base()
        {
        }

        public AccountByEmailOrPasswordNotFoundException(string message) : base(message)
        {
        }

        public AccountByEmailOrPasswordNotFoundException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}