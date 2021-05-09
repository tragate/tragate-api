using System.Collections.Generic;
using System.Linq;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Models;

namespace Tragate.Domain.Interfaces {
    public interface IParameterRepository : IRepository<Parameter> {
        IQueryable<Parameter> GetParameterByType (string type, int statusId);
    }
}