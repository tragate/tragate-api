using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.JsonPatch;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;

namespace Tragate.Application {
    public interface ILocationService : IDisposable {
        List<LocationDto> GetLocationByParentId (int? id, int statusId);
    }
}