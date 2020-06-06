using System.Collections.Generic;
using System.Linq;
using account_service.Entities;

namespace account_service.Helpers
{
    public static class ExtensionMethods
    {
        public static IEnumerable<Account> WithoutPasswords(this IEnumerable<Account> users)
        {
            return users.Select(x => x.RemovePassword());
        }

        public static Account RemovePassword(this Account account)
        {
            account.Password = null;
            return account;
        }

        public static Account RemoveSalt(this Account account)
        {
            account.Salt = null;
            return account;
        }
    }
}