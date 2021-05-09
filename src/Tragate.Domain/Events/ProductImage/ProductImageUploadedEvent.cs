using System.Collections.Generic;
using Tragate.Common.Library.Dto;
using Tragate.Domain.Core.Events;

namespace Tragate.Domain.Events.ProductImage
{
    public class ProductImageUploadedEvent : Event
    {
        public int ProductId { get; set; }
        public List<ImageFileDto> Files { get; set; }
    }
}