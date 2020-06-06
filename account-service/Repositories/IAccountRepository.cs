using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using account_service.Entities;

namespace account_service.Repositories
{
    public interface IAccountRepository
    {
        /// <summary>
        /// Gets a list of all the users
        /// </summary>
        /// <returns>List of all users</returns>
        Task<List<Account>> Get();

        /// <summary>
        /// Gets a single user by their email address
        /// </summary>
        /// <param name="email">the email address to be searched for</param>
        /// <returns>User with the correct email address</returns>
        Task<Account> GetByEmail(string email);

        Task<Account> GetByUsername(string username);
        Task<Account> GetByGuid(Guid id);

        /// <summary>
        /// <param name="id">Guid to search for</para
        /// Gets a single user by their Guid
        /// </summary>m>
        /// <returns>User with the correct Guid</returns>
        Task<Account> Get(Guid id);

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="account">User to be saved</param>
        /// <returns></returns>
        Task<Account> Create(Account account);

        /// <summary>
        /// Updates an existing user
        /// </summary>
        /// <param name="id">Guid of the user</param>
        /// <param name="accountIn">User with updated fields</param>
        /// <returns>Updated user</returns>
        Task Update(Guid id, Account accountIn);

        /// <summary>
        /// Removes an user
        /// </summary>
        /// <param name="accountIn">User to remove</param>
        void Remove(Account accountIn);

        /// <summary>
        /// Removes an user by their Guid
        /// </summary>
        /// <param name="id">Guid of the user te remove</param>
        /// <returns>Async tas to await</returns>
        Task Remove(Guid id);
    }
}