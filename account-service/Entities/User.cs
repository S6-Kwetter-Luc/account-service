using System;
using MongoDB.Bson.Serialization.Attributes;

namespace account_service.Entities
{
    public class User
    {
        [BsonId] public Guid Id { get; set; }
        public string Username { get; set; }
    }
}