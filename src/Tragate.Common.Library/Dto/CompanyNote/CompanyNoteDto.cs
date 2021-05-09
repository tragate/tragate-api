using System;

namespace Tragate.Common.Library.Dto
{
    public class CompanyNoteDto
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string CompanyTitle { get; set; }
        public int CreatedUserId { get; set; }
        public string CreatedUser { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public int StatusId { get; set; }
    }
}