using System;
using Tragate.Domain.Core.Commands;

namespace Tragate.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        CommandResponse Commit();
    }
}