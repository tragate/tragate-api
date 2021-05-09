using System.Collections.Generic;
using System.Linq;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Models;

namespace Tragate.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        bool Add(User user, Company company, IEnumerable<int> companyCategories, int personId);
        bool Update(User user, IEnumerable<int> companyCategories, Company company);
        UserDto GetByEmail(string email);
        UserDto GetAdminByEmail(string email);
        UserDto GetUserById(int id);
        IEnumerable<User> GetPersonsByStatus(int page, int pageSize, string name, StatusType status);
        int CountByUserTypeAndStatus(UserType userType, string name, StatusType status);

        IEnumerable<User> GetUserByUserTypeAndStatus(UserType userType, int page, int pageSize, string name,
            StatusType status);

        UserDashboardDto GetUserDashboardById(int id);

        AdminUserDashboardDto GetAdminUserDashboardById(int id);

        List<UserTodoListDto> GetTodoListByUserId(int id);

        List<UserTodoListDto> GetTodoListByCompanyId(int id);

        List<AdminUserDto> GetAdminUsers(StatusType status);

        UserDto GetUserByExternalUserId(string userId, string email);
    }
}