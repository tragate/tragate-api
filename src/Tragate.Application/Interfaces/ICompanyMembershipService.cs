using System;
using System.Collections.Generic;
using Tragate.Application.ViewModels;
using Tragate.Common.Library.Dto.CompanyMembership;

namespace Tragate.Application
{
    public interface ICompanyMembershipService : IDisposable
    {
        IEnumerable<CompanyMembershipDto> GetCompanyMembershipsByCompanyId(int id);
        void AddCompanyMembership(CompanyMembershipViewModel model);
    }
}