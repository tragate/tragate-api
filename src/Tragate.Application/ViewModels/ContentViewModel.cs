namespace Tragate.Application.ViewModels
{
    public class ContentViewModel {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public int? ContentTypeId { get; set; }
        public int StatusId { get; set; }
        public int CreatedUserId { get; set; }
    }
}