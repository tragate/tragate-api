using System;
using System.Collections.Generic;
using Tragate.Common.Library.Enum;

namespace Tragate.Common.Library.Dto {
    public class CompanyAdminDto {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int CompanyId { get; set; }
        public int CompanyAdminRoleId { get; set; }
        public byte StatusId { get; set; }
    }
}