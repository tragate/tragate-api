using System;
using Tragate.Domain.Core.Models;

namespace Tragate.Domain.Models
{
    public class CompanyAdmin : Entity
    {
        public int PersonId { get; set; }
        public int CompanyId { get; set; }
        public byte CompanyAdminRoleId { get; set; }
        public byte StatusId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}