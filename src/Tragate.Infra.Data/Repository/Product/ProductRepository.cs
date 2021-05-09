using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using Tragate.Common.Library;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Dto.Tag;
using Tragate.Common.Library.Enum;
using Tragate.Common.Library.Helpers;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Core.Notifications;
using Tragate.Domain.Interfaces;
using Tragate.Domain.Models;
using Tragate.Infra.Data.Context;
using Tragate.Infra.Data.Repository;

namespace Tragate.Infra.Data
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly IMediatorHandler _bus;
        private readonly IDbConnection _db;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ConfigSettings _settings;


        public ProductRepository(TragateContext context,
            IMediatorHandler bus,
            IDbConnection db,
            ICategoryRepository categoryRepository,
            IOptions<ConfigSettings> settings
        ) : base(context){
            _bus = bus;
            _db = db;
            _categoryRepository = categoryRepository;
            _settings = settings.Value;
        }


        public Product GetProductIdByUuId(Guid uuid){
            return Db.Product.Single(x => x.UuId == uuid);
        }

        public List<CategoryProductDto> GetProductsByCategoryId(int categoryId, StatusType status){
            var sb = new StringBuilder();
            sb.Append($@"SELECT
                      p.*,
                      cat.*,
                      pm.ParameterCode    as Id,
                      pm.ParameterValue1  as Value
                    FROM Product p
                      INNER JOIN Category cat ON cat.Id = p.CategoryId
                      LEFT JOIN Parameter pm on pm.ParameterType = 'CurrencyId' and pm.ParameterCode = p.CurrencyId
                  WHERE cat.Id = {categoryId}");
            if (status != StatusType.All)
                sb.AppendLine($"and p.StatusId = {(int) status}");

            return _db.Query(sb.ToString(),
                new[]
                {
                    typeof(CategoryProductDto), typeof(CategoryDto), typeof(ParameterDto)
                }, (objects) =>
                {
                    var product = (CategoryProductDto) objects[0];
                    product.CategoryId = ((CategoryDto) objects[1]).Id;
                    product.CategoryTitle = ((CategoryDto) objects[1]).Title;
                    product.Currency = ((ParameterDto) objects[2])?.Value;
                    product.ListImagePath = product.ListImagePath.CheckProductProfileImage();
                    return product;
                }).ToList();
        }

        public ProductDto GetProductById(int id){
            var sql = $@"select
                          p.*,
                          c.*,
                          t.*,
                          pm.ParameterCode  as Id, pm.ParameterValue1 as Value,
                          pm2.ParameterCode as Id, pm2.ParameterValue1 as Value
                        from Product p
                          inner join Company c on c.UserId = p.CompanyId
                          left join ProductTag pt on p.Id = pt.ProductId
                          left join Tag t on t.Id = pt.TagId
                          left join Parameter pm on pm.ParameterType = 'CurrencyId'         and pm.ParameterCode = p.CurrencyId
                          left join Parameter pm2 on pm2.ParameterType = 'UnitTypeId'         and pm2.ParameterCode = p.UnitTypeId
                        where p.Id = {id}";

            var result = _db.Query<ProductDto, CompanyDto, TagDto, ParameterDto, ParameterDto, ProductDto>(sql,
                (p, c, t, pm, pm2) =>
                {
                    p.ProductId = p.Id;
                    p.Tag = t?.Slug;
                    p.CompanyId = c.UserId;
                    p.Currency = pm?.Value;
                    p.UnitType = pm2?.Value;
                    p.ListImagePath = p.ListImagePath.CheckProductProfileImage();
                    return p;
                }).ToList();

            var product = result.First();
            var categoryPath = _categoryRepository
                .GetCategoryPathById(product.CategoryId)
                .Select(x => x.Slug).ToArray();

            product.Tags = result.Select(x => x.Tag).ToArray();
            product.CategoryText = product.CategoryPath = string.Join("/", categoryPath);
            GetNestedProductCategory(product, product.CategoryPath);

            return product;
        }

        private void GetNestedProductCategory(ProductDto product, string categoryPath){
            var orderedCategory = categoryPath.Split('/').Reverse();
            var tree = new CategoryTree();
            var node = orderedCategory.Aggregate<string, CategoryNodeDto>(null,
                (current, item) => tree.Insert(current, item));

            product.CategoryTree = node;
        }

        public ProductDto GetProductDetailById(int id){
            var sql = $@"select
                              p.*, c.*,u.*,l.*,l2.*,
                              pm.ParameterCode  as Id, pm.ParameterValue1 as Value,
                              pm2.ParameterCode as Id, pm2.ParameterValue1 as Value,
                              pm3.ParameterCode as Id, pm3.ParameterValue1 as Value,
                              pm4.ParameterCode as Id, pm4.ParameterValue1 as Value,
                              pm5.ParameterCode as Id, pm5.ParameterValue1 as Value
                        from Product p
                          inner join Company c on c.UserId = p.CompanyId
                          inner join [User] u on u.Id = c.UserId
                          inner join Location l on l.Id = u.LocationId
                          inner join Location l2 on l2.Id = p.OriginLocationId
                          inner join Parameter pm  on pm.ParameterType  = 'MembershipTypeId'   and pm.ParameterCode  = c.MembershipTypeId
                          inner join Parameter pm2 on pm2.ParameterType = 'VerificationTypeId' and pm2.ParameterCode = c.VerificationTypeId
                          left join Parameter pm3 on pm3.ParameterType = 'CurrencyId'         and pm3.ParameterCode = p.CurrencyId
                          left join Parameter pm4 on pm4.ParameterType = 'UnitTypeId'         and pm4.ParameterCode = p.UnitTypeId
                          inner join Parameter pm5 on pm5.ParameterType = 'StatusId'           and pm5.ParameterCode = p.StatusId
                        where p.Id = {id}";


            var result = _db.Query(sql, new[]
            {
                typeof(ProductDto), typeof(CompanyDto), typeof(UserDto), typeof(LocationDto), typeof(LocationDto),
                typeof(ParameterDto), typeof(ParameterDto), typeof(ParameterDto), typeof(ParameterDto),
                typeof(ParameterDto)
            }, (objects) =>
            {
                var p = (ProductDto) objects[0];
                p.Company = (CompanyDto) objects[1];
                p.Company.User = (UserDto) objects[2];
                p.Company.User.Location = (LocationDto) objects[3];
                p.OriginLocation = ((LocationDto) objects[4]).Name;
                p.Company.MembershipType = (objects[5] as ParameterDto)?.Value;
                p.Company.VerificationType = (objects[6] as ParameterDto)?.Value;
                p.Currency = (objects[7] as ParameterDto)?.Value;
                p.UnitType = (objects[8] as ParameterDto)?.Value;
                p.Status = (objects[9] as ParameterDto)?.Value;
                return p;
            }).FirstOrDefault();

            if (result != null){
                result.Tags = _db.Query<string>($@"select T.Slug from ProductTag pt
                  inner join Tag T on pt.TagId = T.Id
                  where pt.ProductId = {id}").ToArray();

                result.Images = _db.Query<ProductImageDetailDto>($@"select pi.Id,pi.SmallImagePath as Path
                from ProductImage pi where pi.ProductId = {id}").Select(x => new ProductImageDetailDto()
                {
                    Id = x.Id,
                    Path = x.Path.CheckProductProfileImage()
                }).ToList();

                if (!result.Images.Any()){
                    result.Images = new List<ProductImageDetailDto>()
                    {
                        new ProductImageDetailDto() {Id = 0, Path = _settings.S3.CDN + "items/product.jpg"}
                    };
                }

                result.Category = _categoryRepository.GetCategoryPathById(result.CategoryId);
            }

            return result;
        }

        public ProductDto GetProductDetailBySlug(string slug){
            var sql = $@"select
                              p.*, c.*,u.*,l.*,l2.*,
                              pm.ParameterCode  as Id, pm.ParameterValue1 as Value,
                              pm2.ParameterCode as Id, pm2.ParameterValue1 as Value,
                              pm3.ParameterCode as Id, pm3.ParameterValue1 as Value,
                              pm4.ParameterCode as Id, pm4.ParameterValue1 as Value,
                              pm5.ParameterCode as Id, pm5.ParameterValue1 as Value
                        from Product p
                          inner join Company c on c.UserId = p.CompanyId
                          inner join [User] u on u.Id = c.UserId
                          inner join Location l on l.Id = u.LocationId
                          inner join Location l2 on l2.Id = p.OriginLocationId
                          inner join Parameter pm  on pm.ParameterType  = 'MembershipTypeId'   and pm.ParameterCode  = c.MembershipTypeId
                          inner join Parameter pm2 on pm2.ParameterType = 'VerificationTypeId' and pm2.ParameterCode = c.VerificationTypeId
                          left join Parameter pm3 on pm3.ParameterType = 'CurrencyId'         and pm3.ParameterCode = p.CurrencyId
                          left join Parameter pm4 on pm4.ParameterType = 'UnitTypeId'         and pm4.ParameterCode = p.UnitTypeId
                          inner join Parameter pm5 on pm5.ParameterType = 'StatusId'           and pm5.ParameterCode = p.StatusId
                        where p.Slug = '{slug}'";


            var result = _db.Query(sql, new[]
            {
                typeof(ProductDto), typeof(CompanyDto), typeof(UserDto), typeof(LocationDto), typeof(LocationDto),
                typeof(ParameterDto), typeof(ParameterDto), typeof(ParameterDto), typeof(ParameterDto),
                typeof(ParameterDto)
            }, (objects) =>
            {
                var p = (ProductDto) objects[0];
                p.Company = (CompanyDto) objects[1];
                p.Company.User = (UserDto) objects[2];
                p.Company.User.Location = (LocationDto) objects[3];
                p.OriginLocation = ((LocationDto) objects[4]).Name;
                p.Company.MembershipType = (objects[5] as ParameterDto)?.Value;
                p.Company.VerificationType = (objects[6] as ParameterDto)?.Value;
                p.Currency = (objects[7] as ParameterDto)?.Value;
                p.UnitType = (objects[8] as ParameterDto)?.Value;
                p.Status = (objects[9] as ParameterDto)?.Value;
                return p;
            }).FirstOrDefault();

            if (result != null){
                result.Tags = _db.Query<string>($@"select T.Slug from Product P
                            inner join ProductTag Tag on P.Id = Tag.ProductId
                            inner join Tag T on Tag.TagId = T.Id
                            where P.Slug = '{slug}'").ToArray();

                result.Images = _db.Query<ProductImageDetailDto>($@"select I.Id,I.SmallImagePath as Path from Product P
                            inner join ProductImage I on P.Id = I.ProductId
                            where P.Slug = '{slug}'").Select(x => new ProductImageDetailDto()
                {
                    Id = x.Id,
                    Path = x.Path.CheckProductProfileImage()
                }).ToList();

                if (!result.Images.Any()){
                    result.Images = new List<ProductImageDetailDto>()
                    {
                        new ProductImageDetailDto() {Id = 0, Path = _settings.S3.CDN + "items/product.jpg"}
                    };
                }

                result.Category = _categoryRepository.GetCategoryPathById(result.CategoryId);
            }

            return result;
        }

        public List<ProductDto> GetProducts(int page, int pageSize, string searchKey, StatusType status,
            int? companyId = null, int? userId = null){
            page -= 1;

            var sb = new StringBuilder();
            const string sql = @"SELECT
                  p.*, c.*, u.*, u2.*, cat.*,
                  pm.ParameterCode  as Id, pm.ParameterValue1 as Value,
                  pm2.ParameterCode as Id, pm2.ParameterValue1 as Value,
                  pm3.ParameterCode as Id, pm3.ParameterValue1 as Value,
                  pm4.ParameterCode as Id, pm4.ParameterValue1 as Value
                FROM Company c
                      INNER JOIN [User] u on u.Id = c.UserId
                      INNER JOIN Product p on c.UserId = p.CompanyId
                      INNER JOIN [User] u2 ON u2.Id = p.CreatedUserId
                      INNER JOIN Category cat ON cat.Id = p.CategoryId
                      INNER JOIN Parameter pm  on pm.ParameterType  = 'MembershipTypeId'   and pm.ParameterCode  = c.MembershipTypeId
                      LEFT JOIN Parameter pm2 on pm2.ParameterType = 'CurrencyId'         and pm2.ParameterCode = p.CurrencyId
                      LEFT JOIN Parameter pm3 on pm3.ParameterType = 'UnitTypeId'         and pm3.ParameterCode = p.UnitTypeId
                      INNER JOIN Parameter pm4 on pm4.ParameterType = 'StatusId'           and pm4.ParameterCode = p.StatusId
                  WHERE 1=1";

            sb.AppendLine(sql);

            if (companyId.HasValue)
                sb.AppendLine($"and c.UserId = {companyId} ");

            if (!string.IsNullOrEmpty(searchKey))
                sb.AppendLine($"and p.Title like '%{searchKey}%'");

            if (status != StatusType.All)
                sb.AppendLine($"and p.StatusId = {(int) status}");

            if (userId.HasValue)
                sb.AppendLine($"and p.CreatedUserId = {userId}");

            sb.AppendLine($"ORDER BY p.Id DESC OFFSET {page * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY");

            return _db.Query(sb.ToString(),
                new[]
                {
                    typeof(ProductDto), typeof(CompanyDto), typeof(UserDto), typeof(UserDto), typeof(CategoryDto),
                    typeof(ParameterDto), typeof(ParameterDto), typeof(ParameterDto), typeof(ParameterDto)
                }, (objects) =>
                {
                    var product = (ProductDto) objects[0];
                    product.Company = (CompanyDto) objects[1];
                    product.Company.User = (UserDto) objects[2];
                    product.CreatedUser = ((UserDto) objects[3]).FullName;
                    product.CategoryId = ((CategoryDto) objects[4]).Id;
                    product.CategoryTitle = ((CategoryDto) objects[4]).Title;
                    product.Company.MembershipType = ((ParameterDto) objects[5]).Value;
                    product.Currency = ((ParameterDto) objects[6])?.Value;
                    product.UnitType = ((ParameterDto) objects[7])?.Value;
                    product.Status = ((ParameterDto) objects[8]).Value;
                    product.ListImagePath = product.ListImagePath.CheckProductProfileImage();
                    return product;
                }).ToList();
        }

        public int CountProducts(string searchKey, StatusType status, int? companyId = null, int? userId = null){
            var sb = new StringBuilder();
            const string sql = @"SELECT count(*) FROM Product p where 1=1";

            sb.AppendLine(sql);

            if (companyId.HasValue)
                sb.AppendLine($"and p.CompanyId = {companyId} ");

            if (!string.IsNullOrEmpty(searchKey))
                sb.AppendLine($"and p.Title like '%{searchKey}%'");

            if (status != StatusType.All)
                sb.AppendLine($"and p.StatusId = {(int) status}");

            if (userId.HasValue)
                sb.AppendLine($"and p.CreatedUserId = {userId}");


            return _db.ExecuteScalar<int>(sb.ToString());
        }


        //TODO:Eventualy consistency yada UoW kapsamında düşünmek lazım
        public bool Add(Product entity, IEnumerable<int> tagIds){
            var transaction = Db.Database.BeginTransaction(IsolationLevel.ReadUncommitted);
            try{
                Db.Product.Add(entity);
                Db.SaveChanges();

                entity.Slug = $"{entity.Title.GenerateSlug()}-{entity.Id}";
                Db.Product.Update(entity);

                tagIds.ToList().ForEach(x => Db.ProductTag.Add(new ProductTag()
                {
                    ProductId = entity.Id,
                    TagId = x
                }));

                Db.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception ex){
                _bus.RaiseEvent(new DomainNotification("Product.Add",
                    "We had a problem during saving your data."));
                Log.Error(ex, "An error occured when product insert transaction !");
                transaction.Rollback();
            }

            return false;
        }

        //TODO:Eventualy consistency yada UoW kapsamında düşünmek lazım
        public bool Update(Product entity, IEnumerable<int> tagIds){
            var transaction = Db.Database.BeginTransaction(IsolationLevel.ReadUncommitted);
            try{
                Db.Product.Update(entity);
                if (tagIds.Any()){
                    Db.ProductTag.RemoveRange(Db.ProductTag.Where(x => x.ProductId == entity.Id).ToList());
                    tagIds.ToList().ForEach(x => Db.ProductTag.Add(new ProductTag()
                    {
                        ProductId = entity.Id,
                        TagId = x
                    }));
                }

                Db.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception ex){
                _bus.RaiseEvent(new DomainNotification("Product.Update",
                    "We had a problem during saving your data."));
                Log.Error(ex, "An error occured when product update transaction !");
                transaction.Rollback();
            }

            return false;
        }
    }
}