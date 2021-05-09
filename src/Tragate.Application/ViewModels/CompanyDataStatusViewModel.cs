namespace Tragate.Application.ViewModels
{
    public class CompanyDataStatusViewModel
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public int? CompanyId { get; set; }
        public int UpdatedUserId { get; set; }
    }
}