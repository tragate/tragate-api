using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using AutoMapper;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;
using Tragate.Common.Library.Helpers;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Core.Notifications;
using Tragate.Domain.Interfaces;
using Tragate.Domain.Models;
using Tragate.Infra.Data.Context;
using Tragate.Infra.Data.Repository;

namespace Tragate.Infra.Data
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _bus;
        private readonly IDbConnection _db;

        public UserRepository(TragateContext context, IMediatorHandler bus,
            IMapper mapper, IDbConnection db) : base(context){
            _mapper = mapper;
            _bus = bus;
            _db = db;
        }

        //TODO:Eventual Consistency ya da UoW yaklaşımları dahilinde düşünmek lazım.
        public bool Add(User user, Company company, IEnumerable<int> companyCategories, int personId){
            var transaction = Db.Database.BeginTransaction(IsolationLevel.ReadUncommitted);
            try{
                //User
                Db.User.Add(user);
                Db.SaveChanges();

                //CompanyAdmin
                var companyAdmin = _mapper.Map<CompanyAdmin>(user);
                companyAdmin.PersonId = personId;
                companyAdmin.CompanyId = user.Id;
                Db.CompanyAdmin.Add(companyAdmin);

                //Company
                company.ContactUserId = company.OwnerUserId = personId;
                company.UserId = user.Id;
                Db.Company.Add(company);

                //CompanyCategory
                foreach (var item in companyCategories){
                    Db.CompanyCategory.Add(new CompanyCategory()
                    {
                        CategoryId = item,
                        CompanyId = user.Id
                    });
                }

                Db.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (System.Exception ex){
                _bus.RaiseEvent(new DomainNotification("User.Company.Add",
                    "We had a problem during saving your data."));
                Log.Error(ex, "An error occured when user add transaction !");
                transaction.Rollback();
                return false;
            }
        }

        //TODO:Eventual Consistency ya da UoW yaklaşımları dahilinde düşünmek lazım.
        public bool Update(User user, IEnumerable<int> companyCategories, Company company){
            var transaction = Db.Database.BeginTransaction(IsolationLevel.ReadUncommitted);
            try{
                Db.Update(user);
                Db.Update(company);

                if (companyCategories.Any()){
                    Db.CompanyCategory.RemoveRange(Db.CompanyCategory.Where(x => x.CompanyId == user.Id).ToList());
                    foreach (var item in companyCategories){
                        Db.CompanyCategory.Add(new CompanyCategory()
                        {
                            CategoryId = item,
                            CompanyId = user.Id
                        });
                    }
                }

                Db.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (System.Exception ex){
                _bus.RaiseEvent(new DomainNotification("User.Company.Update",
                    "We had a problem during saving your data."));
                Log.Error(ex, "An error occured when user update transaction !");
                transaction.Rollback();
                return false;
            }
        }

        /// <summary>
        /// Login'den bagımsız çalışması lazım yoksa query arttıkca user'ın login hızı düşer.Ayrılması lazım buranın
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public UserDto GetByEmail(string email){
            if (!string.IsNullOrEmpty(email)){
                var query = $@"SELECT
                              [user].*,
                              [user].UserTypeId  AS UserType,
                              [user].StatusId  AS StatusType,
                              [user].RegisterTypeId  AS RegisterType,
                              location.*,
                              country.*,
                              p.Id,
                              p.ParameterValue1  as Value,
                              p2.Id,
                              p2.ParameterValue1 as Value,
                              p3.Id,
                              p3.ParameterValue1 as Value
                            FROM [User] [user]
                              INNER JOIN Location location ON [user].LocationId = location.Id
                              INNER JOIN Location country ON [user].CountryId = country.Id
                              INNER JOIN Parameter p ON p.ParameterType = 'StatusId' AND p.ParameterCode = [user].StatusId
                              INNER JOIN Parameter p2 ON p2.ParameterType = 'UserTypeId' AND p2.ParameterCode = [user].UserTypeId
                              INNER JOIN Parameter p3 ON p3.ParameterType = 'RegisterTypeId' AND p3.ParameterCode = [user].RegisterTypeId
                           WHERE [user].Email = '{email}' AND [user].UserTypeId = {(int) UserType.Person}";
                return _db.Query<UserDto, LocationDto, LocationDto, ParameterDto, ParameterDto, ParameterDto, UserDto>(
                    query,
                    (user, location, country, p, p2, p3) =>
                    {
                        user.Location = location;
                        user.Country = country;
                        user.UserStatus = user.StatusType == StatusType.Active;
                        user.Status = p.Value;
                        user.UserTypeName = p2.Value;
                        user.RegisterTypeName = p3.Value;
                        user.ProfileImagePath = user.ProfileImagePath.CheckUserProfileImage();
                        return user;
                    }).SingleOrDefault();
            }

            return null;
        }

        /// <summary>
        ///  Login'den bagımsız çalışması lazım yoksa query arttıkca user'ın login hızı düşer.Ayrılması lazım buranın
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public UserDto GetAdminByEmail(string email){
            var query = $@"SELECT
                              [user].*,
                              [user].UserTypeId  AS UserType,
                              [user].StatusId  AS StatusType,
                              [user].RegisterTypeId  AS RegisterType,
                              systemadmin.*,
                              p.Id,
                              p.ParameterValue1  as Value,
                              p2.Id,
                              p2.ParameterValue1 as Value,
                              p3.Id,
                              p3.ParameterValue1 as Value
                            FROM [User] [user]
                              INNER JOIN SystemAdmin systemadmin ON systemadmin.UserId = [user].Id
                              INNER JOIN Location location ON [user].LocationId = location.Id
                              INNER JOIN Location country ON [user].CountryId = country.Id
                              INNER JOIN Parameter p ON p.ParameterType = 'StatusId' AND p.ParameterCode = [user].StatusId
                              INNER JOIN Parameter p2 ON p2.ParameterType = 'UserTypeId' AND p2.ParameterCode = [user].UserTypeId
                              INNER JOIN Parameter p3 ON p3.ParameterType = 'RegisterTypeId' AND p3.ParameterCode = [user].RegisterTypeId
                           WHERE [user].Email = '{email}' AND [user].UserTypeId = {(int) UserType.Person}";

            return _db.Query<UserDto, SystemAdmin, ParameterDto, ParameterDto, ParameterDto, UserDto>(query,
                (user, systemadmin, p, p2, p3) =>
                {
                    user.UserStatus = user.StatusType == StatusType.Active &&
                                      systemadmin.StatusId == (int) StatusType.Active;
                    user.Status = p.Value;
                    user.UserTypeName = p2.Value;
                    user.RegisterTypeName = p3.Value;
                    user.ProfileImagePath = user.ProfileImagePath.CheckUserProfileImage();
                    return user;
                }).SingleOrDefault();
        }


        public UserDto GetUserById(int id){
            var query = $@"SELECT
                            u.*,
                            u.RegisterTypeId as RegisterType,
                            u.UserTypeId as UserType ,
                            u.StatusId as StatusType,
                            p.Id,
                            p.ParameterValue1  as Value,
                            p2.Id,
                            p2.ParameterValue1 as Value,
                            p3.Id,
                            p3.ParameterValue1 as Value,
                            l.*
                    FROM [User] u
                    INNER JOIN Location l ON u.LocationId = l.Id
                    INNER JOIN Parameter p on p.ParameterType = 'StatusId' and p.ParameterCode = u.StatusId
                    INNER JOIN Parameter p2 on p2.ParameterType = 'UserTypeId' and p2.ParameterCode = u.UserTypeId
                    INNER JOIN Parameter p3 on p3.ParameterType = 'RegisterTypeId' and p3.ParameterCode = u.RegisterTypeId
                    WHERE u.Id = {id}";

            var result = _db.Query<UserDto, ParameterDto, ParameterDto, ParameterDto, LocationDto, UserDto>(query,
                (u, p, p2, p3, l) =>
                {
                    u.Location = l;
                    u.Status = p.Value;
                    u.UserTypeName = p2.Value;
                    u.RegisterTypeName = p3.Value;
                    u.ProfileImagePath = u.ProfileImagePath.CheckUserProfileImage();
                    return u;
                },
                splitOn: "Id").FirstOrDefault();
            return result;
        }

        public IEnumerable<User> GetPersonsByStatus(int page, int pageSize, string name,
            StatusType status = StatusType.All){
            return GetUserByUserTypeAndStatus(UserType.Person, page, pageSize, name, status);
        }

        public int CountByUserTypeAndStatus(UserType userType, string name, StatusType status){
            name = name?.ToLower(new System.Globalization.CultureInfo("en-US"));
            var sb = new StringBuilder();
            sb.Append($"SELECT Count(*) FROM [User] WHERE UserTypeId = {(byte) userType} ");
            if (status != StatusType.All)
                sb.AppendLine($"and StatusId ={(byte) status}");
            if (!string.IsNullOrEmpty(name))
                sb.AppendLine($"and FullName like '%{name}%'");

            return _db.ExecuteScalar<int>(sb.ToString());
        }

        public IEnumerable<User> GetUserByUserTypeAndStatus(UserType userType, int page, int pageSize, string name,
            StatusType status){
            page -= 1;
            name = name?.ToLower(new System.Globalization.CultureInfo("en-US"));
            var sb = new StringBuilder();
            sb.Append($"SELECT * FROM [User] WHERE UserTypeId = {(byte) userType} ");
            if (status != StatusType.All)
                sb.AppendLine($"and StatusId ={(byte) status}");
            if (!string.IsNullOrEmpty(name))
                sb.AppendLine($"and FullName like '%{name}%'");
            sb.AppendLine($"order by 1 desc OFFSET {page * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY");

            return _db.Query<User>(sb.ToString()).ToList();
        }

        public UserDashboardDto GetUserDashboardById(int id){
            var sql = $@"SELECT count(*)
            FROM CompanyAdmin ca inner join Company c on ca.CompanyId = c.UserId
            where ca.PersonId = {id} and  c.StatusId = {(int) StatusType.Active} 
                  and ca.StatusId = {(int) StatusType.Active}";

            return new UserDashboardDto()
            {
                CompanyCount = _db.ExecuteScalar<int>(sql)
            };
        }

        public AdminUserDashboardDto GetAdminUserDashboardById(int id){
            return new AdminUserDashboardDto()
            {
                CompanyCount = _db.ExecuteScalar<int>(
                    $"SELECT COUNT(id) FROM CompanyAdmin WHERE PersonId = {id} AND StatusId = {(int) StatusType.Active}"),
                ProductCount = _db.ExecuteScalar<int>(
                    $"SELECT COUNT(id) FROM Product WHERE CreatedUserId = {id} AND StatusId = {(int) StatusType.Active}"),
                TaskCount = _db.ExecuteScalar<int>(
                    $"SELECT COUNT(id) FROM CompanyTask WHERE ResponsibleUserId = {id} AND StatusId = {(int) StatusType.Active}")
            };
        }

        public List<UserTodoListDto> GetTodoListByUserId(int id){
            var userTodoList = new List<UserTodoListDto>();
            var sql =
                $@"SELECT ProfileImagePath,EmailVerified FROM [User] 
                   WHERE Id = {id} and StatusId = {(int) StatusType.Active}";
            var result = _db.Query(sql).Single();

            if (!result.EmailVerified){
                userTodoList.Add(new UserTodoListDto()
                {
                    Key = "Get verification email and verify your account",
                    Value = "/user/sendverifyemail"
                });
            }

            if (string.IsNullOrEmpty(result.ProfileImagePath)){
                userTodoList.Add(new UserTodoListDto()
                {
                    Key = "Upload your profile photo",
                    Value = "/user/settings"
                });
            }

            var firstUser = _db.Query($@"SELECT TOP 1 c.UserId 
                                FROM CompanyAdmin ca
                          INNER JOIN [User] u ON u.Id = ca.CompanyId
                          INNER JOIN Company c ON c.UserId = ca.CompanyId
                       WHERE ca.PersonId = {id} 
                            AND ca.StatusId = {(int) StatusType.Active} 
                            AND c.StatusId = {(int) StatusType.Active}
                            AND (u.ProfileImagePath IS NULL OR c.EstablishmentYear IS NULL)").SingleOrDefault();

            userTodoList.AddRange(GetTodoListByCompanyId(firstUser != null ? (int) firstUser.UserId : 0));
            return userTodoList;
        }

        public List<UserTodoListDto> GetTodoListByCompanyId(int id){
            var companyTodoList = new List<UserTodoListDto>();
            var result = _db.Query($@"select  u.ProfileImagePath,
                               c.EstablishmentYear 
                       from [User] u inner join Company 
                       c ON u.Id = c.UserId and u.Id={id}").SingleOrDefault();
            if (result != null){
                if (string.IsNullOrEmpty(result.EstablishmentYear)){
                    companyTodoList.Add(new UserTodoListDto()
                    {
                        Key = "Complete company information",
                        Value = $"/companyadmin/edit/{id}"
                    });
                }

                if (string.IsNullOrEmpty(result.ProfileImagePath)){
                    companyTodoList.Add(new UserTodoListDto()
                    {
                        Key = "Upload company logo",
                        Value = $"/companyadmin/settings/{id}"
                    });
                }

                var count = _db.ExecuteScalar<int>($"select count(*) from Product p where p.CompanyId={id}");
                if (count == 0){
                    companyTodoList.Add(new UserTodoListDto()
                    {
                        Key = "Add products for your company",
                        Value = $"/companyadmin/products/{id}"
                    });
                }
            }

            return companyTodoList;
        }

        public List<AdminUserDto> GetAdminUsers(StatusType status){
            var sb = new StringBuilder();
            sb.AppendLine(
                @"SELECT u.Id,u.FullName as Name,Email FROM [User] u INNER JOIN SystemAdmin s ON u.Id = s.UserId");
            if (status > 0)
                sb.AppendLine($"where s.StatusId = {(int) status}");
            return _db.Query<AdminUserDto>(sb.ToString()).ToList();
        }

        public UserDto GetUserByExternalUserId(string userId, string email){
            var query = $@"SELECT
                              [user].*,
                              [user].UserTypeId  AS UserType,
                              [user].StatusId  AS StatusType,
                              [user].RegisterTypeId  AS RegisterType,
                              location.*,
                              country.*,
                              p.Id,
                              p.ParameterValue1  as Value,
                              p2.Id,
                              p2.ParameterValue1 as Value,
                              p3.Id,
                              p3.ParameterValue1 as Value
                            FROM [User] [user]
                              INNER JOIN Location location ON [user].LocationId = location.Id
                              INNER JOIN Location country ON [user].CountryId = country.Id
                              INNER JOIN Parameter p ON p.ParameterType = 'StatusId' AND p.ParameterCode = [user].StatusId
                              INNER JOIN Parameter p2 ON p2.ParameterType = 'UserTypeId' AND p2.ParameterCode = [user].UserTypeId
                              INNER JOIN Parameter p3 ON p3.ParameterType = 'RegisterTypeId' AND p3.ParameterCode = [user].RegisterTypeId
                           WHERE [user].ExternalUserId = '{userId}' AND [user].Email = '{email}' AND [user].StatusId={(int) StatusType.Active}";
            return _db.Query<UserDto, LocationDto, LocationDto, ParameterDto, ParameterDto, ParameterDto, UserDto>(
                query,
                (user, location, country, p, p2, p3) =>
                {
                    user.Location = location;
                    user.Country = country;
                    user.UserStatus = user.StatusType == StatusType.Active;
                    user.Status = p.Value;
                    user.UserTypeName = p2.Value;
                    user.RegisterTypeName = p3.Value;
                    user.ProfileImagePath = user.ProfileImagePath.CheckUserProfileImage();
                    return user;
                }).SingleOrDefault();
        }
    }
}