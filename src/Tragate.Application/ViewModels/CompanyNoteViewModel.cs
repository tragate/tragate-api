namespace Tragate.Application.ViewModels
{
    public class CompanyNoteViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Description { get; set; }
        public int CreatedUserId { get; set; }
    }
}