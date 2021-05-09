using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tragate.Application;
using Tragate.Application.ViewModels;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Core.Notifications;

namespace Tragate.WebApi.Controllers
{
    public class CategoryController : ApiController
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        public CategoryController(
            INotificationHandler<DomainNotification> notifications,
            ICategoryService categoryService,
            IProductService productService) : base(notifications){
            _categoryService = categoryService;
            _productService = productService;
        }

        [HttpGet]
        [Route("categories/status={status:int}")]
        public IActionResult GetCategories(int status, int? parentId, string slug){
            var result = _categoryService.GetCategories((StatusType) status, parentId, slug);
            return Response(result);
        }

        [HttpGet]
        [Route("categories/category-path/{id:int:min(1)}")]
        public IActionResult GetCategoryPathById(int id){
            var result = _categoryService.GetCategoryPathById(id);
            return Response(result);
        }

        [HttpGet]
        [Route("categories/category-path/{slug}")]
        public IActionResult GetCategoryPathByBySlug(string slug){
            var result = _categoryService.GetCategoryPathBySlug(slug);
            return Response(result);
        }

        [HttpGet]
        [Route("categories/category-group")]
        public IActionResult GetCategoryGroup(){
            return Response(_categoryService.GetCategoryGroup());
        }

        [HttpGet]
        [Route("categories/subcategory-group")]
        public IActionResult GetSubCategoryGroup([FromQuery] int[] parentId, int pageSize = 5){
            return Response(_categoryService.GetSubCategoryGroup(parentId, pageSize));
        }

        [HttpGet]
        [Route("categories/id={id:int:min(1)}")]
        public IActionResult GetCategoryById(int id){
            var result = _categoryService.GetCategoryById(id);
            return Response(result);
        }

        [HttpGet]
        [Route("categories/{slug}")]
        public IActionResult GetCategoryBySlug(string slug){
            var result = _categoryService.GetCategoryBySlug(slug);
            return Response(result);
        }

        [HttpGet]
        [Route("categories/{slug}/products-xml")]
        public IActionResult GetProductsByCategorySlug(string slug, int status){
            var category = _categoryService.GetCategoryBySlug(slug);
            var products = _productService.GetProductsByCategoryId(category.Id, (StatusType) status);
            return Response(products);
        }

        [HttpPost]
        [Route("categories")]
        public IActionResult AddCategory([FromBody] CategoryViewModel model){
            _categoryService.AddCategory(model);
            return Response(null, "Category has been created");
        }

        [HttpPut]
        [Route("categories")]
        public IActionResult UpdateCategory([FromBody] CategoryViewModel model){
            _categoryService.UpdateCategory(model);
            return Response(null, "Category has been updated");
        }

        [HttpPost]
        [Route("categories/uploadImage/{id:int:min(1)}")]
        public IActionResult ChangeCategoryImage(IFormFile files, int id){
            _categoryService.UploadImage(files, id);
            return Response(null, "Image has been uploded");
        }
    }
}