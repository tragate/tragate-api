using System;
using Tragate.Common.Library.Enum;

namespace Tragate.Common.Library.Dto
{
    public class SearchAllDto
    {
        /*Company*/
        public int Id { get; set; }
        public string Title { get; set; }
        public string EstablishmentYear { get; set; }
        public string ResponseRate { get; set; }
        public string ResponseTime { get; set; }
        public int? TransactionAmount { get; set; }
        public int? TransactionCount { get; set; }
        public int? MembershipTypeId { get; set; }
        public int? VerificationTypeId { get; set; }
        public byte StatusId { get; set; }
        public UserSearchDto User { get; set; }
        public string MembershipType { get; set; }
        public string VerificationType { get; set; }
        public string Slug { get; set; }
        public string[] CategoryTags { get; set; }

        /*Product*/
        public int ProductId { get; set; }
        public Guid UuId { get; set; }
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
        public DateTime CreatedDate { get; set; }
        public int CompanyId { get; set; }
        public string[] Tags { get; set; }

        public DocumentType DocumentType => this.User != null
            ? DocumentType.Company
            : DocumentType.Product;
    }
}