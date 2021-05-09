using System;

namespace Tragate.Domain.Commands
{
    public abstract class CompanyMembershipCommand : Command
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