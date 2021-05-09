using Microsoft.Extensions.Caching.Distributed;
using Tragate.Application.ServiceUtility.Login.LoginTypes;
using Tragate.Application.ViewModels;
using Tragate.Common.Library.Dto;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Interfaces;
using Tragate.Domain.Models;

namespace Tragate.Application.ServiceUtility.Login
{
    public class LoginFactory : ILoginFactory {

        private ILoginFactory _loginFactory;
        private readonly IMediatorHandler _bus;
        private readonly IDistributedCache _distributedCache;

        public LoginFactory (IMediatorHandler bus,
            IDistributedCache distributedCache) {
            _bus = bus;
            _distributedCache = distributedCache;
        }

        public void Login (UserDto user, LoginViewModel model) {
            if (!model.AutoLogin) {
                _loginFactory = new StandartLogin (_bus);
            } else {
                _loginFactory = new AutoLogin (_bus, _distributedCache);
            }

            _loginFactory.Login (user, model);
        }
    }
}