using Tragate.Domain.Core.Events;

namespace Tragate.Domain.Events
{
    public class CompanyUpdatedEvent : Event
    {
        public int Id { get; set; }
    }
}