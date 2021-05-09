using Tragate.Common.Library.Dto;
using Tragate.Domain.Core.Events;

namespace Tragate.Domain.Events
{
    public class QuoteHistoryCreatedEvent : Event
    {
        public int QuoteId { get; set; }
        public int CompanyId { get; set; }
        public string Title { get; set; }
        public int CreatedUserId { get; set; }
        public QuoteUserDto ReceiverUser { get; set; }
        public QuoteUserDto SenderUser { get; set; }
    }
}