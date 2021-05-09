using System;
using Nest;
using Tragate.Domain.Core.Models;

namespace Tragate.Domain.Models
{
    public class QuoteProduct : Entity
    {
        public int QuoteId { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? TotalPrice { get; set; }
        public int? UnitTypeId { get; set; }
        public string Note { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}