using AspNetCore.EventBus;

namespace Identity.Api.IntegrationEvents.Events
{
    public class UserRegisterEvent : Event
    {
        public int UserId { get; set; }
    }
}