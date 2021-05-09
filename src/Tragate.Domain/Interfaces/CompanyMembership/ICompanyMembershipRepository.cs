using System;
using System.Collections.Generic;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Dto.CompanyMembership;
using Tragate.Domain.Models;

namespace Tragate.Domain.Interfaces
{
    public interface ICompanyMembershipRepository : IRepository<CompanyMembership>
    {
        IEnumerable<CompanyMembershipDto> GetCompanyMembershipsByCompanyId(int id);
        CompanyMembershipDetailDto GetCompanyMembershipDetailByCompanyId(int id);
        bool IsExistsActiveMembershipById(int companyId, DateTime startDate);
    }
}