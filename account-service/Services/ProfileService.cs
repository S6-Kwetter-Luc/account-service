using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using account_service.Entities;
using account_service.Exceptions;
using account_service.Repositories;
using MongoDB.Bson;

namespace account_service.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _repository;

        public ProfileService(IProfileRepository repository)
        {
            _repository = repository;
        }

        public async Task<Profile> GetProfileByGuid(Guid id)
        {
            var profile = await _repository.GetProfileByGuid(id);
            if (profile == null) throw new AccountDoesNotExistException();
            return profile;
        }

        public async Task<bool> GetFollow(Guid id, Guid idToFollow)
        {
            var myProfile = await _repository.GetProfileByGuid(id);
            return myProfile.Following.Any(u => u.Id == idToFollow);
        }

        public async Task Follow(Guid id, Guid idToFollow)
        {
            if (id == idToFollow)
            {
                throw new AppException("Cant follow yourself");
            }

            var myProfile = await _repository.GetProfileByGuid(id);
            var profileToFollow = await _repository.GetProfileByGuid(idToFollow);

            var user = myProfile.Following.Any(u => u.Id == idToFollow);

            var userFromProfileToFollow = profileToFollow.Followers.Any(u => u.Id == id);

            if (user | userFromProfileToFollow)
            {
                throw new AlreadyFollowingException();
            }


            myProfile.Following.Add(new User
            {
                Id = idToFollow,
                Username = profileToFollow.Username
            });
            profileToFollow.Followers.Add(new User
            {
                Id = id,
                Username = myProfile.Username
            });

            await _repository.UpdateProfile(id, myProfile);
            await _repository.UpdateProfile(idToFollow, profileToFollow);
        }

        public async Task UnFollow(Guid id, Guid idToFollow)
        {
            var myProfile = await _repository.GetProfileByGuid(id);
            var profileToFollow = await _repository.GetProfileByGuid(idToFollow);

            var user = myProfile.Following.SingleOrDefault(u => u.Id == idToFollow);
            var userFromProfileToFollow = profileToFollow.Followers.SingleOrDefault(u => u.Id == id);

            if (user == null | userFromProfileToFollow == null)
            {
                throw new WasntEvenFollowingToBeginWithException();
            }

            myProfile.Following.Remove(user);
            profileToFollow.Followers.Remove(userFromProfileToFollow);

            await _repository.UpdateProfile(id, myProfile);
            await _repository.UpdateProfile(idToFollow, profileToFollow);
        }

        public async Task<List<User>> GetFollowers(Guid id)
        {
            return await _repository.GetFollowers(id);
        }

        public async Task<List<User>> GetFollowing(Guid id)
        {
            return await _repository.GetFollowing(id);
        }
    }
}