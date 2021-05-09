using System;
using Tragate.Domain.Core.Models;

namespace Tragate.Domain.Models
{
    public class CompanyTask : Entity
    {
        public int CompanyId { get; set; }
        public int ResponsibleUserId { get; set; }
        public string Description { get; set; }
        public byte CompanyTaskTypeId { get; set; }
        public int CreatedUserId { get; set; }
        public int? UpdatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? EndDate { get; set; }
        public byte StatusId { get; set; }
    }
}