using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Tragate.Application;
using Tragate.Application.ViewModels;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Core.Notifications;
using Tragate.WebApi.Controllers;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;


namespace Tragate.Tests.Unit
{
    public class CategoryControllerTest : TestBase
    {
        private readonly Mock<ICategoryService> _categoryServiceMock;
        private readonly Mock<IProductService> _productServiceMock;

        public CategoryControllerTest(){
            _categoryServiceMock = new Mock<ICategoryService>();
            _productServiceMock = new Mock<IProductService>();
        }

        [Fact]
        public void Get_Categories_By_Parent_Is_Null(){
            //Arrange
            var controller = new CategoryController(new Mock<DomainNotificationHandler>().Object,
                _categoryServiceMock.Object, _productServiceMock.Object);

            //Act
            var actionResult = controller.GetCategories((int) StatusType.Active, It.IsAny<int?>(), It.IsAny<string>());

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
            Assert.AreEqual(null, ((actionResult as OkObjectResult)?.Value as dynamic)?.Message);
            Assert.AreEqual(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);

            _categoryServiceMock.Verify(x => x.GetCategories(StatusType.Active, It.IsAny<int?>(), It.IsAny<string>()));
        }

        [Fact]
        public void Get_Categories_By_Parent_Is_Number(){
            //Arrange
            var controller = new CategoryController(new Mock<DomainNotificationHandler>().Object,
                _categoryServiceMock.Object, _productServiceMock.Object);

            //Act
            var actionResult = controller.GetCategories((int) StatusType.Active, It.IsAny<int>(), It.IsAny<string>());

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
            Assert.AreEqual(null, ((actionResult as OkObjectResult)?.Value as dynamic)?.Message);
            Assert.AreEqual(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);

            _categoryServiceMock.Verify(x => x.GetCategories(StatusType.Active, It.IsAny<int>(), It.IsAny<string>()),
                Times.Once);
        }

        [Fact]
        public void Get_Category_Group_Should_Be_Verify_Correct_Service(){
            //Arrange
            var controller = new CategoryController(new Mock<DomainNotificationHandler>().Object,
                _categoryServiceMock.Object, _productServiceMock.Object);

            //Act
            var actionResult = controller.GetCategoryGroup();

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
            Assert.AreEqual(null, ((actionResult as OkObjectResult)?.Value as dynamic)?.Message);
            Assert.AreEqual(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);

            _categoryServiceMock.Verify(x => x.GetCategoryGroup(), Times.Once);
        }

        [Fact]
        public void Get_Sub_Category_Group_Should_Be_Verify_Correct_Service(){
            //Arrange
            var controller = new CategoryController(new Mock<DomainNotificationHandler>().Object,
                _categoryServiceMock.Object, _productServiceMock.Object);


            //Act
            var actionResult = controller.GetSubCategoryGroup(It.IsAny<int[]>(), It.IsAny<int>());

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
            Assert.AreEqual(null, ((actionResult as OkObjectResult)?.Value as dynamic)?.Message);
            Assert.AreEqual(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);

            _categoryServiceMock.Verify(x => x.GetSubCategoryGroup(It.IsAny<int[]>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void Get_Category_By_Id_Should_Be_Verify_Correct_Service(){
            //Arrange
            var controller = new CategoryController(new Mock<DomainNotificationHandler>().Object,
                _categoryServiceMock.Object, _productServiceMock.Object);


            //Act
            var actionResult = controller.GetCategoryById(It.IsAny<int>());

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
            Assert.AreEqual(null, ((actionResult as OkObjectResult)?.Value as dynamic)?.Message);
            Assert.AreEqual(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);

            _categoryServiceMock.Verify(x => x.GetCategoryById(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void Should_Be_Verify_Add_Category(){
            //Arrange
            var controller = new CategoryController(new Mock<DomainNotificationHandler>().Object,
                _categoryServiceMock.Object, _productServiceMock.Object);


            //Act
            var actionResult = controller.AddCategory(It.IsAny<CategoryViewModel>());

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
            Assert.AreEqual("Category has been created", ((actionResult as OkObjectResult)?.Value as dynamic)?.Message);
            Assert.AreEqual(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);

            _categoryServiceMock.Verify(x => x.AddCategory(It.IsAny<CategoryViewModel>()), Times.Once);
        }


        [Fact]
        public void Should_Be_Verify_Update_Category(){
            //Arrange
            var controller = new CategoryController(new Mock<DomainNotificationHandler>().Object,
                _categoryServiceMock.Object, _productServiceMock.Object);


            //Act
            var actionResult = controller.UpdateCategory(It.IsAny<CategoryViewModel>());

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
            Assert.AreEqual("Category has been updated", ((actionResult as OkObjectResult)?.Value as dynamic)?.Message);
            Assert.AreEqual(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);

            _categoryServiceMock.Verify(x => x.UpdateCategory(It.IsAny<CategoryViewModel>()), Times.Once);
        }

        [Fact]
        public void Should_Be_Verify_Change_Category_Image(){
            //Arrange
            var controller = new CategoryController(new Mock<DomainNotificationHandler>().Object,
                _categoryServiceMock.Object, _productServiceMock.Object);

            //Act
            var actionResult = controller.ChangeCategoryImage(It.IsAny<IFormFile>(), It.IsAny<int>());

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
            Assert.AreEqual("Image has been uploded", ((actionResult as OkObjectResult)?.Value as dynamic)?.Message);
            Assert.AreEqual(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);

            _categoryServiceMock.Verify(x => x.UploadImage(It.IsAny<IFormFile>(), It.IsAny<int>()), Times.Once);
        }
    }
}