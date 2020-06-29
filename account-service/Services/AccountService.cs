using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using account_service.Entities;
using account_service.Exceptions;
using account_service.Helpers;
using account_service.MQ;
using account_service.MQ.Publisher;
using account_service.Repositories;
using MessageBroker;
using Microsoft.Extensions.Options;


namespace account_service.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repository;
        private readonly IHasher _hasher;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IMessageQueuePublisher _messageQueuePublisher;
        private readonly MessageQueueSettings _messageQueueSettings;

        private readonly IAccountKwetPublisher _accountKwetPublisher;
        private readonly IJwtIdClaimReaderHelper _jwtIdClaimReaderHelper;


        public AccountService(IAccountRepository repository, IHasher hasher, ITokenGenerator tokenGenerator,
            IMessageQueuePublisher messageQueuePublisher, IOptions<MessageQueueSettings> messageQueueSettings, IAccountKwetPublisher accountKwetPublisher, IJwtIdClaimReaderHelper jwtIdClaimReaderHelper)
        {
            _repository = repository;
            _hasher = hasher;
            _tokenGenerator = tokenGenerator;
            _messageQueuePublisher = messageQueuePublisher;
            _messageQueueSettings = messageQueueSettings.Value;

            _accountKwetPublisher = accountKwetPublisher;
            _jwtIdClaimReaderHelper = jwtIdClaimReaderHelper;
        }

        public async Task Fill()
        {
            var list = await _repository.Get();
            if (list.Count != 0) return;

            await _repository.Create(new Account()
            {
                Name = "Test 1",
                Email = "test1@gmail.com"
            });

            await _repository.Create(new Account()
            {
                Name = "Test 2",
                Email = "test2@gmail.com"
            });

            await _repository.Create(new Account()
            {
                Name = "Test 3",
                Email = "test3@gmail.com"
            });
        }

        // TODO Custom Exceptions
        public async Task<Account> Authenticate(string email, string password)
        {
            var account = await _repository.GetByEmail(email);
            if (account == null)
                throw new AccountByEmailOrPasswordNotFoundException(
                    "A user with this email or password combination does not exist");
            // AppException("There is no user with this email");

            if (!await _hasher.VerifyHash(password, account.Salt, account.Password))
                throw new AccountByEmailOrPasswordNotFoundException(
                    "A user with this email or password combination does not exist");
            // AppException("The password is not correct");

            account.Token = _tokenGenerator.CreateToken(account.Id);

            return account.RemovePassword().RemoveSalt();
        }

        public async Task<Account> Create(string name, string email, string username, string password)
        {
            var accountByEmail = await _repository.GetByEmail(email.ToLower());
            if (accountByEmail != null)
                throw new AlreadyInUseException("A user with this email is already registered.");

            if (await _repository.GetByUsername(username.ToLower()) != null)
                throw new AlreadyInUseException("A user with this username is already registered.");

            var salt = _hasher.CreateSalt();
            var _password = await _hasher.HashPassword(password, salt);

            var account = new Account()
            {
                Email = email.ToLower(),
                Name = name,
                Salt = salt,
                Profile = new Profile()
                {
                    Username = username.ToLower(),
                    Created = DateTime.Now,
                    Followers = new List<User>(),
                    Following = new List<User>()
                },
                Password = _password,
                OauthIssuer = "none",
            };

            await _repository.Create(account);

            await _messageQueuePublisher.PublishMessageAsync(_messageQueueSettings.Exchange, "email-service",
                "RegisterUser", new {Email = account.Email});

            return account.RemovePassword().RemoveSalt();
        }

        public async Task<Account> GetUserByGuid(Guid id)
        {
            var account = await _repository.GetByGuid(id);
            if (account == null) throw new AccountDoesNotExistException();
            return account.RemovePassword().RemoveSalt();
        }

        public async Task<Account> UpdateAccount(Guid id, string username, string jwt)
        {
            if (id != _jwtIdClaimReaderHelper.getUserIdFromToken(jwt))
            {
                throw new NotAuthenticatedException();
            }

            if (await _repository.GetByUsername(username.ToLower()) != null) throw new AlreadyInUseException("A user with this username is already registered.");

            var account = await _repository.GetByGuid(id);

            if (account == null) throw new AccountDoesNotExistException();

            account.Profile.Username = username;

            await _repository.Update(id, account);

            await _accountKwetPublisher.PublishUpdateUser(account);
            return account.RemovePassword().RemoveSalt();
        }

        public async Task DeleteAccount(Guid id)
        {
            if (await _repository.GetByGuid(id) == null)
            {
                throw new AccountDoesNotExistException();
            }

            await _accountKwetPublisher.PublishDeleteUser(id);
            await _repository.Remove(id);
        }

        public async Task<List<Account>> GetAll()
        {
            return await _repository.Get();
        }
    }
}