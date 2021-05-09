using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
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
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly IDbConnection _db;


        public CompanyRepository(
            TragateContext context,
            IDbConnection db) : base(context){
            _db = db;
        }


        public Company GetByUserId(int userId){
            return Db.Company.FirstOrDefault(x => x.UserId == userId);
        }

        public Company GetByOwnerUserId(int userId)
        {
            return Db.Company.FirstOrDefault(x => x.OwnerUserId == userId);
        }


        public int CountCompaniesByStatus(string name,
            StatusType status, int? categoryGroupId){
            var sb = new StringBuilder();
            sb.Append($@"SELECT Count(*) 
                    FROM [User] u 
                        INNER JOIN Company c on u.Id = c.UserId 
                     WHERE u.UserTypeId = {(byte) UserType.Company} ");

            if (status != StatusType.All)
                sb.AppendLine($"and c.StatusId ={(byte) status}");

            if (!string.IsNullOrEmpty(name))
                sb.AppendLine($"and c.Slug like '%{name.GenerateSlug()}%'");

            if (categoryGroupId.HasValue){
                sb.AppendLine($@"and EXISTS(SELECT *
                 FROM CompanyCategory cc
                   INNER JOIN CategoryGroup cg ON cc.CategoryId = cg.CategoryId
                   INNER JOIN Parameter p
                     ON p.ParameterType = 'CategoryGroupId' AND cg.CategoryGroupId = p.ParameterCode
                 WHERE cc.CompanyId = c.UserId AND cg.CategoryGroupId = {categoryGroupId})");
            }

            return _db.ExecuteScalar<int>(sb.ToString());
        }

        public IEnumerable<CompanyDto> GetCompaniesByStatus(int page, int pageSize, string name,
            StatusType status, int? categoryGroupId){
            page -= 1;
            var sb = new StringBuilder();

            sb.Append($@"SELECT c.*,
                                u.*,
                                p.Id,
                                p.ParameterValue1  as Value
                      FROM [User] u  
                        INNER JOIN Company c on u.Id = c.UserId 
                        INNER JOIN Parameter p on p.ParameterType = 'StatusId' and p.ParameterCode = c.StatusId
                WHERE u.UserTypeId = {(byte) UserType.Company} ");

            if (status != StatusType.All)
                sb.AppendLine($"and c.StatusId ={(byte) status}");

            if (!string.IsNullOrEmpty(name))
                sb.AppendLine($"and c.Slug like '%{name.GenerateSlug()}%'");

            if (categoryGroupId.HasValue){
                sb.AppendLine($@"and EXISTS(SELECT *
                 FROM CompanyCategory cc
                   INNER JOIN CategoryGroup cg ON cc.CategoryId = cg.CategoryId
                   INNER JOIN Parameter p
                     ON p.ParameterType = 'CategoryGroupId' AND cg.CategoryGroupId = p.ParameterCode
                 WHERE cc.CompanyId = c.UserId AND cg.CategoryGroupId = {categoryGroupId})");
            }

            sb.AppendLine($"order by 1 desc OFFSET {page * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY");
            var result = _db
                .Query<CompanyDto, UserDto, ParameterDto, CompanyDto>(
                    sb.ToString(), (c, u, p) =>
                    {
                        c.User = u;
                        c.Status = p.Value;
                        c.ProductCount = _db.Query<int>($@"SELECT count(p.Id) FROM Product p 
                                    WHERE p.CompanyId = {c.UserId} AND 
                                    p.StatusId = {(int) StatusType.Active}").First();
                        return c;
                    }).ToList();


            return result;
        }

        /// <summary>
        /// Not : cd.StatusId alanının sadece transfer edilmiş bir tane kayda sahip olması gereklidir.
        /// Eger duplicate edilmiş transfer edilen data olursa company'e ait birden fazla firma var demektir ama ilk olanı gelir ki
        /// karşılıklığa sebebiyet verebilir. İleri de düzeltilecek şimdi acelesi yok
        /// </summary>
        /// <param name="slug"></param>
        /// <returns></returns>
        public CompanyDto GetCompanyDetailBySlug(string slug){
            var query = $@"SELECT
                                  c.*,
                                  u.*,
                                  u.RegisterTypeId   as RegisterType,
                                  u.UserTypeId       as UserType,
                                  u.StatusId         as StatusType,
                                  u2.Id,
                                  u2.FullName,
                                  p2.Id,
                                  p2.ParameterValue1 as Value,
                                  cd.*,
                                  l.*,
                                  p.Id,
                                  p.ParameterValue1  as Value
                                FROM [User] u
                                  INNER JOIN Company c ON c.UserId = u.Id
                                  INNER JOIN [User] u2 ON u2.Id = c.OwnerUserId
                                  INNER JOIN Location l ON l.Id = u.LocationId
                                  INNER JOIN Location l2 ON l2.Id = u.CountryId
                                  INNER JOIN Location l3 ON l3.Id = u.StateId
                                  INNER JOIN Parameter p on p.ParameterType = 'StatusId' and p.ParameterCode = c.StatusId
                                  INNER JOIN Parameter p2 on p2.ParameterType = 'StatusId' and p2.ParameterCode = u.StatusId
                                  LEFT JOIN CompanyData cd ON cd.CompanyId = c.UserId and cd.StatusId = {(int) StatusType.Transferred}
                                WHERE c.Slug = '{slug}'";

            var result = _db
                .Query<CompanyDto, UserDto, UserDto, ParameterDto, CompanyDataDto, LocationDto, ParameterDto,
                    CompanyDto>(
                    query, (c, u, u2, p2, cd, l, p) =>
                    {
                        c.Id = u.Id;
                        c.OwnerUser = u2.FullName;
                        c.User = u;
                        c.User.Location = l;
                        c.User.Status = p2.Value;
                        c.CompanyData = cd;
                        c.Status = p.Value;
                        c.User.ProfileImagePath = c.User.ProfileImagePath.CheckCompanyProfileImage();
                        c.ProductCount = _db.Query<int>($@"SELECT count(p.Id) FROM Product p 
                                    WHERE p.CompanyId = {c.UserId} AND 
                                    p.StatusId = {(int) StatusType.Active}").First();

                        return c;
                    },
                    splitOn: "Id").FirstOrDefault();

            if (result != null){
                result.CategoryList = _db.Query<CategoryDto>($@"SELECT
                                      c.Id,
                                      c.Title,
                                      c.Slug
                                    FROM CompanyCategory cc
                                      INNER JOIN Category c ON c.Id = cc.CategoryId
                                    WHERE cc.CompanyId = {result.UserId}").ToList();

                result.CategoryTags = result.CategoryList.Select(c => c.Slug).ToArray();

                var parameter = _db.Query<dynamic>($@"
                            SELECT
                                    dbo.ufnGetParameterValue('BusinessTypeId', c.BusinessType)             AS BusinessTypes,
                                    dbo.ufnGetParameterValue('EmployeeCountId', c.EmployeeCountId)         AS EmployeeCount,
                                    dbo.ufnGetParameterValue('AnnualRevenueId', c.AnnualRevenueId)         AS AnnualRevenue,
                                    dbo.ufnGetParameterValue('MembershipTypeId', c.MembershipTypeId)       AS MembershipType,
                                    dbo.ufnGetParameterValue('MembershipPackageId', c.MembershipPackageId) AS MembershipPackage,
                                    dbo.ufnGetParameterValue('VerificationTypeId', c.VerificationTypeId)   AS VerificationType
                            FROM Company c
                            WHERE c.UserId = {result.UserId}", commandType: CommandType.Text).First();

                result.BusinessTypes = parameter.BusinessTypes;
                result.EmployeeCount = parameter.EmployeeCount;
                result.AnnualRevenue = parameter.AnnualRevenue;
                result.MembershipType = parameter.MembershipType;
                result.MembershipPackage = parameter.MembershipPackage;
                result.VerificationType = parameter.VerificationType;
            }

            return result;
        }

        public IEnumerable<CompanySiteMapDto> GetCompanySiteMap(){
            return _db.Query<CompanySiteMapDto>(
                $"select  UserId,Slug,UpdatedDate from Company where StatusId = {(int)StatusType.Active}");
        }

        /// <summary>
        /// Not : cd.StatusId alanının sadece transfer edilmiş bir tane kayda sahip olması gereklidir.
        /// Eger duplicate edilmiş transfer edilen data olursa company'e ait birden fazla firma var ama ilk olanı gelir ki
        /// karşılıklığa sebebiyet verebilir. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CompanyDto GetCompanyDetailById(int id){
            var query = $@"SELECT
                                  c.*,
                                  u.*,
                                  u.RegisterTypeId   as RegisterType,
                                  u.UserTypeId       as UserType,
                                  u.StatusId         as StatusType,
                                  u2.Id,
                                  u2.FullName,
                                  p2.Id,
                                  p2.ParameterValue1 as Value,
                                  cd.*,
                                  l.*,
                                  p.Id,
                                  p.ParameterValue1  as Value
                                FROM [User] u
                                  INNER JOIN Company c ON c.UserId = u.Id
                                  INNER JOIN [User] u2 ON u2.Id = c.OwnerUserId
                                  INNER JOIN Location l ON l.Id = u.LocationId
                                  INNER JOIN Location l2 ON l2.Id = u.CountryId
                                  INNER JOIN Location l3 ON l3.Id = u.StateId
                                  INNER JOIN Parameter p on p.ParameterType = 'StatusId' and p.ParameterCode = c.StatusId
                                  INNER JOIN Parameter p2 on p2.ParameterType = 'StatusId' and p2.ParameterCode = u.StatusId
                                  LEFT JOIN CompanyData cd ON cd.CompanyId = c.UserId and cd.StatusId = {(int) StatusType.Transferred}
                                WHERE u.Id = {id}";

            var result = _db
                .Query<CompanyDto, UserDto, UserDto, ParameterDto, CompanyDataDto, LocationDto, ParameterDto,
                    CompanyDto>(
                    query, (c, u, u2, p2, cd, l, p) =>
                    {
                        c.Id = u.Id;
                        c.OwnerUser = u2.FullName;
                        c.User = u;
                        c.Title = u.FullName;
                        c.User.Location = l;
                        c.User.Status = p2.Value;
                        c.CompanyData = cd;
                        c.Status = p.Value;
                        c.User.ProfileImagePath = c.User.ProfileImagePath.CheckCompanyProfileImage();
                        c.ProductCount = _db.Query<int>($@"SELECT count(p.Id) FROM Product p 
                                    WHERE p.CompanyId = {c.UserId} AND 
                                    p.StatusId = {(int) StatusType.Active}").First();

                        return c;
                    },
                    splitOn: "Id").FirstOrDefault();

            if (result != null){
                result.CategoryList = _db.Query<CategoryDto>($@"SELECT
                                      c.Id,
                                      c.Title,
                                      c.Slug
                                    FROM CompanyCategory cc
                                      INNER JOIN Category c ON c.Id = cc.CategoryId
                                    WHERE cc.CompanyId = {result.UserId}").ToList();

                result.CategoryTags = result.CategoryList.Select(c => c.Slug).ToArray();
                result.CategoryText = string.Join("/", result.CategoryTags);

                var parameter = _db.Query<dynamic>($@"
                            SELECT
                                    dbo.ufnGetParameterValue('BusinessTypeId', c.BusinessType)             AS BusinessTypes,
                                    dbo.ufnGetParameterValue('EmployeeCountId', c.EmployeeCountId)         AS EmployeeCount,
                                    dbo.ufnGetParameterValue('AnnualRevenueId', c.AnnualRevenueId)         AS AnnualRevenue,
                                    dbo.ufnGetParameterValue('MembershipTypeId', c.MembershipTypeId)       AS MembershipType,
                                    dbo.ufnGetParameterValue('MembershipPackageId', c.MembershipPackageId) AS MembershipPackage,
                                    dbo.ufnGetParameterValue('VerificationTypeId', c.VerificationTypeId)   AS VerificationType
                            FROM Company c
                            WHERE c.UserId = {result.UserId}", commandType: CommandType.Text).First();

                result.BusinessTypes = parameter.BusinessTypes;
                result.EmployeeCount = parameter.EmployeeCount;
                result.AnnualRevenue = parameter.AnnualRevenue;
                result.MembershipType = parameter.MembershipType;
                result.MembershipPackage = parameter.MembershipPackage;
                result.VerificationType = parameter.VerificationType;
            }

            return result;
        }
    }
}