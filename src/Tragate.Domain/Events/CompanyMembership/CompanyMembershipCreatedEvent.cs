using Tragate.Domain.Core.Events;

namespace Tragate.Domain.Events
{
    public class CompanyMembershipCreatedEvent : Event
    {
        public int CompanyId { get; set; }
    }
}