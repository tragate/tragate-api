using Tragate.Application.ViewModels;
using Tragate.Common.Library;
using Tragate.Common.Library.Dto;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Core.Notifications;

namespace Tragate.Application.ServiceUtility.Login.LoginTypes
{
    public class StandartLogin : ILoginFactory
    {
        private readonly IMediatorHandler _bus;

        public StandartLogin(IMediatorHandler bus){
            _bus = bus;
        }

        public void Login(UserDto user, LoginViewModel model){
            if (user != null){
                if (PasswordHashHelper.HashPassword(model.Password, user.Salt) != user.Password){
                    _bus.RaiseEvent(new DomainNotification("Login", "Password is invalid"));
                }

                if (!user.UserStatus){
                    _bus.RaiseEvent(new DomainNotification("Login", "Your User Account is not active"));
                }
            }
            else{
                _bus.RaiseEvent(new DomainNotification("Login", "Email doesn't exists"));
            }
        }
    }
}