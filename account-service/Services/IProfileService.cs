using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using account_service.Entities;

namespace account_service.Services
{
    public interface IProfileService
    {
        Task<Profile> GetProfileByGuid(Guid id);
        Task<bool> GetFollow(Guid id, Guid idToFollow);
        Task Follow(Guid id, Guid idToFollow, string jwt);
        Task UnFollow(Guid id, Guid idToFollow, string jwt);
        Task<List<User>> GetFollowers(Guid id);
        Task<List<User>> GetFollowing(Guid id);
    }
}