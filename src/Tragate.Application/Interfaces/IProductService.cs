using System;
using System.Collections.Generic;
using Nest;
using Tragate.Application.ViewModels;
using Tragate.Common.Library;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;

namespace Tragate.Application
{
    public interface IProductService : IDisposable
    {
        ProductDetailDto GetDetailById(int id);
        ProductDetailDto GetDetailBySlug(string slug);

        IEnumerable<ProductListDto> GetProducts(int page, int pageSize,
            string searchKey, StatusType status, int? companyId, int? createdUserId);

        IEnumerable<CategoryProductDto> GetProductsByCategoryId(int categoryId,StatusType status);

        int CountProducts(string searchKey, StatusType status, int? companyId, int? createdUserId);

        IEnumerable<CompanyProductDto> GetProductsByCompanyId(int id, int page, int pageSize,
            string searchKey, StatusType status);

        int CountProductsByCompanyId(int id, string searchKey, StatusType status);

        IEnumerable<UserProductDto> GetProductsByUserId(int id, int page, int pageSize,
            string searchKey, StatusType status);

        int CountProductsByUserId(int id, string searchKey, StatusType status);


        void AddProduct(ProductViewModel model);
        void UpdateProduct(ProductViewModel model);
        void UpdateStatusProduct(ProductStatusViewModel model);
        void DeleteProduct(int id);
        void UpdateCategoryProduct(int id, ProductCategoryViewModel model);
        void UpdateProductListImage(int id, int imageId, int userId);
    }
}