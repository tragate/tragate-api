using MediatR;
using Microsoft.Extensions.Options;
using Tragate.Common.Library;
using Tragate.Common.Library.Enum;
using Tragate.Common.Library.Helpers;
using Tragate.Domain.Commands;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Core.Notifications;
using Tragate.Domain.Events;
using Tragate.Domain.Events.Image;
using Tragate.Domain.Interfaces;
using Tragate.Domain.Models;

namespace Tragate.Domain.CommandHandlers
{
    /// <summary>
    ///TODO: Tüm validasyonlar fluent validasyon classları içine taşınacak.Refactor edilecek
    /// </summary>
    public class BaseUserCommandHandler : CommandHandler
    {
        private readonly ConfigSettings _settings;
        private readonly IUserRepository _userRepository;

        protected BaseUserCommandHandler(IUnitOfWork uow,
            IMediatorHandler bus,
            INotificationHandler<DomainNotification> notifications,
            IOptions<ConfigSettings> settings, IUserRepository userRepository) : base(uow, bus,
            notifications){
            _userRepository = userRepository;
            _settings = settings.Value;
        }

        protected void UploadAndRemoveImage(User user, UploadImageCommand message, string fileName){
            base.RaiseEvent(new UserImageUploadedEvent()
            {
                UserId = message.UserId,
                ProfileImagePath = fileName
            });

            if (user.UserTypeId == (int) UserType.Company){
                base.RaiseEvent(new CompanyUpdatedEvent()
                {
                    Id = user.Id
                });
            }

            base.RaiseEvent(new ImageDeletedEvent()
            {
                BucketName = _settings.S3.FullImagePath,
                Key = user.ProfileImagePath.GetOldImagePath()
            });
        }

        protected bool ValidateEmailAndPassword(User user, string email, string password){
            if (user == null){
                base.RaiseEvent(new DomainNotification("ChangePasswordCommand", "User not found"));
                return false;
            }

            if (user.Password != PasswordHashHelper.HashPassword(password, user.Salt)){
                base.RaiseEvent(new DomainNotification("ChangePasswordCommand", "Password is invalid"));
                return false;
            }

            if (_userRepository.GetByEmail(email) != null){
                base.RaiseEvent(new DomainNotification("ChangePasswordCommand", "Email already exists"));
                return false;
            }

            return true;
        }
    }
}