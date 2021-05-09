using System.Collections.Generic;
using Tragate.Common.Library;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Models;

namespace Tragate.Domain.Interfaces
{
    public interface ICompanyRepository : IRepository<Company>
    {
        Company GetByUserId(int userId);

        IEnumerable<CompanyDto> GetCompaniesByStatus(int page, int pageSize, string name, StatusType status,
            int? categoryId);

        int CountCompaniesByStatus(string name, StatusType status, int? categoryId);
        CompanyDto GetCompanyDetailById(int id);
        CompanyDto GetCompanyDetailBySlug(string slug);
        IEnumerable<CompanySiteMapDto> GetCompanySiteMap();
        Company GetByOwnerUserId(int userId);
    }
}