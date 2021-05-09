using Tragate.Domain.Core.Events;

namespace Tragate.Domain.Events
{
    public class CompanyFastAddedEvent : Event
    {
        public string Email { get; set; }
        public string Fullname { get; set; }
    }
}