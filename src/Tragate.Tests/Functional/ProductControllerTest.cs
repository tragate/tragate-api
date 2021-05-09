using System;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Tragate.Application;
using Tragate.Application.ViewModels;
using Tragate.Common.Library;
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
    public class ProductControllerTest : TestBase
    {
        private INotificationHandler<DomainNotification> _notifications;
        private IMapper _mapper;

        private ProductController GetProductController(){
            var serviceProvider = base.BuildServiceProvider();
            var bus = new InMemoryBus(serviceProvider.GetService<IEventStore>(),
                serviceProvider.GetService<IMediator>());

            _mapper = serviceProvider.GetService<IMapper>();
            var settings = serviceProvider.GetService<IOptions<ConfigSettings>>();
            var productRepository = serviceProvider.GetService<IProductRepository>();
            _notifications = serviceProvider.GetService<INotificationHandler<DomainNotification>>();
            var companyAdminService = serviceProvider.GetService<ICompanyAdminService>();
            var productImageService = serviceProvider.GetService<IProductImageService>();
            var controller = new ProductController(_notifications,
                new ProductService(productRepository, _mapper, bus, companyAdminService, productImageService),
                settings);

            return controller;
        }


        [Theory]
        [InlineData(71)]
        public void Get_Product_Detail_By_Id_As_Success(int id){
            //Act
            var actionResult = GetProductController().GetDetailById(id);

            //Assert
            actionResult.Should().BeOfType<OkObjectResult>();
            Assert.AreEqual(null, ((actionResult as OkObjectResult)?.Value as dynamic)?.Message);
            Assert.AreEqual(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);

            ProductDetailDto productDetail = ((actionResult as OkObjectResult)?.Value as dynamic)?.Data;
            Assert.IsNotNull(productDetail);
        }


//        [Fact]
//        public void Add_Product_Should_Be_Sucess(){
//            //Arrange
//            var model = new ProductViewModel()
//            {
//                UuId = Guid.NewGuid(),
//                Title = "test product",
//                PriceLow = 10,
//                PriceHigh = 20,
//                OriginLocationId = 1,
//                CategoryId = 2,
//                CompanyId = 526,
//                StatusId = (int) StatusType.Active,
//                CreatedUserId = 495
//            };
//
//            //Act
//            var actionResult = GetProductController().AddProduct(model);
//            
//            //Assert
//            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
//        }


        [Fact]
        public void Add_Product_Should_Be_Failed_If_Reguired_Fields_Are_Null(){
            //Arrange
            var model = new ProductViewModel() {UuId = Guid.NewGuid()};
            var errorMessageList = new[]
            {
                "Please ensure you have entered the Title",
                "Please ensure you have entered the CreatedUserId",
                "'Created User Id' must be greater than '0'.",
                "Please ensure you have entered the CompanyId",
                "'Company Id' must be greater than '0'.",
                "Please ensure you have entered the OriginLocationId",
                "'Origin Location Id' must be greater than '0'.",
                "Please ensure you have entered the CategoryId",
                "'Category Id' must be greater than '0'.",
                "Please ensure you have entered the StatusId",
                "'Status Id' must be greater than '0'."
            };
            //Act
            var actionResult = GetProductController().AddProduct(model);

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            var errors = ((DomainNotificationHandler) _notifications).GetNotifications();
            foreach (var item in errorMessageList){
                Assert.IsTrue(errors.Any(x => x.Value == item));
            }

            Assert.AreEqual(errorMessageList.Length, errors.Count);
        }

        [Fact]
        public void Update_Product_Should_Be_Failed_If_Reguired_Fields_Are_Null(){
            //Arrange
            var model = new ProductViewModel();
            var errorMessageList = new[]
            {
                "Please ensure you have entered the Id",
                "Please ensure you have entered the Title",
                "Please ensure you have entered the UpdatedUserId",
                "'Updated User Id' must be greater than '0'.",
                "Please ensure you have entered the CompanyId",
                "'Company Id' must be greater than '0'.",
                "Please ensure you have entered the OriginLocationId",
                "'Origin Location Id' must be greater than '0'.",
                "Please ensure you have entered the CategoryId",
                "'Category Id' must be greater than '0'.",
                "Please ensure you have entered the StatusId",
                "'Status Id' must be greater than '0'."
            };
            //Act
            var actionResult = GetProductController().UpdateProduct(model);

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            var errors = ((DomainNotificationHandler) _notifications).GetNotifications();
            foreach (var item in errorMessageList){
                Assert.IsTrue(errors.Any(x => x.Value == item));
            }

            Assert.AreEqual(errorMessageList.Length, errors.Count);
        }

        [Theory]
        [InlineData(0)]
        public void Delete_Product_Should_Be_Failed_If_Reguired_Fields_Are_Null(int id){
            //Arrange
            var errorMessageList = new[]
            {
                "Please ensure you have entered the Id"
            };

            //Act
            var actionResult = GetProductController().DeleteProduct(id);

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            var errors = ((DomainNotificationHandler) _notifications).GetNotifications();
            foreach (var item in errorMessageList){
                Assert.IsTrue(errors.Any(x => x.Value == item));
            }

            Assert.AreEqual(errorMessageList.Length, errors.Count);
        }

        [Theory]
        [InlineData(0, 0, 0)]
        public void Update_Status_Product_Should_Be_Failed_If_Reguired_Fields_Are_Null(int id, int statusId,
            int updatedUserId){
            //Arrange
            var errorMessageList = new[]
            {
                "Please ensure you have entered the Id",
                "Please ensure you have entered the StatusId",
                "Please ensure you have entered the UpdatedUserId",
                "'Status Id' must be greater than '0'.",
                "'Updated User Id' must be greater than '0'.",
            };

            //Act
            var actionResult = GetProductController().UpdateStatus(new ProductStatusViewModel()
            {
                Id = id,
                StatusId = statusId,
                UpdatedUserId = updatedUserId
            });

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            var errors = ((DomainNotificationHandler) _notifications).GetNotifications();
            foreach (var item in errorMessageList){
                Assert.IsTrue(errors.Any(x => x.Value == item));
            }

            Assert.AreEqual(errorMessageList.Length, errors.Count);
        }

//        [Fact]
//        public void Update_Category_Product_Should_Be_Validte(){
//            //Arrange
//            var errorMessageList = new[]
//            {
//                "Please ensure you have entered the CategoryId",
//                "Please ensure you have entered the UpdatedUserId",
//            };
//
//            //Act
//            var actionResult = GetProductController().UpdateCategory(99, new ProductCategoryViewModel()
//            {
//                CategoryId = 3,
//                UpdatedUserId = 495
//            });
//
//            //Assert
//            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
//            var errors = ((DomainNotificationHandler) _notifications).GetNotifications();
//            foreach (var item in errorMessageList){
//                Assert.IsTrue(errors.Any(x => x.Value == item));
//            }
//
//            Assert.AreEqual(errorMessageList.Length, errors.Count);
//        }
    }
}