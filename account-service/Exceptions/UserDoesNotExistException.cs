using System;

namespace account_service.Exceptions
{
    public class UserDoesNotExistException :Exception
    {
        public UserDoesNotExistException() : base("There is no user with this ID")
        {

        }
    }
}