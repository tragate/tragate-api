using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Tragate.Application.ServiceUtility.Login;
using Tragate.Application.ViewModels;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Commands;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Core.Notifications;
using Tragate.Domain.Interfaces;

namespace Tragate.Application
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _bus;
        private readonly IUserRepository _userRepository;
        private readonly IDistributedCache _distributedCache;
        private readonly ILoginFactory _loginFactory;

        public UserService(IMapper mapper,
            IMediatorHandler bus,
            IUserRepository userRepository,
            IDistributedCache distributedCache,
            ILoginFactory loginFactory){
            _bus = bus;
            _mapper = mapper;
            _userRepository = userRepository;
            _distributedCache = distributedCache;
            _loginFactory = loginFactory;
        }

        /// <summary>
        /// PlatformId uses for find the system admin but It will developed permission process next time ...
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public UserDto Login(LoginViewModel model){
            if (!(PlatformType.Admin == (PlatformType) model.PlatformId ||
                  PlatformType.Web == (PlatformType) model.PlatformId)){
                _bus.RaiseEvent(new DomainNotification("Login", "You're not allowed"));
                return null;
            }

            var user = PlatformType.Admin == (PlatformType) model.PlatformId
                ? _userRepository.GetAdminByEmail(model.Email)
                : _userRepository.GetByEmail(model.Email);

            _loginFactory.Login(user, model);
            return user;
        }

        public UserDto ExternalLogin(ExternalLoginViewModel model){
            var user = _userRepository.GetUserByExternalUserId(model.ExternalUserId, model.Email);
            if (user == null){
                _bus.RaiseEvent(new DomainNotification("ExternalLogin", "User not found"));
                return null;
            }

            return user;
        }

        public IEnumerable<UserDto> GetPersonsByStatus(int page, int pageSize, string name, StatusType status){
            var result = _userRepository.GetPersonsByStatus(page, pageSize, name, status);
            return _mapper.Map<IEnumerable<UserDto>>(result);
        }

        public int CountByUserTypeAndStatus(UserType userType, StatusType status, string name){
            return _userRepository.CountByUserTypeAndStatus(userType, name, status);
        }

        public UserDashboardDto GetUserDashboardById(int id){
            return _userRepository.GetUserDashboardById(id);
        }

        public AdminUserDashboardDto GetAdminUserDashboardById(int id){
            return _userRepository.GetAdminUserDashboardById(id);
        }

        public List<UserTodoListDto> GetTodoListById(int id){
            var user = _userRepository.GetById(id);
            if (user == null){
                _bus.RaiseEvent(new DomainNotification("UserTodoList", "User not found"));
                return null;
            }

            return user.UserTypeId == (int) UserType.Person
                ? _userRepository.GetTodoListByUserId(id)
                : _userRepository.GetTodoListByCompanyId(id);
        }

        public List<AdminUserDto> GetAdminUsers(StatusType status){
            return _userRepository.GetAdminUsers(status);
        }

        public UserDto GetUserById(int id){
            return _userRepository.GetUserById(id);
        }

        public UserDto GetUserByEmail(string email){
            return _mapper.Map<UserDto>(_userRepository.GetByEmail(email));
        }

        public UserDto GetUserByVerifiedToken(string token){
            var userId = _distributedCache.GetString(token);
            if (userId != null){
                return GetUserById(Convert.ToInt32(userId));
            }

            _bus.RaiseEvent(new DomainNotification("Verify", "Activation Link has expired"));
            return null;
        }

        public void CompleteSignUp(CompleteSignUpViewModel model){
            var completeSignUpCommand = _mapper.Map<CompleteSignUpCommand>(model);
            var user = GetUserByVerifiedToken(model.Token);
            if (user != null){
                completeSignUpCommand.Id = user.Id;
                _bus.SendCommand(completeSignUpCommand);
            }
        }

        public void ExternalSignUp(ExternalSignUpViewModel model){
            var signupCommand = _mapper.Map<RegisterNewExternalUserCommand>(model);
            _bus.SendCommand(signupCommand);
        }

        public void ForgotPassword(ForgotPasswordViewModel model){
            var forgotPasswordCommand = _mapper.Map<ForgotPasswordCommand>(model);
            _bus.SendCommand(forgotPasswordCommand);
        }

        public void ResetPassword(ResetPasswordViewModel model){
            var resetPasswordCommand = _mapper.Map<ResetPasswordCommand>(model);
            var user = GetUserByVerifiedToken(model.Token);
            if (user != null){
                resetPasswordCommand.Id = user.Id;
                _bus.SendCommand(resetPasswordCommand);
            }
        }

        public void ChangePassword(int id, UserChangePasswordViewModel model){
            var changePasswordCommand = _mapper.Map<ChangePasswordCommand>(model);
            changePasswordCommand.Id = id;
            _bus.SendCommand(changePasswordCommand);
        }

        public void ChangeEmail(int id, UserChangeEmailViewModel model){
            var changeEmailCommand = _mapper.Map<ChangeEmailCommand>(model);
            changeEmailCommand.Id = id;
            _bus.SendCommand(changeEmailCommand);
        }

        public void Verify(VerifyViewModel model){
            var user = GetUserByVerifiedToken(model.Token);
            if (user != null){
                var updateUserStatusCommand =
                    new UpdateUserEmailVerifyCommand {Id = Convert.ToInt32(user.Id), EmailVerified = true};
                _bus.SendCommand(updateUserStatusCommand);
            }
        }

        public void PersonRegister(PersonViewModel model){
            var registerCommand = _mapper.Map<RegisterNewPersonCommand>(model);
            _bus.SendCommand(registerCommand);
        }

        public void UploadProfileImage(IFormFile files, int userId){
            var uploadedFileCommand = new UploadImageCommand()
            {
                UserId = userId,
                UploadedFile = files
            };
            _bus.SendCommand(uploadedFileCommand);
        }

        public void UpdateUser(int id, PersonViewModel model){
            var updateUserCommand = _mapper.Map<UpdateUserCommand>(model);
            updateUserCommand.Id = id;
            _bus.SendCommand(updateUserCommand);
        }

        public void SendActionEmail(UserSendActivationEmailViewModel model){
            _bus.SendCommand(new SendActivationEmailCommand()
            {
                Email = model.Email
            });
        }

        public void Dispose(){
            GC.SuppressFinalize(this);
        }
    }
}