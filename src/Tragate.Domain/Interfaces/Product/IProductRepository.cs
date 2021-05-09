using System;
using System.Collections.Generic;
using Tragate.Common.Library;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Models;

namespace Tragate.Domain.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        ProductDto GetProductById(int id);
        ProductDto GetProductDetailById(int id);
        ProductDto GetProductDetailBySlug(string slug);

        List<ProductDto> GetProducts(int page, int pageSize, string searchKey,
            StatusType status, int? companyId = null, int? userId = null);

        List<CategoryProductDto> GetProductsByCategoryId(int categoryId, StatusType status);

        int CountProducts(string searchKey, StatusType status, int? companyId = null, int? userId = null);
        Product GetProductIdByUuId(Guid uuid);
        bool Add(Product entity, IEnumerable<int> tagIds);
        bool Update(Product entity, IEnumerable<int> tagIds);
    }
}