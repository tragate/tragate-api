using System;
using Tragate.Common.Library.Enum;

namespace Tragate.Domain.Commands
{
    public abstract class CompanyTaskCommand : Command
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int ResponsibleUserId { get; set; }
        public string Description { get; set; }
        public CompanyTaskType CompanyTaskTypeId { get; set; }
        public int CreatedUserId { get; set; }
        public int? UpdatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? EndDate { get; set; }
        public StatusType StatusId { get; set; }
    }
}