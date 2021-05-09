using System;

namespace Tragate.Common.Library.Dto
{
    public class QuoteListDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public QuoteUserDto BuyerUser { get; set; }
        public QuoteUserDto SellerUser { get; set; }
        public QuoteCompanyDto SellerCompany { get; set; }
        public int QuoteStatusId { get; set; }
        public int OrderStatusId { get; set; }
        public string OrderStatus { get; set; }
        public int SellerContactStatusId { get; set; }
        public string SellerContactStatus { get; set; }
        public int BuyerContactStatusId { get; set; }
        public string BuyerContactStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}