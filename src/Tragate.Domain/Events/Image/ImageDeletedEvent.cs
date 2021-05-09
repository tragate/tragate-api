using Tragate.Domain.Core.Events;

namespace Tragate.Domain.Events.Image
{
    public class ImageDeletedEvent : Event
    {
        public string BucketName { get; set; }
        public string Key { get; set; }
    }
}