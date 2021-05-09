using System;
using Tragate.Domain.Core.Models;

namespace Tragate.Domain.Models
{
    public class Product : Entity
    {
        public Guid UuId { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string ListImagePath { get; set; }
        public string Brand { get; set; }
        public string ModelNumber { get; set; }
        public decimal PriceLow { get; set; }
        public decimal PriceHigh { get; set; }
        public byte? CurrencyId { get; set; }
        public byte? UnitTypeId { get; set; }
        public int? MinimumOrder { get; set; }
        public int OriginLocationId { get; set; }
        public int? SupplyAbility { get; set; }
        public string ShippingDetail { get; set; }
        public string PackagingDetail { get; set; }
        public int CategoryId { get; set; }
        public int CompanyId { get; set; }
        public byte StatusId { get; set; }
        public int CreatedUserId { get; set; }
        public int? UpdatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}