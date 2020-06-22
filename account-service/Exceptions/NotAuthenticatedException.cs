﻿using System;

namespace account_service.Exceptions
{
    public class NotAuthenticatedException : Exception
    {
        public NotAuthenticatedException() : base("You are not authenticated to do this") { }
    }
}