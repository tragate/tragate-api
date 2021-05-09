using System.Collections.Generic;
using System.Linq;
using Moq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Tragate.Application;
using Tragate.Application.ViewModels;
using Tragate.Common.Library;
using Tragate.Common.Result;
using Tragate.Domain.Core.Notifications;
using Tragate.WebApi.Controllers;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Tragate.Tests.Unit
{
    public class ProductControllerTest : TestBase
    {
        private readonly Mock<IProductService> _productServiceMock;
        private readonly IOptions<ConfigSettings> _settingsMock;

        public ProductControllerTest(){
            _productServiceMock = new Mock<IProductService>();

            var provider = base.BuildServiceProvider();
            _settingsMock = provider.GetService<IOptions<ConfigSettings>>();
        }

        [Fact]
        public void Should_Be_Get_Product_Detail_By_Id(){
            //Arrange
            var controller = new ProductController(
                new Mock<DomainNotificationHandler>().Object,
                _productServiceMock.Object,
                _settingsMock);
            //Act
            var actionResult = controller.GetDetailById(It.IsAny<int>());

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
            Assert.AreEqual(null, ((actionResult as OkObjectResult)?.Value as dynamic)?.Message);
            Assert.AreEqual(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);

            _productServiceMock.Verify(x => x.GetDetailById(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void Should_Be_Get_Product_Detail_By_Slug(){
            //Arrange
            var controller = new ProductController(
                new Mock<DomainNotificationHandler>().Object,
                _productServiceMock.Object,
                _settingsMock);
            //Act
            var actionResult = controller.GetDetailBySlug(It.IsAny<string>());

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
            Assert.AreEqual(null, ((actionResult as OkObjectResult)?.Value as dynamic)?.Message);
            Assert.AreEqual(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);

            _productServiceMock.Verify(x => x.GetDetailBySlug(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void Should_Be_Verify_Add_Product(){
            //Arrange
            var controller = new ProductController(
                new Mock<DomainNotificationHandler>().Object,
                _productServiceMock.Object,
                _settingsMock);

            //Act
            var model = new ProductViewModel() {CreatedUserId = 495};
            var actionResult = controller.AddProduct(model);

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
            Assert.AreEqual("Product has been created", ((actionResult as OkObjectResult)?.Value as dynamic)?.Message);
            Assert.AreEqual(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);
            var links = (List<Link>) ((actionResult as OkObjectResult)?.Value as dynamic)?.Links;
            Assert.IsTrue(links.Any());
            Assert.AreEqual($"{_settingsMock.Value.ApiUrl}/productimages/{model.UuId}/users/{model.CreatedUserId}",
                links.First().Href);
            Assert.AreEqual("product.image.upload", links.First().Rel);

            _productServiceMock.Verify(x => x.AddProduct(It.IsAny<ProductViewModel>()), Times.Once);
        }

        [Fact]
        public void Should_Be_Verify_Update_Product(){
            //Arrange
            var controller = new ProductController(
                new Mock<DomainNotificationHandler>().Object,
                _productServiceMock.Object,
                _settingsMock);

            //Act
            var actionResult = controller.UpdateProduct(It.IsAny<ProductViewModel>());

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
            Assert.AreEqual("Product has been updated", ((actionResult as OkObjectResult)?.Value as dynamic)?.Message);
            Assert.AreEqual(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);

            _productServiceMock.Verify(x => x.UpdateProduct(It.IsAny<ProductViewModel>()), Times.Once);
        }

        [Fact]
        public void Should_Be_Verify_Delete_Product(){
            //Arrange
            var controller = new ProductController(
                new Mock<DomainNotificationHandler>().Object,
                _productServiceMock.Object,
                _settingsMock);

            //Act
            var actionResult = controller.DeleteProduct(It.IsAny<int>());

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
            Assert.AreEqual("Product has been deleted", ((actionResult as OkObjectResult)?.Value as dynamic)?.Message);
            Assert.AreEqual(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);

            _productServiceMock.Verify(x => x.DeleteProduct(It.IsAny<int>()), Times.Once);
        }
    }
}