using System;

namespace Tragate.Common.Library.Dto
{
    public class QuoteProductDto
    {
        public int Id { get; set; }
        public int QuoteId { get; set; }
        public int ProductId { get; set; }
        public string Product { get; set; }
        public string ListImagePath { get; set; }
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public int? UnitTypeId { get; set; }
        public string UnitType { get; set; }
        public decimal? TotalPrice { get; set; }
        public string Note { get; set; }
        public int CreatedUserId { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}