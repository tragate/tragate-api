using System.Collections.Generic;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Models;

namespace Tragate.Domain.Interfaces
{
    public interface ICompanyTaskRepository : IRepository<CompanyTask>
    {
        IEnumerable<CompanyTaskDto> GetCompanyTasks(int page, int pageSize, StatusType status, int? companyId = null,
            int? responsibleUserId = null, int? createdUserId = null);

        int CountCompanyTasks(StatusType status, int? companyId = null, int? responsibleUserId = null,
            int? createdUserId = null);
    }
}