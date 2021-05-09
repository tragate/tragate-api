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
using Tragate.Common.Library.Helpers;
using Tragate.CrossCutting.Bus;
using Tragate.Domain.Core.Events;
using Tragate.Domain.Core.Notifications;
using Tragate.Domain.Interfaces;
using Tragate.WebApi.Controllers;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Tragate.Tests.Functional
{
    public class CompanyControllerTest : TestBase
    {
        private INotificationHandler<DomainNotification> _notifications;
        private IMapper _mapper;

        private CompanyController GetCompanyController(){
            var serviceProvider = base.BuildServiceProvider();
            var bus = new InMemoryBus(serviceProvider.GetService<IEventStore>(),
                serviceProvider.GetService<IMediator>());

            _mapper = serviceProvider.GetService<IMapper>();
            var companyRepository = serviceProvider.GetService<ICompanyRepository>();
            _notifications = serviceProvider.GetService<INotificationHandler<DomainNotification>>();
            var productService = serviceProvider.GetService<IProductService>();
            var companyMembershipService = serviceProvider.GetService<ICompanyMembershipService>();
            var companyNoteService = serviceProvider.GetService<ICompanyNoteService>();
            var companyTaskService = serviceProvider.GetService<ICompanyTaskService>();
            var quoteService = serviceProvider.GetService<IQuoteService>();

            var controller = new CompanyController(_notifications,
                new CompanyService(_mapper, bus, companyRepository), productService, companyNoteService,
                companyMembershipService, companyTaskService, quoteService);

            return controller;
        }


        [Theory]
        [InlineData(1, 10, (int) StatusType.All, "", null)]
        [InlineData(1, 10, (int) StatusType.Active, "", null)]
        [InlineData(1, 10, (int) StatusType.Active, "arçelik", null)]
        [InlineData(1, 10, (int) StatusType.Active, "arçelik", 6)]
        [InlineData(1, 10, (int) StatusType.Passive, "", null)]
        public void Should_Be_Success_When_Companies_Search(int page, int pageSize, int statusId, string name,
            int? categoryGroupId){
            var actionResult = GetCompanyController()
                .GetCompaniesByStatus(page, pageSize, statusId, name, categoryGroupId);
            actionResult.Should().BeOfType<OkObjectResult>();
            Assert.AreEqual(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);
        }

        [Theory]
        [InlineData(525, 1, 10, (int) StatusType.All)]
        [InlineData(525, 1, 10, (int) StatusType.Active)]
        [InlineData(525, 1, 10, (int) StatusType.Passive)]
        public void Should_Be_Success_When_Company_Task_Search(int id, int page, int pageSize, int statusId){
            var actionResult = GetCompanyController().GetCompanyTasksByCompanyId(id, page, pageSize, statusId);
            actionResult.Should().BeOfType<OkObjectResult>();
            Assert.AreEqual(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);
        }

        [Theory]
        [InlineData(1, 10, (int) StatusType.Active, "arçelik", 6)]
        public void Should_Be_Return_Correct_Data_When_Companies_Search(int page, int pageSize, int statusId,
            string name,
            int? categoryGroupId){
            var actionResult = GetCompanyController()
                .GetCompaniesByStatus(page, pageSize, statusId, name, categoryGroupId);
            Assert.AreEqual(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);
            PaginatedItemsViewModel<CompanyDto>
                companyList = ((actionResult as OkObjectResult)?.Value as dynamic)?.Data;
            Assert.IsTrue(companyList.DataList.Any(x => x.Slug.Contains("arçelik".GenerateSlug())));
        }


        [Theory]
        [InlineData(505)]
        public void Get_Company_By_Id_As_Success(int id){
            var actionResult = GetCompanyController().GetCompanyById(id);
            actionResult.Should().BeOfType<OkObjectResult>();
            Assert.AreEqual(null, ((actionResult as OkObjectResult)?.Value as dynamic)?.Message);
            Assert.AreEqual(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);

            CompanyDto company = ((actionResult as OkObjectResult)?.Value as dynamic)?.Data;
            company.UserId.Should().Be(505);
        }

        [Theory]
        [InlineData(525)]
        public void Get_Company_Memberships_By_Company_Id_As_Success(int id){
            var actionResult = GetCompanyController().GetCompanyMembershipsById(id);
            actionResult.Should().BeOfType<OkObjectResult>();
            Assert.AreEqual(null, ((actionResult as OkObjectResult)?.Value as dynamic)?.Message);
            Assert.AreEqual(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);
        }

        [Theory]
        [InlineData(525, 1, 10, (int) StatusType.Active)]
        public void Get_Company_Note_By_Id_As_Success(int id, int page, int pageSize, int status){
            var actionResult = GetCompanyController().GetCompanyNotesByCompanyId(id, page, pageSize, status);
            actionResult.Should().BeOfType<OkObjectResult>();
            Assert.AreEqual(null, ((actionResult as OkObjectResult)?.Value as dynamic)?.Message);
            Assert.AreEqual(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);
        }

        [Fact]
        public void Add_Company_Should_Be_Failed_If_FullName_Is_Null(){
            var model = new CompanyViewModel()
            {
                PersonId = 440,
                LocationId = 3,
                CountryId = 1,
                StateId = 3,
                BusinessType = "1,2,3"
            };
            var actionResult = GetCompanyController().AddCompany(model);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            Assert.AreEqual(HttpStatusCode.BadRequest,
                (HttpStatusCode) ((BadRequestObjectResult) actionResult).StatusCode);
            Assert.IsTrue(((DomainNotificationHandler) _notifications).HasNotifications());
            Assert.AreEqual("Please ensure you have entered the UserFullName",
                ((DomainNotificationHandler) _notifications).GetNotifications().First().Value);
        }

        [Fact]
        public void Add_Company_Should_Be_Failed_If_PersonId_Is_Null(){
            var model = new CompanyViewModel()
            {
                FullName = "werwer",
                LocationId = 3,
                CountryId = 1,
                StateId = 3,
                BusinessType = "1,2,3"
            };
            var actionResult = GetCompanyController().AddCompany(model);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            Assert.AreEqual(HttpStatusCode.BadRequest,
                (HttpStatusCode) ((BadRequestObjectResult) actionResult).StatusCode);
            Assert.IsTrue(((DomainNotificationHandler) _notifications).HasNotifications());
            Assert.AreEqual("Please ensure you have entered the PersonId",
                ((DomainNotificationHandler) _notifications).GetNotifications().First().Value);
        }

        [Fact]
        public void Add_Company_Should_Be_Failed_If_LocationId_Is_Null(){
            var model = new CompanyViewModel()
            {
                FullName = "werwer",
                PersonId = 440,
                CountryId = 1,
                StateId = 3,
                BusinessType = "1,2,3"
            };
            var actionResult = GetCompanyController().AddCompany(model);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            Assert.AreEqual(HttpStatusCode.BadRequest,
                (HttpStatusCode) ((BadRequestObjectResult) actionResult).StatusCode);
            Assert.IsTrue(((DomainNotificationHandler) _notifications).HasNotifications());
            Assert.AreEqual("Please ensure you have entered the LocationId",
                ((DomainNotificationHandler) _notifications).GetNotifications().First().Value);
        }

        [Fact]
        public void Add_Company_Should_Be_Failed_If_CountryId_Is_Null(){
            var model = new CompanyViewModel()
            {
                FullName = "werwer",
                PersonId = 440,
                LocationId = 1,
                StateId = 3,
                BusinessType = "1,2,3"
            };
            var actionResult = GetCompanyController().AddCompany(model);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            Assert.AreEqual(HttpStatusCode.BadRequest,
                (HttpStatusCode) ((BadRequestObjectResult) actionResult).StatusCode);
            Assert.IsTrue(((DomainNotificationHandler) _notifications).HasNotifications());
            Assert.AreEqual("Please ensure you have entered the CountryId",
                ((DomainNotificationHandler) _notifications).GetNotifications().First().Value);
        }

        [Fact]
        public void Add_Company_Should_Be_Failed_If_StateId_Is_Null(){
            var model = new CompanyViewModel()
            {
                FullName = "werwer",
                PersonId = 440,
                LocationId = 1,
                CountryId = 1,
                BusinessType = "1,2,3"
            };
            var actionResult = GetCompanyController().AddCompany(model);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            Assert.AreEqual(HttpStatusCode.BadRequest,
                (HttpStatusCode) ((BadRequestObjectResult) actionResult).StatusCode);
            Assert.IsTrue(((DomainNotificationHandler) _notifications).HasNotifications());
            Assert.AreEqual("Please ensure you have entered the StateId",
                ((DomainNotificationHandler) _notifications).GetNotifications().First().Value);
        }

        [Fact]
        public void Update_Company_Should_Be_Failed_If_FullName_Is_Null(){
            var model = new CompanyViewModel()
            {
                PersonId = 440,
                LocationId = 3,
                CountryId = 1,
                StateId = 3,
                BusinessType = "1,2,3",
                StatusId = (int) StatusType.Active
            };
            var actionResult = GetCompanyController().UpdateCompany(498, model);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            Assert.AreEqual(HttpStatusCode.BadRequest,
                (HttpStatusCode) ((BadRequestObjectResult) actionResult).StatusCode);
            Assert.IsTrue(((DomainNotificationHandler) _notifications).HasNotifications());
            Assert.AreEqual("Please ensure you have entered the UserFullName",
                ((DomainNotificationHandler) _notifications).GetNotifications().First().Value);
        }

        [Fact]
        public void Update_Company_Should_Be_Failed_If_Id_Is_Null(){
            var model = new CompanyViewModel()
            {
                FullName = "testCompanyA.Ş.",
                PersonId = 440,
                LocationId = 3,
                CountryId = 1,
                StateId = 3,
                BusinessType = "1,2,3",
                StatusId = (int) StatusType.Active
            };
            var actionResult = GetCompanyController().UpdateCompany(0, model);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            Assert.AreEqual(HttpStatusCode.BadRequest,
                (HttpStatusCode) ((BadRequestObjectResult) actionResult).StatusCode);
            Assert.IsTrue(((DomainNotificationHandler) _notifications).HasNotifications());
            Assert.AreEqual("Please ensure you have entered the Id",
                ((DomainNotificationHandler) _notifications).GetNotifications().First().Value);
        }

        [Fact]
        public void Update_Company_Should_Be_Failed_If_Status_Is_Null(){
            var model = new CompanyViewModel()
            {
                FullName = "testCompanyA.Ş.",
                PersonId = 440,
                LocationId = 1,
                CountryId = 1,
                BusinessType = "1,2,3",
            };
            var actionResult = GetCompanyController().UpdateCompany(498, model);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            Assert.AreEqual(HttpStatusCode.BadRequest,
                (HttpStatusCode) ((BadRequestObjectResult) actionResult).StatusCode);
            Assert.IsTrue(((DomainNotificationHandler) _notifications).HasNotifications());
            Assert.AreEqual("Status shouldn't be 0",
                ((DomainNotificationHandler) _notifications).GetNotifications().First().Value);
        }

        [Fact]
        public void Update_Company_Should_Be_Failed_If_CompanyName_Is_Not_Unique(){
            var model = new CompanyViewModel()
            {
                FullName = "Enes Dsbudak Ltd Şti 2",
                PersonId = 1,
                LocationId = 1,
                StateId = 3882,
                CityId = 3899,
                CountryId = 1,
                BusinessType = "1,2,3",
                StatusId = (int) StatusType.Active
            };
            var actionResult = GetCompanyController().UpdateCompany(496, model);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            Assert.AreEqual(HttpStatusCode.BadRequest,
                (HttpStatusCode) ((BadRequestObjectResult) actionResult).StatusCode);
            Assert.IsTrue(((DomainNotificationHandler) _notifications).HasNotifications());
            Assert.AreEqual("Company name must be unique",
                ((DomainNotificationHandler) _notifications).GetNotifications().First().Value);
        }
    }
}