using System.Threading.Tasks;
using AspNetCore.EventBus;
using Microsoft.Extensions.Logging;
using UserInfo.Api.Data.Entities;
using UserInfo.Api.Data.Services;
using UserInfo.Api.IntegrationEvents.Events;

namespace UserInfo.Api.IntegrationEvents.EventHandling
{
    public class UserRegisterEventHandler : IEventHandler<UserRegisterEvent>
    {
        private readonly ILogger<UserRegisterEventHandler> _logger;

        private readonly IInfoService _infoService;

        public UserRegisterEventHandler(ILogger<UserRegisterEventHandler> logger,
            IInfoService infoService)
        {
            _logger = logger;

            _infoService = infoService;
        }

        public async Task Handle(UserRegisterEvent @event)
        {
            var info = new Info
            {
                Tweets = 0,
                Following = 0,
                Followers = 0,
                Likes = 0,
                UserId = @event.UserId
            };

            await _infoService.Create(info);

            _logger.LogInformation($"Welcome {@event.UserId}!");
        }
    }
}