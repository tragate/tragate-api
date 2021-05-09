using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using AutoMapper;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Tragate.Common.Library.Constants;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Core.Notifications;
using Tragate.Domain.Interfaces;
using Tragate.Domain.Models;
using Tragate.Infra.Data.Context;
using Tragate.Infra.Data.Repository;

namespace Tragate.Infra.Data {
    public class LocationRepository : Repository<Location>, ILocationRepository {
        public LocationRepository (TragateContext context) : base (context) {

        }

        public IQueryable<Location> GetLocationByParentId (int? id, int statusId) {
            if (statusId == (int) StatusType.All)
                return Db.Location.Where (x => x.ParentLocationId == id);
            else
                return Db.Location.Where (x => x.ParentLocationId == id && x.StatusId == statusId);
        }
    }
}