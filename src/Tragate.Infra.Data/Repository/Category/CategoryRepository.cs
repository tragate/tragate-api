using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using FluentValidation.Results;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;
using Tragate.Common.Library.Helpers;
using Tragate.Domain.Interfaces;
using Tragate.Domain.Models;
using Tragate.Infra.Data.Context;
using Tragate.Infra.Data.Repository;

namespace Tragate.Infra.Data
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly IDbConnection _db;

        public CategoryRepository(
            IDbConnection db,
            TragateContext context) : base(context){
            _db = db;
        }

        /// <summary>
        /// parentId is default but if give a slug at the sametime then parentId overrites by Id which is the fetched category
        /// Id by slug from db.(Not : just valid If give together fields (parentId and slug))
        /// </summary>
        /// <param name="status"></param>
        /// <param name="parentId"></param>
        /// <param name="slug"></param>
        /// <returns></returns>
        public IQueryable<Category> GetCategories(StatusType status, int? parentId, string slug){
            if (!string.IsNullOrEmpty(slug))
                parentId = Db.Category.First(x => x.Slug == slug.Trim()).Id;

            var categories = status != StatusType.All
                ? Db.Category.Where(x => x.ParentId == parentId && x.StatusId == (byte) status)
                : Db.Category.Where(x => x.ParentId == parentId);
            return categories.OrderBy(p => p.Priority);
        }

        public List<CategoryTreeDto> GetCategoryPathById(int id){
            string sql = $@"select * from ufnGetCategoryPath({id})";
            return _db.Query<CategoryTreeDto>(sql).Reverse().ToList();
        }

        public List<CategoryTreeDto> GetCategoryPathBySlug(string slug){
            string sql = $@"select * from ufnGetCategoryPathBySlug('{slug}')";
            return _db.Query<CategoryTreeDto>(sql).Reverse().ToList();
        }

        public List<GroupCategoryDto> GetCategoryGroup(){
            string sql =
                $@"SELECT
                        p.ParameterValue1 AS CategoryGroup,
                        p.ParameterValue2 AS CategoryGroupImagePath,
                        c.Title           AS CategoryTitle,
                        sc.Title          AS SubCategoryTitle,
                        c.ImagePath AS CategoryImagePath,
                        sc.ImagePath AS SubCategoryImagePath,
                        c.Slug as CategorySlug,
                        sc.Slug as SubCategorySlug
                    FROM CategoryGroup cg
                    INNER JOIN Category c ON c.Id = cg.CategoryId
                    INNER JOIN Parameter
                                p ON p.ParameterType = 'CategoryGroupId' 
                                AND cg.CategoryGroupId = p.ParameterCode
                    CROSS APPLY (SELECT TOP 6 *
                                FROM Category
                                WHERE ParentId = c.Id ORDER BY Priority) AS sc
                    WHERE 
                        c.StatusId = 3
                        AND sc.StatusId = 3
                        ORDER BY p.Priority";

            return _db.Query<dynamic>(sql).Select(x => new GroupCategoryDto()
            {
                CategoryGroup = x.CategoryGroup,
                CategoryTitle = x.CategoryTitle,
                SubCategoryTitle = x.SubCategoryTitle,
                CategoryImagePath = x.CategoryImagePath,
                SubCategoryImagePath = x.SubCategoryImagePath,
                CategoryGroupImagePath = x.CategoryGroupImagePath,
                CategorySlug = x.CategorySlug,
                SubCategorySlug = x.SubCategorySlug
            }).GroupBy(x => x.CategoryGroup).Select(p => new GroupCategoryDto()
            {
                CategoryTitle = p.Key,
                CategoryGroupImagePath = p.FirstOrDefault().CategoryGroupImagePath,
                CategoryList = p.GroupBy(x => x.CategoryTitle).Select(x => new RootCategoryDto()
                {
                    CategoryTitle = x.Key,
                    ImagePath = x.FirstOrDefault()?.CategoryImagePath.CheckCategoryProfileImage(),
                    Slug = x.FirstOrDefault()?.CategorySlug,
                    SubCategoryList = x.Select(c => new SubCategoryDto()
                    {
                        CategoryTitle = c.SubCategoryTitle,
                        ImagePath = c.SubCategoryImagePath.CheckCategoryProfileImage(),
                        Slug = c.SubCategorySlug
                    }).ToList()
                }).ToList()
            }).ToList();
        }

        public List<RootCategoryDto> GetSubCategoryGroup(int[] ids, int pageSize){
            string sql = $@"SELECT
                        c.Title  AS CategoryTitle,
                        sc.Title AS SubCategoryTitle,
                        c.ImagePath AS CategoryImagePath,
                        sc.ImagePath AS SubCategoryImagePath,
                        c.Slug as CategorySlug,
                        sc.Slug as SubCategorySlug,
                        c.MetaDescription as CategoryDescription
                    FROM Category c
                    CROSS APPLY (SELECT TOP {pageSize} *
                                FROM Category
                                WHERE ParentId = c.Id 
                                ORDER BY Priority) AS sc
                    WHERE sc.ParentId IN ({string.Join(",", ids)}) 
                    AND c.StatusId = 3 AND sc.StatusId = 3
                    Order By sc.Priority";

            return _db.Query<dynamic>(sql).Select(x => new GroupCategoryDto()
            {
                CategoryTitle = x.CategoryTitle,
                CategoryImagePath = x.CategoryImagePath,
                SubCategoryTitle = x.SubCategoryTitle,
                SubCategoryImagePath = x.SubCategoryImagePath,
                CategorySlug = x.CategorySlug,
                SubCategorySlug = x.SubCategorySlug,
                CategoryDescription = x.CategoryDescription
            }).GroupBy(x => x.CategoryTitle).Select(p => new RootCategoryDto()
            {
                CategoryTitle = p.Key,
                ImagePath = p.FirstOrDefault()?.CategoryImagePath.CheckCategoryProfileImage(),
                Slug = p.FirstOrDefault()?.CategorySlug,
                Description = p.FirstOrDefault().CategoryDescription,
                SubCategoryList = p.Select(c => new SubCategoryDto()
                {
                    CategoryTitle = c.SubCategoryTitle,
                    ImagePath = c.SubCategoryImagePath.CheckCategoryProfileImage(),
                    Slug = c.SubCategorySlug
                }).ToList()
            }).ToList();
        }

        public Category GetCategoryBySlug(string slug){
            return _db.Query<Category>($"select * from category (nolock) where Slug = '{slug}' ").FirstOrDefault();
        }
    }
}