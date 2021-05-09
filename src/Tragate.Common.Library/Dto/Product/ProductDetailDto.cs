using System;
using System.Collections.Generic;

namespace Tragate.Common.Library.Dto
{
    public class ProductDetailDto
    {
        public int Id { get; set; }
        public Guid UuId { get; set; }
        public string ProductTitle { get; set; }
        public string CompanyTitle { get; set; }
        public string ListImagePath { get; set; }
        public string ProductSlug { get; set; }
        public string CompanySlug { get; set; }
        public int MembershipTypeId { get; set; }
        public string MembershipType { get; set; }
        public int VerificationTypeId { get; set; }
        public string VerificationType { get; set; }
        public string Location { get; set; }
        public int TransactionAmount { get; set; }
        public int TransactionCount { get; set; }
        public string ResponseRate { get; set; }
        public string ResponseTime { get; set; }
        public string EstablishmentYear { get; set; }
        public int CompanyId { get; set; }
        public string CompanyStatusId { get; set; }
        public List<ProductImageDetailDto> Images { get; set; }
        public string[] Tags { get; set; }
        public string Brand { get; set; }
        public string ModelNumber { get; set; }
        public string Description { get; set; }
        public decimal PriceLow { get; set; }
        public decimal PriceHigh { get; set; }
        public int? CurrencyId { get; set; }
        public string Currency { get; set; }
        public int? UnitTypeId { get; set; }
        public string UnitType { get; set; }
        public int? MinimumOrder { get; set; }
        public int OriginLocationId { get; set; }
        public string OriginLocation { get; set; }
        public int? SupplyAbility { get; set; }
        public string ShippingDetail { get; set; }
        public string PackagingDetail { get; set; }
        public int CategoryId { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public List<CategoryTreeDto> Category { get; set; }
    }
}