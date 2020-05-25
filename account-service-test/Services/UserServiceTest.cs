using System;
using System.Threading.Tasks;
using account_service.Entities;
using account_service.Exceptions;
using account_service.Helpers;
using account_service.Repositories;
using account_service.Services;
using account_service.Entities;
using account_service.Exceptions;
using account_service.Helpers;
using account_service.Repositories;
using account_service.Services;
using MongoDB.Bson;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace account_service_test.Services
{
    public class UserServiceTest
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private IUserService _service;

        public UserServiceTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        #region Authentication Tests

        [Fact]
        public async Task Authenticate_Successful()
        {
            var repository = new Mock<IUserRepository>();
            var hasher = new Hasher();
            var tokenGenerator = new Mock<ITokenGenerator>();

            var salt = hasher.CreateSalt();
            var hashedPassword = await hasher.HashPassword("testtest", salt);
            var guid = new Guid();

            repository.Setup(r => r.GetByEmail("test@test.nl")).Returns(async () => new User()
            {
                Id = guid,
                Email = "test@test.nl",
                Name = "test1",
                Salt = salt,
                Password = hashedPassword,
                OauthIssuer = "none",
                Token = "",
                OauthSubject = ""
            });

            tokenGenerator.Setup(t => t.CreateToken(guid)).Returns("");

            _service = new UserService(repository.Object, hasher, tokenGenerator.Object);

            var result = await _service.Authenticate("test@test.nl", "testtest");

            var expectedUser = new User()
            {
                Id = guid,
                Email = "test@test.nl",
                Name = "test1",
                Salt = null,
                Password = null,
                OauthIssuer = "none",
                Token = "",
                OauthSubject = ""
            };

            _testOutputHelper.WriteLine(result.ToJson());
            _testOutputHelper.WriteLine(expectedUser.ToJson());

            Assert.Equal(expectedUser.ToJson(), result.ToJson());
        }

        [Fact]
        public async Task Authenticate_WrongPassword()
        {
            var repository = new Mock<IUserRepository>();
            var hasher = new Hasher();
            var tokenGenerator = new Mock<ITokenGenerator>();

            var salt = hasher.CreateSalt();
            var hashedPassword = await hasher.HashPassword("testtest", salt);
            var guid = new Guid();

            repository.Setup(r => r.GetByEmail("test@test.nl")).Returns(async () => new User()
            {
                Id = guid,
                Email = "test@test.nl",
                Name = "test1",
                Salt = salt,
                Password = hashedPassword,
                OauthIssuer = "none",
                Token = "",
                OauthSubject = ""
            });

            tokenGenerator.Setup(t => t.CreateToken(guid)).Returns("");

            _service = new UserService(repository.Object, hasher, tokenGenerator.Object);

            Exception ex =
                await Assert.ThrowsAsync<UserByEmailOrPasswordNotFoundException>(() => _service.Authenticate("test@test.nl", "wrongpassword"));

            Assert.Equal("A user with this email or password combination does not exist", ex.Message);
        }

        [Fact]
        public async Task Authenticate_NoUser()
        {
            var repository = new Mock<IUserRepository>();
            var hasher = new Hasher();
            var tokenGenerator = new Mock<ITokenGenerator>();

            _service = new UserService(repository.Object, hasher, tokenGenerator.Object);

            Exception ex =
                await Assert.ThrowsAsync<UserByEmailOrPasswordNotFoundException>(() => _service.Authenticate("test@test.nl", "testtest"));

            Assert.Equal("A user with this email or password combination does not exist", ex.Message);
        }

        #endregion

        [Fact]
        public async Task Create_Successful()
        {
            var repository = new Mock<IUserRepository>();
            var _hasher = new Mock<IHasher>();
            var hasher = new Hasher();
            var tokenGenerator = new Mock<ITokenGenerator>();

            var salt = hasher.CreateSalt();
            var hashedPassword = await hasher.HashPassword("testtest", salt);
            var guid = new Guid();

            var user = new User()
            {
                Id = guid,
                Email = "test@test.nl",
                Name = "test1",
                Salt = salt,
                Password = hashedPassword,
                OauthIssuer = "none",
                Token = "",
                OauthSubject = ""
            };

            repository.Setup(r => r.Create(user)).Returns(async () => new User()
            {
                Id = guid,
                Email = "test@test.nl",
                Name = "test1",
                Salt = salt,
                Password = hashedPassword,
                OauthIssuer = "none",
                Token = "",
                OauthSubject = ""
            });

            _hasher.Setup(s => s.CreateSalt()).Returns(salt);
            _hasher.Setup(s => s.HashPassword("testtest", salt)).Returns(async () => hashedPassword);

            _service = new UserService(repository.Object, _hasher.Object, tokenGenerator.Object);

            var result = await _service.Create("test1", "test@test.nl", "test1", "testtest");

            var expectedUser = new User()
            {
                Id = guid,
                Email = "test@test.nl",
                Username = "test1",
                Name = "test1",
                Salt = null,
                Password = null,
                OauthIssuer = "none",
                Token = null,
                OauthSubject = null
            };

            // _testOutputHelper.WriteLine(result.ToJson());
            // _testOutputHelper.WriteLine(expectedUser.ToJson());

            Assert.Equal(expectedUser.ToJson(), result.ToJson());
        }

        [Fact]
        public async Task Create_Exception()
        {
            var repository = new Mock<IUserRepository>();
            var _hasher = new Mock<IHasher>();
            var hasher = new Hasher();
            var tokenGenerator = new Mock<ITokenGenerator>();

            var Email = "test@test.nl";

            var salt = hasher.CreateSalt();
            var hashedPassword = await hasher.HashPassword(Email, salt);
            var guid = new Guid();

            repository.Setup(r => r.GetByEmail(Email)).Returns(async () => new User()
            {
                Id = guid,
                Email = "test@test.nl",
                Name = "test1",
                Salt = salt,
                Password = hashedPassword,
                OauthIssuer = "none",
                Token = "",
                OauthSubject = ""
            });

            _service = new UserService(repository.Object, _hasher.Object, tokenGenerator.Object);

            Exception ex =
                await Assert.ThrowsAsync<AlreadyInUseException>(() => _service.Create("test1", Email, "test1", "testtest"));

            Assert.Equal("A user with this email is already registered.", ex.Message);
        }
    }
}