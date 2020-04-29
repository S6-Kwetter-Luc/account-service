﻿using System.Collections.Generic;
using System.Linq;
using account_service.Entities;

namespace account_service.Helpers
{
    public static class ExtensionMethods
    {
        public static IEnumerable<User> WithoutPasswords(this IEnumerable<User> users) {
            return users.Select(x => x.RemovePassword());
        }

        public static User RemovePassword(this User user) {
            user.Password = null;
            return user;
        }

        public static User RemoveSalt(this User user) {
            user.Salt = null;
            return user;
        }
    }
}