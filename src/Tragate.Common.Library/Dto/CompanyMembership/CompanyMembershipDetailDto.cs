using System;
using Tragate.Common.Library.Enum;

namespace Tragate.Common.Library.Dto
{
    public class CompanyMembershipDetailDto
    {
        public int CompanyId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string CompanyName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Slug { get; set; }
        public string MembershipPackage { get; set; }
        public MembershipPackageType MembershipPackageType { get; set; }
    }
}