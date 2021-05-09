using System;
using Tragate.Domain.Core.Models;

namespace Tragate.Domain.Models
{
    public class CompanyNote : Entity
    {
        public int CompanyId { get; set; }
        public string Description { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int StatusId { get; set; }
    }
}