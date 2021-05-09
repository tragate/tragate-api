using System;
using Tragate.Domain.Core.Events;

namespace Tragate.Domain.Events
{
    public class AnonymUserCreatedEvent : Event
    {
        public AnonymUserCreatedEvent(){
            Subject = "Complete Your Sign Up";
        }

        public int UserId { get; set; }
        public string Subject { get; set; }
    }
}