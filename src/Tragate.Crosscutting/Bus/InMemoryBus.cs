using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using Serilog;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Core.Events;

namespace Tragate.CrossCutting.Bus
{
    public sealed class InMemoryBus : IMediatorHandler
    {
        private readonly IMediator _mediator;
        private readonly IEventStore _eventStore;

        public InMemoryBus(IEventStore eventStore, IMediator mediator)
        {
            _eventStore = eventStore;
            _mediator = mediator;
        }

        public Task RaiseEvent<T>(T @event) where T : Event
        {
            if (!@event.MessageType.Equals("DomainNotification"))
                _eventStore?.Save(@event);
            
            Log.Information(JsonConvert.SerializeObject(@event));
            return Publish(@event);
        }

        public Task SendCommand<T>(T command) where T : Command
        {
            return Publish(command);
        }

        private Task Publish<T>(T message) where T : Message
        {
            return _mediator.Publish(message);
        }
    }
}