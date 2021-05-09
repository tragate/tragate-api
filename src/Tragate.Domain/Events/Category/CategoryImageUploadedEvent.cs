using Tragate.Domain.Core.Events;

namespace Tragate.Domain.Events {
    public class CategoryImageUploadedEvent : Event {
        public int Id { get; set; }
        public string ImagePath { get; set; }
    }
}