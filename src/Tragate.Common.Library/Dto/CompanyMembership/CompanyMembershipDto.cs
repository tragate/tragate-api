using System;

namespace Tragate.Common.Library.Dto.CompanyMembership
{
    public class CompanyMembershipDto
    {
        public string MembershipPackage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}