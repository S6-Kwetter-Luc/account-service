﻿using System;
using System.IdentityModel.Tokens.Jwt;

namespace account_service.Helpers
{
    public class JwtIdClaimReaderHelper : IJwtIdClaimReaderHelper
    {
        public Guid getUserIdFromToken(string jwt)
        {
            var token = new JwtSecurityToken(jwt.Replace("Bearer ", String.Empty));
            var idclaim = Guid.Parse((string)token.Payload["unique_name"]);
            return idclaim;
        }
    }
}