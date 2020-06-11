using System;

namespace account_service.Exceptions
{
    public class AccountDoesNotExistException : Exception
    {
        public AccountDoesNotExistException() : base("There is no user with this ID")
        {
        }
    }
}