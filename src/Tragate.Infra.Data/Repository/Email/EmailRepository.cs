using Tragate.Domain.Interfaces.Email;
using Tragate.Domain.Models;
using Tragate.Infra.Data.Context;

namespace Tragate.Infra.Data.Repository
{
    public class EmailRepository : Repository<Email>, IEmailRepository
    {
        public EmailRepository(TragateContext context) : base(context){
        }
    }
}