using System;

namespace Tragate.Common.Library
{
    public class CategoryProductDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string ListImagePath { get; set; }
        public string Brand { get; set; }
        public decimal PriceLow { get; set; }
        public int? CurrencyId { get; set; }
        public string Currency { get; set; }
        public int CategoryId { get; set; }
        public string CategoryTitle { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}