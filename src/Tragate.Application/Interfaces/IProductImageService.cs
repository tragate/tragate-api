using System;
using Microsoft.AspNetCore.Http;
using Tragate.Common.Library.Dto;

namespace Tragate.Application
{
    public interface IProductImageService
    {
        void UploadImages(Guid uuid, int userId, IFormFileCollection files);
        void DeleteImage(int id);
        ProductImageDto GetById(int id);
    }
}