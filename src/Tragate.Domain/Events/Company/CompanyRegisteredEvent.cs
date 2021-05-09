using Tragate.Common.Library.Enum;
using Tragate.Domain.Core.Events;

namespace Tragate.Domain.Events
{
    public class CompanyRegisteredEvent : Event
    {
        public int Id { get; set; }
    }
}