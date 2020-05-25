using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using account_service.Entities;
using account_service.Exceptions;
using account_service.Helpers;
using account_service.Repositories;


namespace account_service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IHasher _hasher;
        private readonly ITokenGenerator _tokenGenerator;

        public UserService(IUserRepository repository, IHasher hasher, ITokenGenerator tokenGenerator)
        {
            _repository = repository;
            _hasher = hasher;
            _tokenGenerator = tokenGenerator;
        }

        public async Task Fill()
        {
            var list = await _repository.Get();
            if (list.Count != 0) return;

            await _repository.Create(new User()
            {
                Name = "Test 1",
                Email = "test1@gmail.com"
            });

            await _repository.Create(new User()
            {
                Name = "Test 2",
                Email = "test2@gmail.com"
            });

            await _repository.Create(new User()
            {
                Name = "Test 3",
                Email = "test3@gmail.com"
            });
        }

        // TODO Custom Exceptions
        public async Task<User> Authenticate(string email, string password)
        {
            var user = await _repository.GetByEmail(email);
            if (user == null) throw new UserByEmailOrPasswordNotFoundException("A user with this email or password combination does not exist");
            // AppException("There is no user with this email");

            if (!await _hasher.VerifyHash(password, user.Salt, user.Password))
                throw new UserByEmailOrPasswordNotFoundException("A user with this email or password combination does not exist");
            // AppException("The password is not correct");

            user.Token = _tokenGenerator.CreateToken(user.Id);

            return user.RemovePassword().RemoveSalt();
        }

        public async Task<User> Create(string name, string email, string username, string password)
        {
            var emailuser = await _repository.GetByEmail(email.ToLower());
            if (emailuser != null) throw new AlreadyInUseException("A user with this email is already registered.");

            if (await _repository.GetByUsername(username.ToLower()) != null) throw new AlreadyInUseException("A user with this username is already registered.");

            var salt = _hasher.CreateSalt();
            var _password = await _hasher.HashPassword(password, salt);
            var user = new User()
            {
                Email = email.ToLower(),
                Name = name,
                Salt = salt,
                Username = username.ToLower(),
                Password = _password,
                OauthIssuer = "none",
            };
            await _repository.Create(user);

            return user.RemovePassword().RemoveSalt();
        }

        public async Task<User> GetUserByGuid(Guid id)
        {
            var user = await _repository.GetByGuid(id);
            if (user == null) throw new UserDoesNotExistException();
            return user;
        }

        public async Task<List<User>> GetAll()
        {
            return await _repository.Get();
        }
    }
}