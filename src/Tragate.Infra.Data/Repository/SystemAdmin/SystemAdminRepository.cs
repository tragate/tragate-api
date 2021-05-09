using Tragate.Domain.Interfaces;
using Tragate.Domain.Models;
using Tragate.Infra.Data.Context;
using Tragate.Infra.Data.Repository;

namespace Tragate.Infra.Data
{
    public class SystemAdminRepository : Repository<SystemAdmin>, ISystemAdminRepository
    {
        public SystemAdminRepository(TragateContext context) : base(context)
        {
        }
    }
}