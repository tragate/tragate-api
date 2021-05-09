using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Nest;
using Tragate.Common.Library.Dto;
using Tragate.Domain.Models;

namespace Tragate.Domain.Interfaces
{
    public interface IProductImageRepository : IRepository<ProductImage>
    {
        bool AddList(int productId, List<ImageFileDto> files);
    }
}