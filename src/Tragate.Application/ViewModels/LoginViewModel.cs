using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Tragate.Common.Library.Enum;

namespace Tragate.Application.ViewModels
{
    public class LoginViewModel : AnonymUserViewModel
    {
        public string Password { get; set; }
        public bool AutoLogin { get; set; }
        public string Token { get; set; }
        public int PlatformId { get; set; }
    }
}