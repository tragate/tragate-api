using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Interfaces;
using Tragate.Domain.Models;
using Tragate.Infra.Data.Context;

namespace Tragate.Infra.Data.Repository
{
    public class CompanyTaskRepository : Repository<CompanyTask>, ICompanyTaskRepository
    {
        private readonly IDbConnection _db;

        public CompanyTaskRepository(TragateContext context, IDbConnection db) : base(context){
            _db = db;
        }

        public IEnumerable<CompanyTaskDto> GetCompanyTasks(int page, int pageSize, StatusType status,
            int? companyId = null, int? responsibleUserId = null, int? createdUserId = null){
            page -= 1;
            var sb = new StringBuilder();
            sb.AppendLine(@"SELECT
                          ct.Id,
                          ct.CompanyId,
                          u.FullName          AS CompanyTitle,
                          c.Slug              AS CompanySlug,
                          ct.ResponsibleUserId,
                          rs.FullName         AS ResponsibleUser,
                          ct.Description,
                          ct.CompanyTaskTypeId,
                          ctt.ParameterValue1 AS CompanyTaskType,
                          ct.CreatedUserId,
                          cu.FullName         AS CreatedUser,
                          ct.CreatedDate,
                          ct.EndDate,
                          ct.StatusId,
                          st.ParameterValue1  AS Status
                        FROM CompanyTask ct
                          INNER JOIN [User] u ON u.Id = ct.CompanyId
                          INNER JOIN [User] rs ON rs.Id = ct.ResponsibleUserId
                          INNER JOIN [User] cu ON cu.Id = ct.CreatedUserId
                          INNER JOIN Company c ON ct.CompanyId = c.UserId
                          INNER JOIN Parameter ctt ON ctt.ParameterCode = ct.CompanyTaskTypeId AND ctt.ParameterType = 'CompanyTaskTypeId'
                          INNER JOIN Parameter st ON st.ParameterCode = ct.StatusId AND st.ParameterType = 'StatusId' WHERE 1=1");
            if (companyId.HasValue)
                sb.AppendLine($"and ct.CompanyId={companyId}");

            if (responsibleUserId.HasValue)
                sb.AppendLine($"and ct.ResponsibleUserId={responsibleUserId}");

            if (createdUserId.HasValue)
                sb.AppendLine($"and ct.CreatedUserId={createdUserId}");

            if (status != StatusType.All)
                sb.AppendLine($"and ct.StatusId ={(int) status}");

            sb.AppendLine($"ORDER BY 1 OFFSET {page * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY");
            return _db.Query<CompanyTaskDto>(sb.ToString());
        }

        public int CountCompanyTasks(StatusType status, int? companyId = null, int? responsibleUserId = null,
            int? createdUserId = null){
            var sb = new StringBuilder();
            sb.AppendLine("SELECT count(*) FROM CompanyTask ct where 1=1");

            if (companyId.HasValue)
                sb.AppendLine($"and ct.CompanyId={companyId}");

            if (responsibleUserId.HasValue)
                sb.AppendLine($"and ct.ResponsibleUserId={responsibleUserId}");

            if (createdUserId.HasValue)
                sb.AppendLine($"and ct.CreatedUserId={createdUserId}");

            if (status != StatusType.All)
                sb.AppendLine($"and ct.StatusId ={(int) status}");

            return _db.ExecuteScalar<int>(sb.ToString());
        }
    }
}