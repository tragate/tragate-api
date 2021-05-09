using System;
using Tragate.Domain.Core.Models;

namespace Tragate.Domain.Models
{
    public class QuoteHistory : Entity
    {
        public int QuoteId { get; set; }
        public string Description { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}