using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using account_service.DatastoreSettings;
using account_service.Entities;
using MongoDB.Driver;

namespace account_service.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IMongoCollection<Account> _accounts;

        public AccountRepository(IAccountstoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _accounts = database.GetCollection<Account>(settings.AccountCollectionName);
        }

        public async Task<List<Account>> Get() =>
            await _accounts.Find(account => true).ToListAsync();

        public async Task<Account> GetByEmail(string email) =>
            await _accounts.Find(account => account.Email == email).FirstOrDefaultAsync();

        public async Task<Account> GetByUsername(string username) =>
            await _accounts.Find(account => account.Profile.Username == username).FirstOrDefaultAsync();

        public async Task<Account> GetByGuid(Guid id) =>
            await _accounts.Find(account => account.Id == id).FirstOrDefaultAsync();

        public async Task<Account> Get(Guid id) =>
            await _accounts.Find<Account>(book => book.Id == id).FirstOrDefaultAsync();

        public async Task<Account> Create(Account account)
        {
            await _accounts.InsertOneAsync(account);
            return account;
        }

        public async Task Update(Guid id, Account accountIn) =>
            await _accounts.ReplaceOneAsync(account => account.Id == id, accountIn);

        public async void Remove(Account accountIn) =>
            await _accounts.DeleteOneAsync(account => account.Id == accountIn.Id);

        public async Task Remove(Guid id) =>
            await _accounts.DeleteOneAsync(account => account.Id == id);
    }
}