using System;
using System.Collections.Generic;
using Nest;
using Newtonsoft.Json;

namespace Tragate.Common.Library.Dto
{
    [ElasticsearchType(Name = "product")]
    public class ProductDto : Root
    {
        [Text(Ignore = true)]
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Guid UuId { get; set; }

        [Text(Ignore = true)]
        public string Description { get; set; }

        public string ListImagePath { get; set; }
        public string Brand { get; set; }
        public string ModelNumber { get; set; }
        public decimal PriceLow { get; set; }
        public decimal PriceHigh { get; set; }
        public int? CurrencyId { get; set; }
        public string Currency { get; set; }
        public int? UnitTypeId { get; set; }
        public string UnitType { get; set; }
        public int? MinimumOrder { get; set; }
        public int CategoryId { get; set; }
        public int StatusId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CompanyId { get; set; }
        public string[] Tags { get; set; }

        [JsonIgnore]
        [Text(Ignore = true)]
        public int OriginLocationId { get; set; }

        [JsonIgnore]
        [Text(Ignore = true)]
        public string OriginLocation { get; set; }

        [JsonIgnore]
        [Text(Ignore = true)]
        public int? SupplyAbility { get; set; }

        [JsonIgnore]
        [Text(Ignore = true)]
        public string ShippingDetail { get; set; }

        [JsonIgnore]
        [Text(Ignore = true)]
        public string PackagingDetail { get; set; }

        [JsonIgnore]
        [Text(Ignore = true)]
        public List<ProductImageDetailDto> Images { get; set; }

        [JsonIgnore]
        [Text(Ignore = true)]
        public string Tag { get; set; }

        [JsonIgnore]
        [Text(Ignore = true)]
        public CompanyDto Company { get; set; }

        [JsonProperty("company")]
        [Text(Ignore = true)]
        public CompanySearchDto ProductCompany { get; set; }

        [JsonIgnore]
        [Text(Ignore = true)]
        public List<CategoryTreeDto> Category { get; set; }

        [JsonIgnore]
        [Text(Ignore = true)]
        public string Status { get; set; }

        [JsonIgnore]
        [Text(Ignore = true)]
        public string CreatedUser { get; set; }

        [JsonIgnore]
        [Text(Ignore = true)]
        public string CategoryTitle { get; set; }

        /// <summary>
        /// This field using for path hierarchical aggregate category
        /// </summary>
        [JsonIgnore]
        public CategoryNodeDto CategoryTree { get; set; }
    }
}