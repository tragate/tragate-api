using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tragate.Application;
using Tragate.Domain.Core.Notifications;

namespace Tragate.WebApi.Controllers
{
    public class ProductImageController : ApiController
    {
        private readonly IProductImageService _productImageService;

        public ProductImageController(INotificationHandler<DomainNotification> notifications,
            IProductImageService productImageService) : base(notifications){
            _productImageService = productImageService;
        }

        [HttpPost]
        [Route("productimages/{uuid:guid}/users/{userId:int:min(1)}")]
        public IActionResult UploadImage(Guid uuid, int userId, IFormFileCollection files){
            _productImageService.UploadImages(uuid, userId, files);
            return Response(null, "Images has been uploaded");
        }

        [HttpDelete]
        [Route("productimages/{id:int:min(1)}")]
        public IActionResult DeleteImage(int id){
            _productImageService.DeleteImage(id);
            return Response(null, "Images has been deleted");
        }
    }
}