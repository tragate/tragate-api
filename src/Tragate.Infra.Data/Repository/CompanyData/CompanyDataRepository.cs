using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using Tragate.Common.Library.Enum;
using Tragate.Common.Library.Helpers;
using Tragate.Domain.Interfaces;
using Tragate.Domain.Models;
using Tragate.Infra.Data.Context;
using Tragate.Infra.Data.Repository;

namespace Tragate.Infra.Data
{
    public class CompanyDataRepository : Repository<CompanyData>, ICompanyDataRepository
    {
        private readonly IDbConnection _db;

        public CompanyDataRepository(TragateContext context, IDbConnection db) : base(context){
            _db = db;
        }

        public int CountByCompaniesDataByStatus(string name, StatusType status, int? companyId){
            name = name?.ToLower(new System.Globalization.CultureInfo("en-US"));
            var sb = new StringBuilder();
            sb.Append($"SELECT Count(*) FROM CompanyData where 1=1");
            if (StatusType.All != status)
                sb.Append($"and StatusId ={(byte) status}");

            if (companyId.HasValue)
                sb.Append($"and CompanyId ={companyId}");

            if (!string.IsNullOrEmpty(name))
                sb.AppendLine(
                    $"and (TitleLatinized like '%{name.GenerateSlug()}%' or City like '%{name}%' or Membership like '%{name}%')");
            return _db.ExecuteScalar<int>(sb.ToString());
        }

        public IEnumerable<CompanyData>
            GetCompaniesDataByStatus(int page, int pageSize, string name, StatusType status, int? companyId){
            page -= 1;
            var sb = new StringBuilder();
            sb.Append($"SELECT * FROM CompanyData where 1=1");
            if (StatusType.All != status)
                sb.Append($"and StatusId ={(byte) status}");

            if (companyId.HasValue)
                sb.Append($"and CompanyId ={companyId}");

            if (!string.IsNullOrEmpty(name))
                sb.AppendLine(
                    $"and (TitleLatinized like '%{name.GenerateSlug()}%' or City like '%{name}%' or Membership like '%{name}%')");

            sb.AppendLine($"order by 1 OFFSET {page * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY");
            return _db.Query<CompanyData>(sb.ToString()).ToList();
        }
    }
}