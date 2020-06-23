using System;
using System.Threading.Tasks;
using account_service.Entities;

namespace account_service.MQ.Publisher
{
    public interface IAccountKwetPublisher
    {
        public Task PublishUpdateUser(Account updatedAccount);
        public Task PublishDeleteUser(Guid id);
    }
}