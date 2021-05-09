using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Tragate.Application.ViewModels;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;

namespace Tragate.Application
{
    public interface IUserService : IDisposable
    {
        UserDto Login(LoginViewModel model);

        UserDto ExternalLogin(ExternalLoginViewModel model);

        UserDto GetUserByVerifiedToken(string token);

        UserDto GetUserById(int id);

        UserDto GetUserByEmail(string email);

        IEnumerable<UserDto> GetPersonsByStatus(int page, int pageSize, string name, StatusType status);

        int CountByUserTypeAndStatus(UserType userType, StatusType status, string name);

        UserDashboardDto GetUserDashboardById(int id);

        AdminUserDashboardDto GetAdminUserDashboardById(int id);

        List<UserTodoListDto> GetTodoListById(int id);

        List<AdminUserDto> GetAdminUsers(StatusType status);

        void Verify(VerifyViewModel model);

        void PersonRegister(PersonViewModel model);

        void CompleteSignUp(CompleteSignUpViewModel model);

        void ExternalSignUp(ExternalSignUpViewModel model);

        void ForgotPassword(ForgotPasswordViewModel model);

        void ResetPassword(ResetPasswordViewModel model);

        void ChangePassword(int id, UserChangePasswordViewModel model);

        void ChangeEmail(int id, UserChangeEmailViewModel model);

        void UploadProfileImage(IFormFile files, int userId);

        void UpdateUser(int id, PersonViewModel model);

        void SendActionEmail(UserSendActivationEmailViewModel model);
    }
}