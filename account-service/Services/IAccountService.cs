using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using account_service.Entities;

namespace account_service.Services
{
    public interface IAccountService
    {
        // Task Update(User user, string password = null);
        // Task Delete(int id);

        /// <summary>
        /// Used to log the user in
        /// </summary>
        /// <param name="email">Email-address of the user</param>
        /// <param name="password">Password of the user</param>
        /// <returns>Authenticated user</returns>
        Task<Account> Authenticate(string email, string password);

        /// <summary>
        /// Used to create new users
        /// </summary>
        /// <param name="name">Account name</param>
        /// <param name="email"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>user without password</returns>
        Task<Account> Create(string name, string email, string username, string password);

        /// <summary>
        /// Get list of all users
        /// </summary>
        /// <returns>List of all users</returns>
        Task<List<Account>> GetAll();

        Task<Account> GetUserByGuid(Guid id);

        /// <summary>
        /// Add 3 new users if the db is empty
        /// </summary>
        /// <returns></returns>
        Task Fill();
    }
}