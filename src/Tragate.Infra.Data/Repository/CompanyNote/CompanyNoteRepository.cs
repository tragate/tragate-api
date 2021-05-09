using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Interfaces;
using Tragate.Domain.Models;
using Tragate.Infra.Data.Context;
using Tragate.Infra.Data.Repository;

namespace Tragate.Infra.Data
{
    public class CompanyNoteRepository : Repository<CompanyNote>, ICompanyNoteRepository
    {
        private readonly IDbConnection _db;

        public CompanyNoteRepository(TragateContext context, IDbConnection db) : base(context){
            _db = db;
        }

        public IEnumerable<CompanyNoteDto> GetCompanyNotes(int page, int pageSize, int? status, int?
            companyId = null, int? userId = null){
            page -= 1;
            var sb = new StringBuilder();
            sb.AppendLine(@"select  cn.Id,
                                    cn.CompanyId,
                                    cn.Description,
                                    cn.CreatedUserId,
                                    cn.CreatedDate,
                                    cn.StatusId,
                                    u.FullName as CreatedUser,
                                    c.FullName as CompanyTitle
            from CompanyNote cn
            INNER JOIN [User] u ON u.Id = cn.CreatedUserId
            INNER JOIN [User] c ON c.Id = cn.CompanyId  where 1=1");

            if (companyId.HasValue)
                sb.AppendLine($"and cn.CompanyId={companyId}");

            if (userId.HasValue)
                sb.AppendLine($"and cn.CreatedUserId={userId}");

            if (status.HasValue)
                sb.AppendLine($"and cn.StatusId={status}");

            sb.AppendLine($"order by 1 OFFSET {page * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY");
            return _db.Query<CompanyNoteDto>(sb.ToString())
                .AsEnumerable();
        }

        public int CountCompanyNotes(int? status, int? companyId = null, int? userId = null){
            var sb = new StringBuilder();
            sb.AppendLine(@"select count(*) from CompanyNote cn  where 1=1");

            if (companyId.HasValue)
                sb.AppendLine($"and cn.CompanyId={companyId}");

            if (userId.HasValue)
                sb.AppendLine($"and cn.CreatedUserId={userId}");

            if (status.HasValue)
                sb.AppendLine($"and cn.StatusId={status}");

            return _db.ExecuteScalar<int>(sb.ToString());
        }
    }
}