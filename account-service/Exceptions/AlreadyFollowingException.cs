using System;

namespace account_service.Exceptions
{
    public class AlreadyFollowingException : Exception
    {
        public AlreadyFollowingException() : base("Can't follow this user agane")
        {
        }
    }
}