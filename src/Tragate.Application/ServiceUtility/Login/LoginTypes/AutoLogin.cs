using System;
using Microsoft.Extensions.Caching.Distributed;
using Tragate.Application.ViewModels;
using Tragate.Common.Library.Dto;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Core.Notifications;

namespace Tragate.Application.ServiceUtility.Login.LoginTypes
{
    public class AutoLogin : ILoginFactory
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IMediatorHandler _bus;

        public AutoLogin(
            IMediatorHandler bus,
            IDistributedCache distributedCache){
            _distributedCache = distributedCache;
            _bus = bus;
        }

        public void Login(UserDto user, LoginViewModel model){
            var userId = _distributedCache.GetString(model.Token);
            if (userId == null){
                _bus.RaiseEvent(new DomainNotification("Login", "Invalid token"));
            }
            else{
                if (user == null || user.Id != Convert.ToInt32(userId)){
                    _bus.RaiseEvent(new DomainNotification("Login", "Invalid User"));
                }
                else{
                    _distributedCache.Remove(model.Token);
                }
            }
        }
    }
}