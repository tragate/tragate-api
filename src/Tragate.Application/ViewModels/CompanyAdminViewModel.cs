using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Tragate.Common.Library.Enum;

namespace Tragate.Application.ViewModels {
    public class CompanyAdminViewModel {
        public string Email { get; set; }
        public int CompanyId { get; set; }
        public int CompanyAdminRoleId { get; set; }
        public int StatusId { get; set; }
    }
}