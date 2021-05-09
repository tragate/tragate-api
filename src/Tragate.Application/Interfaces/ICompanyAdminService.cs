using System;
using System.Collections.Generic;
using Tragate.Application.ViewModels;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;

namespace Tragate.Application
{
    public interface ICompanyAdminService : IDisposable
    {
        IEnumerable<CompanyAdminUserDto> GetCompanyAdminsByCompanyId(int page, int pageSize, int companyId,
            StatusType status);

        int CountCompanyAdminsByCompanyId(int companyId, StatusType status);

        IEnumerable<CompanyAdminCompanyDto> GetCompanyAdminsByUserId(int page, int pageSize, int userId,
            StatusType status, string name);

        int CountCompanyAdminsByUserId(int userId, StatusType status, string name);
        bool IsAdminOfCompany(int companyId, int loggedUserId);
        CompanyDashboardDto GetCompanyDashboardById(int id);
        void AddCompanyAdmin(CompanyAdminViewModel model);
        void UpdateCompanyAdmin(int id, CompanyAdminViewModel model);
        void RemoveCompanyAdmin(int id);
    }
}