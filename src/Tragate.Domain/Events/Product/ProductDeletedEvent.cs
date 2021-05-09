using Tragate.Domain.Core.Events;

namespace Tragate.Domain.Events
{
    public class ProductDeletedEvent : Event
    {
        public int Id { get; set; }
    }
}