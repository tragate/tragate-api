using Tragate.Domain.Core.Events;

namespace Tragate.Domain.Events {
    public class UserForgotPasswordEvent : Event {
        public int UserId { get; set; }
        public string Email { get; set; }
    }
}