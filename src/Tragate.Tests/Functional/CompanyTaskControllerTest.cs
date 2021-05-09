using System.Linq;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Tragate.Application;
using Tragate.Application.ViewModels;
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
    public class CompanyTaskControllerTest : TestBase
    {
        private INotificationHandler<DomainNotification> _notifications;
        private IMapper _mapper;

        private CompanyTaskController GetCompanyTaskController(){
            var serviceProvider = base.BuildServiceProvider();
            var bus = new InMemoryBus(serviceProvider.GetService<IEventStore>(),
                serviceProvider.GetService<IMediator>());

            _mapper = serviceProvider.GetService<IMapper>();
            _notifications = serviceProvider.GetService<INotificationHandler<DomainNotification>>();
            var companyTaskRepository = serviceProvider.GetService<ICompanyTaskRepository>();
            var controller = new CompanyTaskController(_notifications, new CompanyTaskService(companyTaskRepository,
                _mapper, bus));

            return controller;
        }

        [Theory]
        [InlineData(1, 10, (int) StatusType.All)]
        [InlineData(1, 10, (int) StatusType.Active)]
        [InlineData(1, 10, (int) StatusType.Passive)]
        public void Should_Be_Success_When_Company_Task_Search(int page, int pageSize, int statusId){
            var actionResult = GetCompanyTaskController().GetCompanyTasks(page, pageSize, statusId, null, null, null);
            actionResult.Should().BeOfType<OkObjectResult>();
            Assert.AreEqual(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);
        }

        [Fact]
        public void Should_Be_Validate_When_Add_Company_Task(){
            //Arrange
            var model = new CompanyTaskViewModel();
            var errorMessageList = new[]
            {
                "'Company Id' should not be empty.",
                "'Company Id' must be greater than '0'.",
                "'Description' should not be empty.",
                "'Responsible User Id' should not be empty.",
                "'Responsible User Id' must be greater than '0'.",
                "'Created User Id' should not be empty.",
                "'Created User Id' must be greater than '0'.",
                "'Company Task Type Id' should not be empty.",
                "The specified condition was not met for 'Company Task Type Id'.",
                "'Status Id' should not be empty.",
                "The specified condition was not met for 'Status Id'.",
            };

            //Act
            var actionResult = GetCompanyTaskController().AddCompanyTask(model);

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            var errors = ((DomainNotificationHandler) _notifications).GetNotifications();
            foreach (var item in errorMessageList){
                Assert.IsTrue(errors.Any(x => x.Value == item));
            }

            Assert.AreEqual(errorMessageList.Length, errors.Count);
        }

        [Fact]
        public void Should_Be_Validate_When_Update_Company_Task_Status(){
            //Arrange
            var model = new CompanyTaskStatusViewModel();
            var errorMessageList = new[]
            {
                "'Status Id' should not be empty.",
                "The specified condition was not met for 'Status Id'.",
                "'Updated User Id' must be greater than '0'."
            };

            //Act
            var actionResult = GetCompanyTaskController().UpdateStatusCompanyTask(1, model);

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            var errors = ((DomainNotificationHandler) _notifications).GetNotifications();
            foreach (var item in errorMessageList){
                Assert.IsTrue(errors.Any(x => x.Value == item));
            }

            Assert.AreEqual(errorMessageList.Length, errors.Count);
        }

        [Fact]
        public void Should_Be_Validate_When_Delete_Company_Task(){
            //Arrange

            var errorMessageList = new[]
            {
                "'Id' should not be empty.",
                "'Id' must be greater than '0'."
            };

            //Act
            var actionResult = GetCompanyTaskController().DeleteCompanyTask(0);

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            var errors = ((DomainNotificationHandler) _notifications).GetNotifications();
            foreach (var item in errorMessageList){
                Assert.IsTrue(errors.Any(x => x.Value == item));
            }

            Assert.AreEqual(errorMessageList.Length, errors.Count);
        }

        [Fact]
        public void Should_Be_Validate_Not_Found_When_Delete_Company_Task(){
            //Arrange

            var errorMessageList = new[]
            {
                "Company task not found",
            };

            //Act
            var actionResult = GetCompanyTaskController().DeleteCompanyTask(int.MaxValue);

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            var errors = ((DomainNotificationHandler) _notifications).GetNotifications();
            foreach (var item in errorMessageList){
                Assert.IsTrue(errors.Any(x => x.Value == item));
            }

            Assert.AreEqual(errorMessageList.Length, errors.Count);
        }
    }
}