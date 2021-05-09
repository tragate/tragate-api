using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Dto.CompanyMembership;
using Tragate.Domain.Interfaces;
using Tragate.Domain.Models;
using Tragate.Infra.Data.Context;

namespace Tragate.Infra.Data.Repository
{
    public class CompanyMembershipRepository : Repository<CompanyMembership>, ICompanyMembershipRepository
    {
        private readonly IDbConnection _db;

        public CompanyMembershipRepository(TragateContext context, IDbConnection db) : base(context){
            _db = db;
        }

        public IEnumerable<CompanyMembershipDto> GetCompanyMembershipsByCompanyId(int id){
            return _db.Query<CompanyMembershipDto>($@"
                            SELECT
                              p.ParameterValue1 as MembershipPackage,
                              cm.StartDate,
                              cm.EndDate,
                              u.FullName as CreatedUser,
                              cm.CreatedDate
                            FROM CompanyMembership cm
                              INNER JOIN Parameter p ON p.ParameterCode = cm.MembershipPackageId AND p.ParameterType = 'MembershipPackageId'
                              INNER JOIN [User] u ON u.Id = cm.CreatedUserId
                            WHERE cm.CompanyId = {id}");
        }

        public CompanyMembershipDetailDto GetCompanyMembershipDetailByCompanyId(int id){
            return _db.Query<CompanyMembershipDetailDto>($@"SELECT
                                      c.UserId          AS CompanyId,
                                      u.FullName        AS UserName,
                                      u.Email           AS UserEmail,
                                      co.FullName       AS CompanyName,
                                      p.ParameterValue1 AS MembershipPackage,
                                      cm.StartDate,
                                      cm.EndDate,
                                      cm.MembershipPackageId AS MembershipPackageType,
                                      c.Slug
                                    FROM CompanyMembership cm
                                      INNER JOIN Company c ON c.UserId = cm.CompanyId
                                      INNER JOIN [User] u ON u.Id = c.OwnerUserId
                                      INNER JOIN [User] co ON co.Id = c.UserId
                                      INNER JOIN Parameter p ON p.ParameterCode = cm.MembershipPackageId AND p.ParameterType = 'MembershipPackageId'
                                    WHERE cm.CompanyId = {id}").Single();
        }

        public bool IsExistsActiveMembershipById(int companyId, DateTime startDate){
            var count = _db.ExecuteScalar<int>(
                $"select count(*) from CompanyMembership where EndDate >='{startDate}' and CompanyId={companyId}");
            return count > 0;
        }
    }
}