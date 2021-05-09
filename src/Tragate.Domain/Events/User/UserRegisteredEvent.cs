using Tragate.Domain.Core.Events;

namespace Tragate.Domain.Events {
    public class UserRegisteredEvent : Event {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}