using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using account_service.Entities;

namespace account_service.Repositories
{
    public interface IProfileRepository
    {
        Task<List<User>> GetFollowers(Guid id);
        Task<List<User>> GetFollowing(Guid id);
        Task<Profile> GetProfileByGuid(Guid id);
        Task UpdateProfile(Guid id, Profile profile);
    }
}