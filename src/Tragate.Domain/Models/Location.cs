using System;
using Tragate.Domain.Core.Models;

namespace Tragate.Domain.Models {
    public class Location : Entity {
        public string Name { get; set; }
        public string FullName { get; set; }
        public int? ParentLocationId { get; set; }
        public int LocationTypeId { get; set; }
        public int StatusId { get; set; }
    }
}