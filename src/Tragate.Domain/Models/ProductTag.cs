using Tragate.Domain.Core.Models;

namespace Tragate.Domain.Models
{
    public class ProductTag:Entity
    {
        public int ProductId { get; set; }
        public int TagId { get; set; }
    }
}