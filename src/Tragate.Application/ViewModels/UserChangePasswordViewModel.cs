using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Tragate.Application.ViewModels {
    public class UserChangePasswordViewModel {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}