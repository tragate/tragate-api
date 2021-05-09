using System;
using Tragate.Domain.Core.Models;

namespace Tragate.Domain.Models
{
    public class Quote : Entity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int? BuyerCompanyId { get; set; }
        public int? BuyerUserId { get; set; }
        public int SellerCompanyId { get; set; }
        public int? SellerUserId { get; set; }
        public decimal? ProductPrice { get; set; }
        public decimal? ShippingFee { get; set; }
        public decimal? InsuranceFee { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? Paid { get; set; }
        public decimal? Balance { get; set; }
        public int? CurrencyId { get; set; }
        public string PaymentNote { get; set; }
        public string ShipmentNote { get; set; }
        public int? ShipmentUserAddressId { get; set; }
        public int? InvoiceUserAddressId { get; set; }
        public int? TradeTermId { get; set; }
        public int? ShippingMethodId { get; set; }
        public int QuoteStatusId { get; set; }
        public int OrderStatusId { get; set; }
        public int BuyerContactStatusId { get; set; }
        public int SellerContactStatusId { get; set; }
        public DateTime? ShipmentDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}