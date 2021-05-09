using System;

namespace Tragate.Domain.Commands
{
    public abstract class QuoteCommand : Command
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? BuyerCompanyId { get; set; }
        public int? BuyerUserId { get; set; }
        public string BuyerUserEmail { get; set; }
        public int? BuyerUserCountryId { get; set; }
        public int? BuyerUserStateId { get; set; }
        public int SellerCompanyId { get; set; }
        public int? ProductId { get; set; }
        public string ProductNote { get; set; }
        public int? ProductQuantity { get; set; }
        public int? ProductUnitTypeId { get; set; }
        public int? SellerUserId { get; set; }
        public decimal? ProductPrice { get; set; }
        public decimal? ShippingFee { get; set; }
        public decimal? InsuranceFee { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? Paid { get; set; }
        public decimal? Balance { get; set; }
        public int? CurrencyId { get; set; } //Enum Type
        public string PaymentNote { get; set; }
        public string ShipmentNote { get; set; }
        public int? ShipmentUserAddressId { get; set; }
        public int? InvoiceUserAddressId { get; set; }
        public int? TradeTermId { get; set; } //Enum Type
        public int? ShippingMethodId { get; set; } //Enum Type
        public int QuoteStatusId { get; set; } //Enum Type
        public int OrderStatusId { get; set; } //Enum Type
        public int BuyerContactStatusId { get; set; } //Enum Type
        public int SellerContactStatusId { get; set; } //Enum Type
        public DateTime? ShipmentDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public int? CreatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}