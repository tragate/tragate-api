using System;
using System.Collections.Generic;
using System.Linq;
using Tragate.Application.ViewModels;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;

namespace Tragate.Application
{
    public interface ICompanyDataService : IDisposable
    {
        CompanyDataDto GetCompanyDataById(int id);

        IEnumerable<CompanyDataDto> GetCompaniesDataByStatus(int page, int pageSize, string name, StatusType status,
            int? companyId);

        int CountByCompaniesDataByStatus(StatusType status, string name, int? companyId);

        void UpdateStatusOfCompanyData(CompanyDataStatusViewModel model);

        void UpdateCompanyData(CompanyDataViewModel model);
    }
}