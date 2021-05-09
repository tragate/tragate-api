using System;
using System.Collections.Generic;
using Tragate.Domain.Core.Events;

namespace Tragate.Infra.Data.Repository.EventSourcing
{
    public interface IEventStoreRepository
    {
        void Store(StoredEvent theEvent);
        IList<StoredEvent> All(int aggregateId);
    }
}