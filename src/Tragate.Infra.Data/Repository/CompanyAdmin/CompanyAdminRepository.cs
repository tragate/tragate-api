using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using Microsoft.Extensions.Options;
using Tragate.Common.Library;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;
using Tragate.Common.Library.Helpers;
using Tragate.Domain.Interfaces;
using Tragate.Domain.Models;
using Tragate.Infra.Data.Context;
using Tragate.Infra.Data.Repository;

namespace Tragate.Infra.Data
{
    public class CompanyAdminRepository : Repository<CompanyAdmin>, ICompanyAdminRepository
    {
        private readonly IDbConnection _db;

        public CompanyAdminRepository(
            TragateContext context,
            IDbConnection db) : base(context){
            _db = db;
        }

        public List<string> GetCompanyAdminEmailsByCompanyId(int companyId, StatusType status){
            var sb = new StringBuilder();
            string sql = $@"SELECT
                            u.Email
                        FROM CompanyAdmin c
                        INNER JOIN [User] u ON u.Id = c.PersonId
                        WHERE c.CompanyId = {companyId}";

            sb.Append(sql);
            if (status != StatusType.All)
                sb.AppendLine($"AND c.StatusId = {(int) status}");

            return _db.Query<string>(sb.ToString()).ToList();
        }

        public IEnumerable<CompanyAdminUserDto> GetCompanyAdminsByCompanyId(int page, int pageSize, int companyId,
            StatusType status){
            page -= 1;
            var sb = new StringBuilder();
            string sql = $@"SELECT
                            c.Id,
                            u.Id as UserId,
                            u.FullName as UserName,
                            u.Email,
                            u.ProfileImagePath,
                            dbo.ufnGetParameterValue('CompanyAdminRoleId', c.CompanyAdminRoleId) as Role
                        FROM CompanyAdmin c
                        INNER JOIN [User] u ON u.Id = c.PersonId
                        WHERE c.CompanyId = {companyId}";

            sb.Append(sql);
            if (status != StatusType.All)
                sb.AppendLine($"AND c.StatusId = {(int) status}");
            sb.AppendLine($"order by 1 OFFSET {page * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY");

            return _db.Query<dynamic>(sb.ToString()).Select(x => new CompanyAdminUserDto()
            {
                Id = x.Id,
                UserId = x.UserId,
                UserName = x.UserName,
                Email = x.Email,
                Role = x.Role,
                ProfileImagePath = (x.ProfileImagePath as string).CheckUserProfileImage()
            }).ToList();
        }

        public int CountCompanyAdminsByCompanyId(int companyId, StatusType status){
            StringBuilder sb = new StringBuilder();
            string sql = $@"SELECT
                            Count(*)
                        FROM CompanyAdmin c
                        INNER JOIN [User] u ON u.Id = c.PersonId
                        WHERE c.CompanyId = {companyId}";

            sb.Append(sql);
            if (status != StatusType.All)
                sb.AppendLine($"AND c.StatusId = {(int) status}");
            return _db.ExecuteScalar<int>(sb.ToString());
        }

        public bool IsAdminOfCompany(int companyId, int loggedUserId){
            return Db.CompanyAdmin.Any(x =>
                x.CompanyId == companyId && x.PersonId == loggedUserId && x.StatusId == (int) StatusType.Active);
        }

        public IEnumerable<CompanyAdminCompanyDto> GetCompanyAdminsByUserId(int page, int pageSize, int userId,
            StatusType status, string name){
            page -= 1;
            StringBuilder sb = new StringBuilder();
            string sql = $@"SELECT 
                                ca.Id,
                                u.Id       AS CompanyId,
                                u.FullName AS CompanyName,
                                u.ProfileImagePath,
                                c.Slug
                            FROM [User] u
                                INNER JOIN CompanyAdmin ca ON u.Id = ca.CompanyId
                                INNER JOIN Company c on c.UserId = ca.CompanyId
                            WHERE ca.PersonId = {userId} and ca.StatusId = {(int) StatusType.Active}";

            sb.Append(sql);

            if (status != StatusType.All)
                sb.AppendLine($"AND c.StatusId = {(int) status}");

            if (!string.IsNullOrEmpty(name))
                sb.AppendLine($"and c.Slug like '%{name.GenerateSlug()}%'");

            sb.AppendLine("GROUP BY ca.Id, u.Id, u.FullName, u.ProfileImagePath, c.Slug");
            sb.AppendLine($"order by 1  desc OFFSET {page * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY");

            return _db.Query<dynamic>(sb.ToString()).Select(x => new CompanyAdminCompanyDto()
            {
                CompanyId = x.CompanyId,
                CompanyName = x.CompanyName,
                Slug = x.Slug,
                ProfileImagePath = (x.ProfileImagePath as string).CheckCompanyProfileImage()
            }).ToList();
        }

        public int CountCompanyAdminsByUserId(int userId, StatusType status, string name){
            StringBuilder sb = new StringBuilder();
            var sql = $@"SELECT count(*)
                FROM CompanyAdmin ca inner join Company c on ca.CompanyId = c.UserId
                where ca.PersonId = {userId} and ca.StatusId = {(int) StatusType.Active}";

            sb.Append(sql);
            if (status != StatusType.All)
                sb.AppendLine($"AND c.StatusId = {(int) status}");

            if (!string.IsNullOrEmpty(name))
                sb.AppendLine($"and c.Slug like '%{name.GenerateSlug()}%'");

            return _db.ExecuteScalar<int>(sb.ToString());
        }

        public CompanyDashboardDto GetCompanyDashboardById(int id){
            var productCountQuery =
                $@"select count(*) from Product where  CompanyId = {id} and  StatusId = {(int) StatusType.Active}";
            var adminCountQuery =
                $@"SELECT count(*)
                from CompanyAdmin ca inner join [User] u on ca.PersonId = u.Id
                where ca.CompanyId = {id} and ca.StatusId = {(int) StatusType.Active} and u.StatusId = {(int) StatusType.Active}";

            return new CompanyDashboardDto()
            {
                ProductCount = _db.ExecuteScalar<int>(productCountQuery),
                AdminCount = _db.ExecuteScalar<int>(adminCountQuery)
            };
        }
    }
}