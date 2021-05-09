using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Tragate.Application.ViewModels {
    public class ResetPasswordViewModel {
        public string Token { get; set; }
        public string Password { get; set; }
        public string PasswordMatch { get; set; }
    }
}