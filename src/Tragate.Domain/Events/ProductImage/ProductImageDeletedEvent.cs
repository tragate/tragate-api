using Tragate.Domain.Core.Events;

namespace Tragate.Domain.Events.ProductImage
{
    public class ProductImageDeletedEvent : Event
    {
        public int Id { get; set; }
    }
}