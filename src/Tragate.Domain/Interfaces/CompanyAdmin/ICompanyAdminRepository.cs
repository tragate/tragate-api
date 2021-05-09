using System.Collections.Generic;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Models;

namespace Tragate.Domain.Interfaces
{
    public interface ICompanyAdminRepository : IRepository<CompanyAdmin>
    {
        List<string> GetCompanyAdminEmailsByCompanyId(int companyId, StatusType status);

        IEnumerable<CompanyAdminUserDto> GetCompanyAdminsByCompanyId(int page, int pageSize, int companyId,
            StatusType status);

        int CountCompanyAdminsByCompanyId(int companyId, StatusType status);
        bool IsAdminOfCompany(int companyId, int loggedUserId);

        IEnumerable<CompanyAdminCompanyDto> GetCompanyAdminsByUserId(int page, int pageSize, int userId,
            StatusType status,
            string name);

        int CountCompanyAdminsByUserId(int userId, StatusType status, string name);
        CompanyDashboardDto GetCompanyDashboardById(int id);
    }
}