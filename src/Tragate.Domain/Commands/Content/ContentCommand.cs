using System;
using Tragate.Common.Library.Enum;

namespace Tragate.Domain.Commands {
    public abstract class ContentCommand : Command {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public int? ContentTypeId { get; set; }
        public StatusType StatusType { get; set; }
        public int CreatedUserId { get; set; }
    }
}