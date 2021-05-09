using Tragate.Application.ViewModels;
using Tragate.Common.Library.Dto;
using Tragate.Domain.Models;

namespace Tragate.Application.ServiceUtility.Login {
    public interface ILoginFactory {
        void Login (UserDto user, LoginViewModel model);
    }
}