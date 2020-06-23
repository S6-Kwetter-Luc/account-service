using System;
using System.Threading.Tasks;
using account_service.Entities;
using MessageBroker;
using Microsoft.Extensions.Options;

namespace account_service.MQ.Publisher
{
    public class AccountKwetPublisher : IAccountKwetPublisher
    {
        private readonly IMessageQueuePublisher _messageQueuePublisher;
        private readonly MessageQueueSettings _messageQueueSettings;

        public AccountKwetPublisher(IMessageQueuePublisher messageQueuePublisher, IOptions<MessageQueueSettings> messageQueueSettings)
        {
            _messageQueueSettings = messageQueueSettings.Value;
            _messageQueuePublisher = messageQueuePublisher;
        }

        public async Task PublishUpdateUser(Account updatedAccount)
        {
            await _messageQueuePublisher.PublishMessageAsync(_messageQueueSettings.Exchange, "kwet-service", "update-user", new { Id = updatedAccount.Id, NewUsername = updatedAccount.Profile.Username});
        }

        public async Task PublishDeleteUser(Guid id)
        {
            await _messageQueuePublisher.PublishMessageAsync(_messageQueueSettings.Exchange, "kwet-service", "delete-user", new { Id = id });
        }
    }
}