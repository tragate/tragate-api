using System;
using Tragate.Common.Library.Enum;

namespace Tragate.Common.Library.Dto {
    public class CompanyAdminUserDto {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string ProfileImagePath { get; set; }
    }
}