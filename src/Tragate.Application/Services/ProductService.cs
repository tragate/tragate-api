using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Tragate.Application.ViewModels;
using Tragate.Common.Library;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Commands;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Core.Notifications;
using Tragate.Domain.Interfaces;

namespace Tragate.Application
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICompanyAdminService _companyAdminService;
        private readonly IProductImageService _productImageService;
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _bus;

        public ProductService(
            IProductRepository productRepository,
            IMapper mapper,
            IMediatorHandler bus,
            ICompanyAdminService companyAdminService,
            IProductImageService productImageService){
            _productRepository = productRepository;
            _mapper = mapper;
            _bus = bus;
            _companyAdminService = companyAdminService;
            _productImageService = productImageService;
        }

        public ProductDetailDto GetDetailById(int id){
            return _mapper.Map<ProductDetailDto>(_productRepository.GetProductDetailById(id));
        }

        public ProductDetailDto GetDetailBySlug(string slug){
            return _mapper.Map<ProductDetailDto>(_productRepository.GetProductDetailBySlug(slug));
        }

        public IEnumerable<ProductListDto> GetProducts(int page, int pageSize, string searchKey, StatusType status,
            int? companyId, int? createdUserId){
            return _mapper.Map<List<ProductListDto>>(_productRepository.GetProducts(page, pageSize,
                searchKey, status, companyId, createdUserId));
        }

        public IEnumerable<CategoryProductDto> GetProductsByCategoryId(int categoryId, StatusType status){
            return _productRepository.GetProductsByCategoryId(categoryId, status);
        }

        public int CountProducts(string searchKey, StatusType status, int? companyId, int? createdUserId){
            return _productRepository.CountProducts(searchKey, status, companyId, createdUserId);
        }

        public IEnumerable<CompanyProductDto> GetProductsByCompanyId(int id, int page, int pageSize, string searchKey,
            StatusType status){
            return _mapper.Map<List<CompanyProductDto>>(_productRepository.GetProducts(page, pageSize, searchKey,
                status, id));
        }

        public int CountProductsByCompanyId(int id, string searchKey, StatusType status){
            return _productRepository.CountProducts(searchKey, status, id);
        }

        public IEnumerable<UserProductDto> GetProductsByUserId(int id, int page, int pageSize, string searchKey,
            StatusType status){
            return _mapper.Map<List<UserProductDto>>(_productRepository.GetProducts(page, pageSize, searchKey,
                status, null, id));
        }

        public int CountProductsByUserId(int id, string searchKey, StatusType status){
            return _productRepository.CountProducts(searchKey, status, null, id);
        }

        public void AddProduct(ProductViewModel model){
            var registerCommand = _mapper.Map<AddNewProductCommand>(model);
            _bus.SendCommand(registerCommand);
        }

        public void UpdateProduct(ProductViewModel model){
            var updateCommand = _mapper.Map<UpdateProductCommand>(model);
            _bus.SendCommand(updateCommand);
        }

        public void UpdateStatusProduct(ProductStatusViewModel model){
            var updateStatusCommand = _mapper.Map<UpdateStatusProductCommand>(model);
            _bus.SendCommand(updateStatusCommand);
        }

        public void UpdateCategoryProduct(int id, ProductCategoryViewModel model){
            var product = GetDetailById(id);
            if (product == null){
                _bus.RaiseEvent(new DomainNotification("UpdateCategoryProduct", "Product not found"));
                return;
            }

            if (_companyAdminService.IsAdminOfCompany(product.CompanyId, model.UpdatedUserId)){
                var updateCategoryCommand = _mapper.Map<UpdateCategoryProductCommand>(model);
                updateCategoryCommand.Id = id;
                _bus.SendCommand(updateCategoryCommand);
            }
        }

        /// <summary>
        /// Notes:to get product image from id need to go product image repo but this command must be handler
        /// inside of product command handler . In DDD , problem domains must be seperated therefore product image data
        /// got from service and pass as parameter beacause services can inject to other services but  handlers can not.
        /// this is must in all services so you need refactor to all of them :)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="imageId"></param>
        /// <param name="userId"></param>
        public void UpdateProductListImage(int id, int imageId, int userId){
            var productImage = _productImageService.GetById(imageId);
            if (productImage != null){
                _bus.SendCommand(new UpdateProductListImageCommand()
                {
                    Id = id,
                    UpdatedUserId = userId,
                    ListImagePath = productImage.SmallImagePath
                });
            }
            else{
                _bus.RaiseEvent(new DomainNotification("UpdateProductListImage", "Image not found"));
            }
        }

        public void DeleteProduct(int id){
            var updateCommand = new DeleteProductCommand() {Id = id};
            _bus.SendCommand(updateCommand);
        }


        public void Dispose(){
            GC.SuppressFinalize(this);
        }
    }
}