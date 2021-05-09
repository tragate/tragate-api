using System;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using Tragate.Common.Library;
using Tragate.Common.Library.Enum;
using Tragate.Common.Library.Helpers;
using Tragate.Domain.Commands;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Core.Infrastructure;
using Tragate.Domain.Core.Notifications;
using Tragate.Domain.Events;
using Tragate.Domain.Interfaces;
using Tragate.Domain.Models;

namespace Tragate.Domain.CommandHandlers
{
    public class UserCommandHandler : BaseUserCommandHandler,
        INotificationHandler<RegisterNewPersonCommand>,
        INotificationHandler<UpdateUserEmailVerifyCommand>,
        INotificationHandler<ForgotPasswordCommand>,
        INotificationHandler<ResetPasswordCommand>,
        INotificationHandler<ChangePasswordCommand>,
        INotificationHandler<UploadImageCommand>,
        INotificationHandler<UpdateUserCommand>,
        INotificationHandler<ChangeEmailCommand>,
        INotificationHandler<SendActivationEmailCommand>,
        INotificationHandler<CompleteSignUpCommand>,
        INotificationHandler<RegisterNewExternalUserCommand>
    {
        private readonly IMapper _mapper;
        private readonly ConfigSettings _settings;
        private readonly IUserRepository _userRepository;
        private readonly IFileUploadService _fileUploadService;

        public UserCommandHandler(IMapper mapper,
            IUserRepository userRepository,
            IFileUploadService fileUploadService,
            IOptions<ConfigSettings> settings,
            IUnitOfWork uow,
            IMediatorHandler bus,
            INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications, settings,
            userRepository){
            _mapper = mapper;
            _settings = settings.Value;
            _userRepository = userRepository;
            _fileUploadService = fileUploadService;
        }

        public void Handle(RegisterNewPersonCommand message){
            base.Validate(message);
            if (_userRepository.GetByEmail(message.Email) != null){
                base.RaiseEvent(new DomainNotification("RegisterNewPersonCommand", "Already exists this email"));
                return;
            }

            var entity = _mapper.Map<User>(message);
            _userRepository.Add(entity);
            if (Commit()){
                var registeredEvent = _mapper.Map<UserRegisteredEvent>(entity);
                base.RaiseEvent(registeredEvent);
            }
        }

        /// <summary>
        /// If It was came from other social channel then It's ok but only to anonym user must set to tragate user ! 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(UpdateUserEmailVerifyCommand message){
            base.Validate(message);
            if (message.IsValid()){
                var user = _userRepository.GetById(message.Id);
                user.EmailVerified = message.EmailVerified;
                if (user.RegisterTypeId == (byte) RegisterType.Anonymous)
                    user.RegisterTypeId = (byte) RegisterType.Tragate;
                _userRepository.Update(user);
                base.Commit();
            }
        }

        public void Handle(ForgotPasswordCommand message){
            base.Validate(message);
            if (message.IsValid()){
                var user = _userRepository.GetByEmail(message.Email);
                if (user != null){
                    var forgotPasswordEvent = _mapper.Map<UserForgotPasswordEvent>(user);
                    base.RaiseEvent(forgotPasswordEvent);
                }
                else
                    base.RaiseEvent(new DomainNotification("ForgotPasswordCommand", "Email not found"));
            }
        }

        public void Handle(ResetPasswordCommand message){
            base.Validate(message);
            if (message.IsValid()){
                var user = _userRepository.GetById(message.Id);
                var salt = PasswordHashHelper.GetSalt();
                user.Password = PasswordHashHelper.HashPassword(message.Password, salt);
                user.Salt = salt;
                _userRepository.Update(user);
                base.Commit();
            }
        }

        public void Handle(ChangePasswordCommand message){
            base.Validate(message);
            if (message.IsValid()){
                var user = _userRepository.GetById(message.Id);
                if (user != null){
                    var oldPassword = PasswordHashHelper.HashPassword(message.OldPassword, user.Salt);
                    if (user.Password == oldPassword){
                        var salt = PasswordHashHelper.GetSalt();
                        user.Password = PasswordHashHelper.HashPassword(message.NewPassword, salt);
                        user.Salt = salt;
                        _userRepository.Update(user);
                        base.Commit();
                    }
                    else{
                        base.RaiseEvent(new DomainNotification("ChangePasswordCommand", "Old password is invalid"));
                    }
                }
                else{
                    base.RaiseEvent(new DomainNotification("ChangePasswordCommand", "User not found"));
                }
            }
        }

        public void Handle(ChangeEmailCommand message){
            base.Validate(message);
            if (message.IsValid()){
                var user = _userRepository.GetById(message.Id);
                if (base.ValidateEmailAndPassword(user, message.Email, message.Password)){
                    user.Email = message.Email;
                    _userRepository.Update(user);
                    if (base.Commit()){
                        base.SendCommand(new UpdateUserEmailVerifyCommand()
                        {
                            Id = message.Id,
                            EmailVerified = false
                        });

                        base.RaiseEvent(new UserRegisteredEvent()
                        {
                            UserId = message.Id,
                            Email = message.Email,
                            FullName = user.FullName,
                        });
                    }
                }
            }
        }

        public void Handle(UploadImageCommand message){
            base.Validate(message);
            var user = _userRepository.GetById(message.UserId);
            if (user == null){
                base.RaiseEvent(new DomainNotification("UserUploadImageCommand", "User not found"));
                return;
            }

            if (message.IsValid()){
                var fileName = Helper.GetUserImageName(user.FullName, (UserType) user.UserTypeId);
                var result =
                    _fileUploadService.Upload(message.UploadedFile, _settings.S3.UploadPath, fileName);
                result.ContinueWith(x =>
                {
                    if (!x.IsFaulted)
                        base.UploadAndRemoveImage(user, message, fileName);
                });
            }
        }

        public void Handle(UpdateUserCommand message){
            base.Validate(message);
            var user = _userRepository.GetById(message.Id);
            if (user == null){
                base.RaiseEvent(new DomainNotification("UpdateUserCommand", "User not found"));
                return;
            }

            _mapper.Map(message, user);
            _userRepository.Update(user);
            base.Commit();
        }

        public void Handle(SendActivationEmailCommand message){
            base.Validate(message);
            if (message.IsValid()){
                var user = _userRepository.GetByEmail(message.Email);
                if (user != null){
                    base.RaiseEvent(new UserRegisteredEvent()
                    {
                        UserId = user.Id,
                        Email = message.Email,
                        FullName = user.FullName,
                    });
                }
                else{
                    base.RaiseError(message, "Email doesn't exists");
                }
            }
        }

        /// <summary>
        /// TODO: Refactor -> Mapper kullan 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(CompleteSignUpCommand message){
            base.Validate(message);
            if (message.IsValid()){
                var user = _userRepository.GetById(message.Id);
                var salt = PasswordHashHelper.GetSalt();
                user.Salt = salt;
                user.FullName = message.FullName;
                user.Password = PasswordHashHelper.HashPassword(message.Password, salt);
                _userRepository.Update(user);
                if (base.Commit()){
                    base.SendCommand(new UpdateUserEmailVerifyCommand()
                    {
                        Id = message.Id,
                        EmailVerified = true
                    });
                }
            }
        }

        public void Handle(RegisterNewExternalUserCommand message){
            base.Validate(message);
            if (message.IsValid()){
                var existsUser = _userRepository.GetByEmail(message.Email);
                if (existsUser == null){
                    var user = _mapper.Map<User>(message);
                    _userRepository.Add(user);
                    base.Commit();
                }
                else{
                    base.RaiseError(message, "Already email exists");
                }
            }
        }
    }
}