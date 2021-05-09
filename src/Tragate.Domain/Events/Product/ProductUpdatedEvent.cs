using Tragate.Domain.Core.Events;

namespace Tragate.Domain.Events
{
    public class ProductUpdatedEvent : Event
    {
        public int Id { get; set; }
    }
}