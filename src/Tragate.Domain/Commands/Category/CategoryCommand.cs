using System;
using Microsoft.AspNetCore.Http;
using Tragate.Common.Library.Enum;

namespace Tragate.Domain.Commands {
    public abstract class CategoryCommand : Command {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public int? ParentId { get; set; }
        public StatusType StatusType { get; set; }
        public string Metakeyword { get; set; }
        public string MetaDescription { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool HasChild { get; set; }
        public IFormFile UploadedFile { get; set; }
        public int Priority { get; set; }
    }
}