using System;


namespace Tragate.Common.Library.Dto
{
    public class QuoteDto : QuoteListDto
    {
        public string Description { get; set; }
        public QuoteCompanyDto BuyerCompany { get; set; }
        public decimal? ProductPrice { get; set; }
        public decimal? ShippingFee { get; set; }
        public decimal? InsuranceFee { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? Paid { get; set; }
        public decimal? Balance { get; set; }
        public int? CurrencyId { get; set; }
        public string Currency { get; set; }
        public string PaymentNote { get; set; }
        public string ShipmentNote { get; set; }
        public int? ShipmentUserAddressId { get; set; }
        public string ShipmentUserAddress { get; set; }
        public int? InvoiceUserAddressId { get; set; }
        public string InvoiceUserAddress { get; set; }
        public int? TradeTermId { get; set; }
        public string TradeTerm { get; set; }
        public int? ShippingMethodId { get; set; }
        public string ShippingMethod { get; set; }
        public DateTime? ShipmentDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
    }
}