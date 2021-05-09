using Tragate.Domain.Core.Events;

namespace Tragate.Domain.Events {
    public class UserImageUploadedEvent : Event {
        public int UserId { get; set; }
        public string ProfileImagePath { get; set; }
    }
}