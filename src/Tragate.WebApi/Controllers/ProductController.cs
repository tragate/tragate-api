using System;
using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Tragate.Application;
using Tragate.Application.ViewModels;
using Tragate.Common.Library;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;
using Tragate.Common.Result;
using Tragate.Domain.Core.Notifications;

namespace Tragate.WebApi.Controllers
{
    /// <summary>
    /// TODO: tag autocomplete ve addon olmalı ki olmayan tag anında eklenip
    /// reload olup idsi alınabilmeli ki productTag tablosuna basabilelim.
    /// https://demos.telerik.com/kendo-ui/multiselect/addnewitem
    /// </summary>
    public class ProductController : ApiController
    {
        private readonly IProductService _productService;
        private readonly ConfigSettings _settings;

        public ProductController(
            INotificationHandler<DomainNotification> notifications,
            IProductService productService,
            IOptions<ConfigSettings> settings) : base(notifications){
            _productService = productService;
            _settings = settings.Value;
        }

        [HttpGet]
        [Route("products/page={page:int:min(1)}/pageSize={pageSize:int:max(28)}")]
        public IActionResult GetProducts(int page, int pageSize, int? companyId, int? createdUserId, string name,
            int status){
            var result =
                _productService.GetProducts(page, pageSize, name, (StatusType) status, companyId, createdUserId);
            var count = _productService.CountProducts(name, (StatusType) status, companyId, createdUserId);
            var model = new PaginatedItemsViewModel<ProductListDto>(
                page, pageSize, count, result);

            return Response(model);
        }

        [HttpGet]
        [Route("products/{id:int:min(1)}")]
        public IActionResult GetDetailById(int id){
            return Response(_productService.GetDetailById(id));
        }

        [HttpGet]
        [Route("products/{slug}")]
        public IActionResult GetDetailBySlug(string slug){
            return Response(_productService.GetDetailBySlug(slug));
        }

        [HttpPost]
        [Route("products")]
        public IActionResult AddProduct([FromBody] ProductViewModel model){
            model.UuId = Guid.NewGuid();
            _productService.AddProduct(model);
            return Response(null, "Product has been created", new List<Link>
            {
                new Link
                {
                    Rel = "product.image.upload",
                    Href = $"{_settings.ApiUrl}/productimages/{model.UuId}/users/{model.CreatedUserId}"
                }
            });
        }

        [HttpPut]
        [Route("products")]
        public IActionResult UpdateProduct([FromBody] ProductViewModel model){
            _productService.UpdateProduct(model);
            return Response(null, "Product has been updated");
        }

        [HttpDelete]
        [Route("products/{id:int:min(1)}")]
        public IActionResult DeleteProduct(int id){
            _productService.DeleteProduct(id);
            return Response(null, "Product has been deleted");
        }

        [HttpPatch]
        [Route("products/{id}/categories")]
        public IActionResult UpdateCategory(int id, [FromBody] ProductCategoryViewModel model){
            _productService.UpdateCategoryProduct(id, model);
            return Response(null, "Product category has been updated");
        }

        [HttpPatch]
        [Route("products/{id}/productimages/{imageId}")]
        public IActionResult UpdateListImage(int id, int imageId, int updatedUserId){
            _productService.UpdateProductListImage(id, imageId, updatedUserId);
            return Response(null, "Product list image has been updated");
        }

        [HttpPatch]
        [Route("products")]
        public IActionResult UpdateStatus([FromBody] ProductStatusViewModel model){
            _productService.UpdateStatusProduct(model);
            return Response(null, "Product status has been updated");
        }
    }
}