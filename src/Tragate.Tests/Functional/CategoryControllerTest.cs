using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Tragate.Application;
using Tragate.Application.ViewModels;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;
using Tragate.CrossCutting.Bus;
using Tragate.Domain.Core.Events;
using Tragate.Domain.Core.Notifications;
using Tragate.Domain.Interfaces;
using Tragate.WebApi.Controllers;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Tragate.Tests.Functional
{
    public class CategoryControllerTest : TestBase
    {
        private INotificationHandler<DomainNotification> _notifications;

        private CategoryController GetCategoryController(){
            var serviceProvider = base.BuildServiceProvider();
            var bus = new InMemoryBus(serviceProvider.GetService<IEventStore>(),
                serviceProvider.GetService<IMediator>());

            var mapper = serviceProvider.GetService<IMapper>();
            var categoryRepository = serviceProvider.GetService<ICategoryRepository>();
            _notifications = serviceProvider.GetService<INotificationHandler<DomainNotification>>();
            var productService = serviceProvider.GetService<IProductService>();

            var controller =
                new CategoryController(_notifications, new CategoryService(mapper, bus, categoryRepository),
                    productService);

            return controller;
        }

        [Theory]
        [InlineData(1, null)]
        [InlineData(2, "agriculture")]
        [InlineData(null, null)]
        public void Get_Categories_By_ParentId_And_Status_As_Success(int? parentId, string slug){
            var actionResult = GetCategoryController()
                .GetCategories((int) StatusType.Active, parentId, slug);
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
            Assert.AreEqual(null, ((actionResult as OkObjectResult)?.Value as dynamic)?.Message);
            Assert.AreEqual(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);
            Assert.IsFalse(((DomainNotificationHandler) _notifications).HasNotifications());
        }

        [Fact]
        public void Get_Category_Group_As_Success(){
            var actionResult = GetCategoryController().GetCategoryGroup();
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
            Assert.AreEqual(null, ((actionResult as OkObjectResult)?.Value as dynamic)?.Message);
            Assert.AreEqual(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);
            Assert.IsFalse(((DomainNotificationHandler) _notifications).HasNotifications());
        }

        [Fact]
        public void Get_Category_Group_Should_Be_Return_List_Of_Group_Category_Dto(){
            var actionResult = GetCategoryController().GetCategoryGroup();
            Assert.IsInstanceOfType(
                ((actionResult as OkObjectResult)?.Value as dynamic)?.Data as List<GroupCategoryDto>,
                typeof(List<GroupCategoryDto>));
        }

        [Fact]
        public void Get_Category_Group_Should_Be_12_Items(){
            var actionResult = GetCategoryController().GetCategoryGroup();
            var result = ((actionResult as OkObjectResult)?.Value as dynamic)?.Data as List<GroupCategoryDto>;
            Assert.AreEqual(12, result?.Count);
        }

        [Fact]
        public void Get_Category_Group_Should_Be_Filled_All_Field(){
            var actionResult = GetCategoryController().GetCategoryGroup();
            if (!(((actionResult as OkObjectResult)?.Value as dynamic)?.Data is List<GroupCategoryDto> result)) return;
            foreach (var group in result){
                Assert.IsNotNull(@group.CategoryTitle);
                foreach (var category in @group.CategoryList){
                    Assert.IsNotNull(category.CategoryTitle);
                    Assert.IsNotNull(category.ImagePath);

                    foreach (var subcategory in category.SubCategoryList){
                        Assert.IsNotNull(subcategory.CategoryTitle);
                        Assert.IsNotNull(subcategory.ImagePath);
                    }
                }
            }
        }

        [Theory]
        [InlineData(new[] {1, 2, 4}, 5)]
        public void Get_Sub_Category_Group_As_Success(int[] parentIds, int pageSize){
            var actionResult = GetCategoryController().GetSubCategoryGroup(parentIds, pageSize);
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
            Assert.AreEqual(null, ((actionResult as OkObjectResult)?.Value as dynamic)?.Message);
            Assert.AreEqual(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);
            Assert.IsFalse(((DomainNotificationHandler) _notifications).HasNotifications());
        }

        [Theory]
        [InlineData(new[] {1, 2, 4}, 5)]
        public void Get_Sub_Category_Group_Should_Be_Return_List_Of_Sub_Root_Category_Dto(int[] parentIds,
            int pageSize){
            var actionResult = GetCategoryController().GetSubCategoryGroup(parentIds, pageSize);
            Assert.IsInstanceOfType(
                ((actionResult as OkObjectResult)?.Value as dynamic)?.Data as List<RootCategoryDto>,
                typeof(List<RootCategoryDto>));
        }

        [Theory]
        [InlineData(new[] {1, 2, 4}, 5)]
        public void Get_Sub_Category_Group_Should_Be_Filled_All_Field(int[] parentIds, int pageSize){
            var actionResult = GetCategoryController().GetSubCategoryGroup(parentIds, pageSize);
            if (!(((actionResult as OkObjectResult)?.Value as dynamic)?.Data is List<RootCategoryDto> result)) return;
            foreach (var category in result){
                Assert.IsNotNull(category.CategoryTitle);
                Assert.IsNotNull(category.ImagePath);

                foreach (var subcategory in category.SubCategoryList){
                    Assert.IsNotNull(subcategory.CategoryTitle);
                    Assert.IsNotNull(subcategory.ImagePath);
                }
            }
        }

        [Theory]
        [InlineData(new[] {1, 2, 4}, 5)]
        public void Get_Sub_CategoryList_Max_Count_Should_Be_5_Items(int[] parentIds, int pageSize){
            var actionResult = GetCategoryController().GetSubCategoryGroup(parentIds, pageSize);
            if (!(((actionResult as OkObjectResult)?.Value as dynamic)?.Data is List<RootCategoryDto> result)) return;
            foreach (var category in result){
                Assert.IsFalse(category.SubCategoryList.Count > 5);
            }
        }

        [Theory]
        [InlineData(1)]
        public void Get_Category_By_Id_As_Success(int id){
            var actionResult = GetCategoryController().GetCategoryById(id);
            actionResult.Should().BeOfType<OkObjectResult>();

            Assert.AreEqual(null, ((actionResult as OkObjectResult)?.Value as dynamic)?.Message);
            Assert.AreEqual(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);

            if (!(((actionResult as OkObjectResult)?.Value as dynamic)?.Data is CategoryDto result)) return;
            result.Id.Should().Be(1);
            result.ParentId.Should().BeNull();
            result.Title.Should().Be("Agriculture");
            result.Slug.Should().Be("agriculture");
            result.MetaKeyword.Should().BeNull();
            result.StatusId.Should().Be(3);
            result.Priority.Should().Be(1);
        }

        [Fact]
        public void Add_Category_Should_Be_Failed_If_Model_Is_Null(){
            var model = new CategoryViewModel();
            var actionResult = GetCategoryController().AddCategory(model);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            Assert.IsTrue(((DomainNotificationHandler) _notifications).HasNotifications());
        }

        [Fact]
        public void Add_Category_Should_Be_Failed_If_Title_Is_Null(){
            var model = new CategoryViewModel()
            {
                ParentId = 1,
                StatusId = 3
            };
            var actionResult = GetCategoryController().AddCategory(model);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            Assert.AreEqual(HttpStatusCode.BadRequest,
                (HttpStatusCode) ((BadRequestObjectResult) actionResult).StatusCode);
            Assert.IsTrue(((DomainNotificationHandler) _notifications).HasNotifications());
            Assert.AreEqual("Please ensure you have entered the Title",
                ((DomainNotificationHandler) _notifications).GetNotifications().First().Value);
        }

        [Fact]
        public void Add_Category_Should_Be_Failed_If_StatusId_Zero(){
            var model = new CategoryViewModel()
            {
                Title = "test",
                StatusId = 0
            };
            var actionResult = GetCategoryController().AddCategory(model);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            Assert.AreEqual(HttpStatusCode.BadRequest,
                (HttpStatusCode) ((BadRequestObjectResult) actionResult).StatusCode);
            Assert.IsTrue(((DomainNotificationHandler) _notifications).HasNotifications());
            Assert.AreEqual("Status shouldn't be 0",
                ((DomainNotificationHandler) _notifications).GetNotifications().First().Value);
        }

        [Fact]
        public void Add_Category_Should_Be_Failed_If_Equal_Id_And_ParentId(){
            var model = new CategoryViewModel()
            {
                Title = "test",
                StatusId = (int) StatusType.Active,
                Id = 1,
                ParentId = 1
            };
            var actionResult = GetCategoryController().AddCategory(model);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            Assert.AreEqual(HttpStatusCode.BadRequest,
                (HttpStatusCode) ((BadRequestObjectResult) actionResult).StatusCode);
            Assert.IsTrue(((DomainNotificationHandler) _notifications).HasNotifications());
            Assert.AreEqual("The root of category shouldn't be itself",
                ((DomainNotificationHandler) _notifications).GetNotifications().First().Value);
        }
    }
}