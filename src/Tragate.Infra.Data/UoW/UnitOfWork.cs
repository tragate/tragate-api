using System.Linq;
using Serilog;
using Tragate.Common.Library.Aspects;
using Tragate.Domain.Core.Commands;
using Tragate.Domain.Interfaces;
using Tragate.Infra.Data.Context;

namespace Tragate.Infra.Data.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TragateContext _context;

        public UnitOfWork(TragateContext context){
            _context = context;
        }

        [ExceptionHandler]
        public CommandResponse Commit(){
            var rowsAffected = _context.SaveChanges();
            return new CommandResponse(rowsAffected > 0);
        }

        public void Dispose(){
            _context.Dispose();
        }
    }
}