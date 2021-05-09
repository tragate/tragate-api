using System.Linq;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Interfaces;
using Tragate.Domain.Models;
using Tragate.Infra.Data.Context;
using Tragate.Infra.Data.Repository;

namespace Tragate.Infra.Data {
    public class ParameterRepository : Repository<Parameter>, IParameterRepository {
        public ParameterRepository (TragateContext context) : base (context) {

        }

        public IQueryable<Parameter> GetParameterByType (string type, int statusId) {
            if (statusId == (int) StatusType.All)
                return Db.Parameter.Where (x => x.ParameterType == type).OrderBy (x => x.ParameterCode);
            else
                return Db.Parameter.Where (x => x.ParameterType == type && x.StatusId == statusId).OrderBy (x => x.ParameterCode);
        }
    }
}