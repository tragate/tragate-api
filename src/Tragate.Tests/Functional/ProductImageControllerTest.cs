using Moq;
using Xunit;
using System;
using System.IO;
using MediatR;
using System.Linq;
using System.Text;
using AutoMapper;
using Tragate.Application;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Tragate.CrossCutting.Bus;
using Tragate.Domain.Core.Events;
using Tragate.WebApi.Controllers;
using Tragate.Domain.Core.Notifications;
using Microsoft.Extensions.DependencyInjection;
using Tragate.Domain.Interfaces;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Tragate.Tests.Functional
{
    public class ProductImageControllerTest : TestBase
    {
        private INotificationHandler<DomainNotification> _notifications;

        private ProductImageController GetProductImageController(){
            var serviceProvider = base.BuildServiceProvider();
            var bus = new InMemoryBus(serviceProvider.GetService<IEventStore>(),
                serviceProvider.GetService<IMediator>());

            _notifications = serviceProvider.GetService<INotificationHandler<DomainNotification>>();
            var productImageRepository = serviceProvider.GetService<IProductImageRepository>();
            var mapper = serviceProvider.GetService<IMapper>();
            var controller =
                new ProductImageController(_notifications,
                    new ProductImageService(bus, productImageRepository, mapper));
            return controller;
        }


        [Fact]
        public void Upload_Image_Should_Be_Return_BadRequest_When_Given_InCorrect_Params(){
            //Arrange
            var controller = GetProductImageController();
            var dummyFile = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data",
                "dummy.txt");
            var dummyFileList = new FormFileCollection {dummyFile};
            //Act
            var actionResult = controller
                .UploadImage(It.IsAny<Guid>(),
                    It.IsAny<int>(),
                    dummyFileList);

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
        }

        [Fact]
        public void Upload_Image_Should_Be_Fail_If_Files_Count_Greather_Then_10(){
            //Arrange
            var controller = GetProductImageController();
            var fileList = new FormFileCollection();
            for (var i = 1; i <= 11; i++){
                var file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data",
                    "dummy.jpg");
                fileList.Add(file);
            }

            //Act
            var actionResult = controller
                .UploadImage(Guid.NewGuid(),
                    It.IsAny<int>(),
                    fileList);

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            Assert.AreEqual("Files shouldn't be greater than 10",
                ((DomainNotificationHandler) _notifications).GetNotifications().First().Value);
        }

        [Fact]
        public void Upload_Image_Should_Be_Fail_If_Files_Extension_Not_Supported(){
            //Arrange
            var controller = GetProductImageController();
            var fileList = new FormFileCollection();
            var file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("file")), 0, 0, "Data", "dummy.txt");
            fileList.Add(file);

            //Act
            var actionResult = controller
                .UploadImage(Guid.NewGuid(),
                    It.IsAny<int>(),
                    fileList);

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            Assert.AreEqual("File extensions should be one of '.jpg','.jpeg','.png','.bmp','.gif'",
                ((DomainNotificationHandler) _notifications).GetNotifications().First().Value);
        }

        [Fact]
        public void Upload_Image_Should_Be_Fail_If_Uuid_Not_Exists(){
            //Arrange
            var controller = GetProductImageController();
            var fileList = new FormFileCollection();
            var file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("file")), 0, 0, "Data", "dummy.txt");
            fileList.Add(file);

            //Act
            var actionResult = controller
                .UploadImage(It.IsAny<Guid>(),
                    It.IsAny<int>(),
                    fileList);

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            Assert.AreEqual("Please ensure you have entered the UuId",
                ((DomainNotificationHandler) _notifications).GetNotifications().First().Value);
        }

        [Fact]
        public void Delete_Image_Should_Be_Fail_If_Id_Not_Exists(){
            //Arrange
            var controller = GetProductImageController();

            //Act
            var actionResult = controller.DeleteImage(It.IsAny<int>());

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            Assert.AreEqual("'Id' must be greater than '0'.",
                ((DomainNotificationHandler) _notifications).GetNotifications().First().Value);
        }
    }
}