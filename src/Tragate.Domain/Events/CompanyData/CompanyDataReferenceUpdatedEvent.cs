using Tragate.Domain.Core.Events;

namespace Tragate.Domain.Events.CompanyData
{
    public class CompanyDataReferenceUpdatedEvent : Event
    {
        public int Id { get; set; }
    }
}