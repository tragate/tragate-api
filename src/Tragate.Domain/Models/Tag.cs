using System;
using Tragate.Domain.Core.Models;

namespace Tragate.Domain.Models
{
    public class Tag : Entity
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public int StatusId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}