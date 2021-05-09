using System;

namespace Tragate.Common.Library.Dto
{
    public class CompanyTaskDto
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string CompanyTitle { get; set; }
        public string CompanySlug { get; set; }
        public int ResponsibleUserId { get; set; }
        public string ResponsibleUser { get; set; }
        public string Description { get; set; }
        public byte CompanyTaskTypeId { get; set; }
        public string CompanyTaskType { get; set; }
        public int CreatedUserId { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? EndDate { get; set; }
        public byte StatusId { get; set; }
        public string Status { get; set; }
    }
}