namespace Tragate.Application.ViewModels
{
    public class CreateQuoteViewModel
    {
        public string Title { get; set; }
        public string UserMessage { get; set; }
        public int? BuyerCompanyId { get; set; }
        public int? BuyerUserId { get; set; }
        public string BuyerUserEmail { get; set; }
        public int? BuyerUserCountryId { get; set; }
        public int? BuyerUserStateId { get; set; }
        public int SellerCompanyId { get; set; }
        public int? SellerUserId { get; set; }
        public int? ProductId { get; set; }
        public string ProductNote { get; set; }
        public int? ProductQuantity { get; set; }
        public int? ProductUnitTypeId { get; set; }
        public int? CreatedUserId { get; set; }
    }
}