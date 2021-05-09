using System;
using Nest;
using Tragate.Common.Library.Enum;

namespace Tragate.Common.Library.Dto
{
    public class LocationDto
    {
        [Text(Ignore = true)]
        public int Id { get; set; }

        public string Name { get; set; }

        [Text(Ignore = true)]
        public int? ParentLocationId { get; set; }
    }
}