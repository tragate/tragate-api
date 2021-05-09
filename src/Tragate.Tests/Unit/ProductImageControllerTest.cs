using System;
using Moq;
using Xunit;
using Tragate.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Tragate.WebApi.Controllers;
using Tragate.Domain.Core.Notifications;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Tragate.Tests.Unit
{
    public class ProductImageControllerTest : TestBase
    {
        private readonly Mock<IProductImageService> _productImageServiceMock;

        public ProductImageControllerTest(){
            _productImageServiceMock = new Mock<IProductImageService>();
        }


        [Fact]
        public void Should_Be_Verify_Upload_Image_Product(){
            //Arrange
            var controller = new ProductImageController(
                new Mock<DomainNotificationHandler>().Object,
                _productImageServiceMock.Object);

            //Act
            var actionResult =
                controller.UploadImage(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<IFormFileCollection>());

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
            Assert.AreEqual("Images has been uploaded", ((actionResult as OkObjectResult)?.Value as dynamic)?.Message);
            Assert.AreEqual(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);

            _productImageServiceMock.Verify(
                x => x.UploadImages(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<IFormFileCollection>()), Times.Once);
        }

        [Fact]
        public void Should_Be_Verify_Delete_Image_Product(){
            //Arrange
            var controller = new ProductImageController(
                new Mock<DomainNotificationHandler>().Object,
                _productImageServiceMock.Object);

            //Act
            var actionResult = controller.DeleteImage(It.IsAny<int>());

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
            Assert.AreEqual("Images has been deleted", ((actionResult as OkObjectResult)?.Value as dynamic)?.Message);
            Assert.AreEqual(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);

            _productImageServiceMock.Verify(x => x.DeleteImage(It.IsAny<int>()), Times.Once);
        }
    }
}