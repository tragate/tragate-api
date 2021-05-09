using System;
using Newtonsoft.Json;

namespace Tragate.Application.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [JsonIgnore]
        public Guid UuId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public string ModelNumber { get; set; }
        public decimal PriceLow { get; set; }
        public decimal PriceHigh { get; set; }
        public int? CurrencyId { get; set; }
        public int? UnitTypeId { get; set; }
        public int? MinimumOrder { get; set; }
        public int OriginLocationId { get; set; }
        public int? SupplyAbility { get; set; }
        public string ShippingDetail { get; set; }
        public string PackagingDetail { get; set; }
        public int CategoryId { get; set; }
        public int CompanyId { get; set; }
        public int StatusId { get; set; }
        public int CreatedUserId { get; set; }
        public int? UpdatedUserId { get; set; }
        public int[] TagIds { get; set; }
    }
}