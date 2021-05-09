using System.Collections.Generic;
using System.Linq;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Models;

namespace Tragate.Domain.Interfaces
{
    public interface ICompanyDataRepository : IRepository<CompanyData>
    {
        int CountByCompaniesDataByStatus(string name, StatusType status, int? referenceUserId);

        IEnumerable<CompanyData> GetCompaniesDataByStatus(int page, int pageSize, string name, StatusType status,
            int? referenceUserId);
    }
}