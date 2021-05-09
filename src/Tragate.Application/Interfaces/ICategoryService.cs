using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Tragate.Application.ViewModels;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;

namespace Tragate.Application
{
    public interface ICategoryService : IDisposable
    {
        IQueryable<CategoryDto> GetCategories(StatusType status, int? parentId, string slug);

        List<CategoryTreeDto> GetCategoryPathById(int id);

        List<CategoryTreeDto> GetCategoryPathBySlug(string slug);

        List<GroupCategoryDto> GetCategoryGroup();

        List<RootCategoryDto> GetSubCategoryGroup(int[] ids, int pageSize);

        CategoryDto GetCategoryById(int id);

        CategoryDto GetCategoryBySlug(string slug);

        void AddCategory(CategoryViewModel model);

        void UpdateCategory(CategoryViewModel model);

        void UploadImage(IFormFile files, int id);
    }
}