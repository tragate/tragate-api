using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Tragate.Domain.Core.Events;
using Tragate.Infra.Data.Context;

namespace Tragate.Infra.Data.Repository.EventSourcing
{
    public class EventStoreSQLRepository : IEventStoreRepository
    {
        private EventStoreSQLContext _context;
        private readonly IServiceProvider _serviceProvider;

        public EventStoreSQLRepository(IServiceProvider serviceProvider){
            _serviceProvider = serviceProvider;
        }

        public IList<StoredEvent> All(int aggregateId){
            return (from e in _context.StoredEvent where e.AggregateId == aggregateId select e).ToList();
        }

        /// <summary>
        /// At all async  events are disposed therefore use service locator as below link; 
        /// https://thinkrethink.net/2017/12/22/cannot-access-a-disposed-object-in-asp-net-core-when-injecting-dbcontext/
        /// SaveChanges not async for get error as A second operation started on this context before a previous operation completed.
        /// Any instance members are not guaranteed to be thread safe."
        /// </summary>
        /// <param name="theEvent"></param>
        public void Store(StoredEvent theEvent){
            _context = _serviceProvider.GetRequiredService<EventStoreSQLContext>();
            _context.StoredEvent.Add(theEvent);
            _context.SaveChanges();
        }
    }
}