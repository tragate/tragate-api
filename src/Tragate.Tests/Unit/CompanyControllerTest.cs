using Microsoft.AspNetCore.Mvc;
using Moq;
using Tragate.Application;
using Tragate.Application.ViewModels;
using Tragate.Domain.Core.Notifications;
using Tragate.WebApi.Controllers;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Tragate.Tests.Unit
{
    public class CompanyControllerTest : TestBase
    {
        private readonly Mock<ICompanyService> _companyServiceMock;
        private readonly Mock<IProductService> _productServiceMock;
        private readonly Mock<ICompanyMembershipService> _companyMembershipServiceMock;
        private readonly Mock<ICompanyNoteService> _companyNoteServiceMock;
        private readonly Mock<ICompanyTaskService> _companyTaskServiceMock;
        private readonly Mock<IQuoteService> _quoteServiceMock;

        public CompanyControllerTest(){
            _companyServiceMock = new Mock<ICompanyService>();
            _productServiceMock = new Mock<IProductService>();
            _companyMembershipServiceMock = new Mock<ICompanyMembershipService>();
            _companyNoteServiceMock = new Mock<ICompanyNoteService>();
            _companyTaskServiceMock = new Mock<ICompanyTaskService>();
            _quoteServiceMock = new Mock<IQuoteService>();
        }

        [Fact]
        public void Should_Be_Get_All_Data_Of_Company_By_Id(){
            //Arrange
            var controller = new CompanyController(new Mock<DomainNotificationHandler>().Object,
                _companyServiceMock.Object, _productServiceMock.Object, _companyNoteServiceMock.Object,
                _companyMembershipServiceMock.Object, _companyTaskServiceMock.Object, _quoteServiceMock.Object);


            //Act
            var actionResult = controller.GetCompanyById(It.IsAny<int>());

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
            Assert.AreEqual(null, ((actionResult as OkObjectResult)?.Value as dynamic)?.Message);
            Assert.AreEqual(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);

            _companyServiceMock.Verify(x => x.GetCompanyDetailById(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void Should_Be_Verify_Add_Company(){
            //Arrange
            var controller = new CompanyController(new Mock<DomainNotificationHandler>().Object,
                _companyServiceMock.Object, _productServiceMock.Object, _companyNoteServiceMock.Object,
                _companyMembershipServiceMock.Object, _companyTaskServiceMock.Object, _quoteServiceMock.Object);

            //Act
            controller.AddCompany(It.IsAny<CompanyViewModel>());

            //Assert
            _companyServiceMock.Verify(x => x.AddCompany(It.IsAny<CompanyViewModel>()), Times.Once);
        }

        [Fact]
        public void Should_Be_Verify_Update_Company(){
            //Arrange
            var controller = new CompanyController(new Mock<DomainNotificationHandler>().Object,
                _companyServiceMock.Object, _productServiceMock.Object, _companyNoteServiceMock.Object,
                _companyMembershipServiceMock.Object, _companyTaskServiceMock.Object, _quoteServiceMock.Object);

            //Act
            controller.UpdateCompany(It.IsAny<int>(), It.IsAny<CompanyViewModel>());

            //Assert
            _companyServiceMock.Verify(x => x.UpdateCompany(It.IsAny<int>(), It.IsAny<CompanyViewModel>()), Times.Once);
        }

        [Fact]
        public void Should_Be_Verify_Remove_Company(){
            //Arrange
            var controller = new CompanyController(new Mock<DomainNotificationHandler>().Object,
                _companyServiceMock.Object, _productServiceMock.Object, _companyNoteServiceMock.Object,
                _companyMembershipServiceMock.Object, _companyTaskServiceMock.Object, _quoteServiceMock.Object);

            //Act
            controller.DeleteCompany(It.IsAny<int>());

            //Assert
            _companyServiceMock.Verify(x => x.RemoveCompany(It.IsAny<int>()), Times.Once);
        }
    }
}