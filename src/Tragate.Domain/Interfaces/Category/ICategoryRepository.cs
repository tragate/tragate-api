using System.Collections.Generic;
using System.Linq;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Models;

namespace Tragate.Domain.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        IQueryable<Category> GetCategories(StatusType status, int? parentId, string slug);
        List<CategoryTreeDto> GetCategoryPathById(int id);
        List<CategoryTreeDto> GetCategoryPathBySlug(string slug);
        List<GroupCategoryDto> GetCategoryGroup();
        List<RootCategoryDto> GetSubCategoryGroup(int[] ids, int pageSize);
        Category GetCategoryBySlug(string slug);
    }
}