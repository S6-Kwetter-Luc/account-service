using System;

namespace account_service.Exceptions
{
    public class WasntEvenFollowingToBeginWithException : Exception
    {
        public WasntEvenFollowingToBeginWithException() : base("You didnt even follow this user!")
        {
        }
    }
}