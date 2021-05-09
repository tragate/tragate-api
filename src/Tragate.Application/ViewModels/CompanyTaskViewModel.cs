using System;

namespace Tragate.Application.ViewModels
{
    public class CompanyTaskViewModel
    {
        public int CompanyId { get; set; }
        public int ResponsibleUserId { get; set; }
        public string Description { get; set; }
        public int CompanyTaskTypeId { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime? EndDate { get; set; }
        public int StatusId { get; set; }
    }
}