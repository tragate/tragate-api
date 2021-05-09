using System;
using Tragate.Common.Library.Enum;

namespace Tragate.Domain.Commands {
    public abstract class CompanyAdminCommand : Command {
        public int Id { get; set; }
        public string Email { get; set; }
        public int PersonId { get; set; }
        public int CompanyId { get; set; }
        public int CompanyAdminRoleId { get; set; }
        public StatusType StatusType { get; set; }
    }
}