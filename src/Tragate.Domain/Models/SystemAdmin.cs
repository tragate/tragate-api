using System;
using Tragate.Domain.Core.Models;

namespace Tragate.Domain.Models
{
    public class SystemAdmin : Entity
    {
        public int UserId { get; set; }
        public byte SystemAdminRoleId { get; set; }
        public byte StatusId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}