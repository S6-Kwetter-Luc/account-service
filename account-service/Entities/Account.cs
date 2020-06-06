using System;
using MongoDB.Bson.Serialization.Attributes;

namespace account_service.Entities
{
    public class Account
    {
        [BsonId] public Guid Id { get; set; }
        public string Name { get; set; }
        public Profile Profile { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
        public string OauthSubject { get; set; }
        public string OauthIssuer { get; set; }
        public string Token { get; set; }
    }
}