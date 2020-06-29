﻿using System;

namespace account_service.Helpers
{
    public interface IJwtIdClaimReaderHelper
    {
        public Guid getUserIdFromToken(string jwt);
    }
}