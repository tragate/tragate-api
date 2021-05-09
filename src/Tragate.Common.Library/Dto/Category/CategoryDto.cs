using System;
using System.Collections.Generic;
using Tragate.Common.Library.Enum;

namespace Tragate.Common.Library.Dto {
    public class CategoryDto {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string MetaKeyword { get; set; }
        public string MetaDescription { get; set; }
        public byte StatusId { get; set; }
        public string ImagePath { get; set; }
        public int? Priority { get; set; }
    }
}