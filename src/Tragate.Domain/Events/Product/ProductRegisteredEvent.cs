using System.Collections.Generic;
using Tragate.Domain.Core.Events;

namespace Tragate.Domain.Events
{
    public class ProductRegisteredEvent : Event
    {
        public int Id { get; set; }
    }
}