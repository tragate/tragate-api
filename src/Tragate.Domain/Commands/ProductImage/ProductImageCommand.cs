using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Tragate.Domain.Commands
{
    public abstract class ProductImageCommand : Command
    {
        public int Id { get; set; }
        public Guid UuId { get; set; }
        public IFormFileCollection Files { get; set; }
        public int UpdatedUserId { get; set; }
    }
}