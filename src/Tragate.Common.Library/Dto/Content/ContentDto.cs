using System;
using Tragate.Common.Library.Enum;

namespace Tragate.Common.Library.Dto {
    public class ContentDto {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public byte? ContentTypeId { get; set; }
        public StatusType StatusType { get; set; }
    }
}