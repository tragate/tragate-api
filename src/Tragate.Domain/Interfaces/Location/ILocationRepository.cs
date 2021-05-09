using System.Collections.Generic;
using System.Linq;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Models;

namespace Tragate.Domain.Interfaces {
    public interface ILocationRepository : IRepository<Location> {
        IQueryable<Location> GetLocationByParentId (int? id, int statusId);
    }
}