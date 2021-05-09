using System;
using Tragate.Domain.Core.Models;

namespace Tragate.Domain.Models
{
    public class Email : Entity
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}