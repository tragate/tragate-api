using System;
using Tragate.Domain.Core.Models;

namespace Tragate.Domain.Models {
    public class Content : Entity {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public byte? ContentTypeId { get; set; }
        public byte StatusId { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}