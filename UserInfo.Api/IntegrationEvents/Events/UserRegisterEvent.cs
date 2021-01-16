using AspNetCore.EventBus;

namespace UserInfo.Api.IntegrationEvents.Events
{
    public class UserRegisterEvent : Event
    {
        public int UserId { get; set; }
    }
}