﻿using System.Threading.Tasks;
 using account_service.DatastoreSettings;
 using account_service.Repositories;
 using Mongo2Go;
 using Xunit;
using Xunit.Abstractions;

namespace account_service_test.Repositories
{
    public class UserRepositoryTest
    {
        private readonly MongoDbRunner _mongoDbRunner;
        private readonly IAccountRepository _accountRepository;
        public UserRepositoryTest(ITestOutputHelper testOutputHelper)
        {
            _mongoDbRunner = MongoDbRunner.Start();
            var settings = new AccountstoreDatabaseSettings
            {
                ConnectionString = _mongoDbRunner.ConnectionString,
                DatabaseName = "IntegrationTest",
                AccountCollectionName = "TestCollection"
            };
            _accountRepository = new AccountRepository(settings);
        }

        public void Dispose()
        {
            _mongoDbRunner?.Dispose();
        }

        [Fact]
        public async Task Get_Successful()
        {

        }
    }
}