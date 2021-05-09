using System;
using System.Collections.Generic;
using Tragate.Application.ViewModels;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;

namespace Tragate.Application
{
    public interface ICompanyTaskService : IDisposable
    {
        IEnumerable<CompanyTaskDto> GetCompanyTasks(int page, int pageSize, StatusType status, int? companyId,
            int? responsibleUserId, int? createdUserId);

        int CountCompanyTasks(StatusType status, int? companyId, int? responsibleUserId, int? createdUserId);
        IEnumerable<CompanyTaskDto> GetCompanyTasksByCompanyId(int id, int page, int pageSize, StatusType status);
        int CountCompanyTasksByCompanyId(int id, StatusType status);
        IEnumerable<UserTaskDto> GetCompanyTasksByUserId(int id, int page, int pageSize, StatusType status);
        int CountCompanyTasksByUserId(int id, StatusType status);
        void AddCompanyTask(CompanyTaskViewModel model);
        void UpdateStatusCompanyTask(int id, CompanyTaskStatusViewModel model);
        void DeleteCompanyTask(int id);
    }
}