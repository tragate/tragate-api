using System;
using Tragate.Domain.Core.Models;

namespace Tragate.Domain.Models
{
    public class ProductImage : Entity
    {
        public int ProductId { get; set; }
        public string SmallImagePath { get; set; }
        public string BigImagePath { get; set; }
        public long FileSize { get; set; }
        public byte StatusId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}