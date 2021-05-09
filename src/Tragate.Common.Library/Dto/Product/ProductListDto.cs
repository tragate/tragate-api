using System;

namespace Tragate.Common.Library.Dto
{
    /// <summary>
    /// Product dto has been used elasticsearch therefore it's name changed as ProductListDto
    /// </summary>
    public class ProductListDto
    {
        public int Id { get; set; }
        public string ProductTitle { get; set; }
        public string ListImagePath { get; set; }
        public string CompanyTitle { get; set; }
        public string ProductSlug { get; set; }
        public string CompanySlug { get; set; }
        public string Currency { get; set; }
        public string UnitType { get; set; }
        public decimal PriceLow { get; set; }
        public decimal PriceHigh { get; set; }
        public string MembershipType { get; set; }
        public int MinimumOrder { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public string CategoryTitle { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}