using System;
using System.Collections.Generic;

namespace account_service.Entities
{
    /// <summary>
    /// This class is only used to track followers and following
    /// </summary>
    public class Profile
    {
        public string Username { get; set; }
        public string Bio { get; set; }
        public DateTime Created { get; set; }
        public List<User> Followers { get; set; }
        public List<User> Following { get; set; }
    }
}