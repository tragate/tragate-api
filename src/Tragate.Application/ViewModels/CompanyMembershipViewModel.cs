using System;

namespace Tragate.Application.ViewModels
{
    public class CompanyMembershipViewModel
    {
        public int CompanyId { get; set; }
        public int MembershipPackageId { get; set; }
        public int MembershipTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CreatedUserId { get; set; }
    }
}