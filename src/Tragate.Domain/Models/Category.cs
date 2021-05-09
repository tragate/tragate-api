using System;
using Tragate.Domain.Core.Models;

namespace Tragate.Domain.Models {
    public class Category : Entity {
        public string Title { get; set; }
        public string Slug { get; set; }
        public int? ParentId { get; set; }
        public byte StatusId { get; set; }
        public string MetaKeyword { get; set; }
        public string MetaDescription { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ImagePath { get; set; }
        public int? Priority { get; set; }
    }
}