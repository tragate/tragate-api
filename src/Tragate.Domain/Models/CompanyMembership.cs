using System;
using Tragate.Domain.Core.Models;

namespace Tragate.Domain.Models
{
    public class CompanyMembership : Entity
    {
        public int CompanyId { get; set; }
        public int MembershipPackageId { get; set; }
        public int MembershipTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}