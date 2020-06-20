using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using account_service.DatastoreSettings;
using account_service.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace account_service.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly IMongoCollection<Account> _accounts;

        public ProfileRepository(IAccountstoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _accounts = database.GetCollection<Account>(settings.AccountCollectionName);
        }

        public async Task<List<User>> GetFollowers(Guid id) =>
            await _accounts.Find(account => account.Id == id).Project(account => account.Profile.Followers)
                .FirstAsync();

        public async Task<List<User>> GetFollowing(Guid id) =>
            await _accounts.Find(account => account.Id == id).Project(account => account.Profile.Following)
                .FirstAsync();

        public async Task<Profile> GetProfileByGuid(Guid id) =>
            await _accounts.Find(account => account.Id == id)
                .Project(account => account.Profile).FirstOrDefaultAsync();

        public async Task UpdateProfile(Guid id, Profile profile) =>
            await _accounts.UpdateOneAsync(Builders<Account>.Filter.Eq("Id", id),
                Builders<Account>.Update.Set("Profile", profile));


        // public async Task<List<Profile>> Get() =>
        //     await _accounts.Find(account => true)
        //         .Project(Blog => Blog.Profile).ToListAsync();
        //
        // public async Task<Profile> GetByEmail(string email) =>
        //     await _accounts.Find(account => account.Email == email)
        //         .Project(Blog => Blog.Profile).FirstOrDefaultAsync();
        //
        // public async Task<Profile> GetByUsername(string username) =>
        //     await _accounts.Find(account => account.Profile.Username == username)
        //         .Project(Blog => Blog.Profile).FirstOrDefaultAsync();
        //
        //
        //
        // public async Task<Profile> Get(Guid id) =>
        //     await _accounts.Find<Account>(book => book.Id == id)
        //         .Project(Blog => Blog.Profile).FirstOrDefaultAsync();
        //
        // public async Task<Account> Create(Account account)
        // {
        //     await _accounts.InsertOneAsync(account);
        //     return account;
        // }
        //
        // public async Task Update(Guid id, Account accountIn) =>
        //     await _accounts.ReplaceOneAsync(account => account.Id == id, accountIn);
        //
        // public async void Remove(Account accountIn) =>
        //     await _accounts.DeleteOneAsync(account => account.Id == accountIn.Id);
        //
        // public async Task Remove(Guid id) =>
        //     await _accounts.DeleteOneAsync(account => account.Id == id);
    }
}