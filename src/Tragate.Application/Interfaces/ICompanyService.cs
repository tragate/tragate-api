using System;
using System.Collections.Generic;
using System.Linq;
using Tragate.Application.ViewModels;
using Tragate.Common.Library;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;

namespace Tragate.Application
{
    public interface ICompanyService : IDisposable
    {
        CompanyDto GetCompanyDetailById(int id);

        CompanyDto GetCompanyDetailBySlug(string slug);

        IEnumerable<CompanyDto> GetCompaniesByStatus(int page, int pageSize, string name, StatusType status,
            int? categoryGroupId);
        
        IEnumerable<CompanySiteMapDto> GetCompanySiteMap();

        int CountCompaniesByStatus(string name, StatusType status, int? categoryGroupId);

        void AddCompany(CompanyViewModel model);

        void UpdateCompany(int id, CompanyViewModel model);

        void RemoveCompany(int id);
        void UpdateOwnerAndContactUser(int id, int ownerUserId, int contactUserId);
        void AddCompanyFast(CompanyFastAddViewModel model);
    }
}