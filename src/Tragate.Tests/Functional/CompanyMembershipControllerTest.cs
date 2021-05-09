using System;
using System.Linq;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Tragate.Application;
using Tragate.Application.ViewModels;
using Tragate.CrossCutting.Bus;
using Tragate.Domain.Core.Events;
using Tragate.Domain.Core.Notifications;
using Tragate.Domain.Interfaces;
using Tragate.WebApi.Controllers;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Tragate.Tests.Functional
{
    public class CompanyMembershipControllerTest : TestBase
    {
        private INotificationHandler<DomainNotification> _notifications;

        private CompanyMembershipController GetMembershipController(){
            var serviceProvider = base.BuildServiceProvider();
            var mapper = serviceProvider.GetService<IMapper>();
            var bus = new InMemoryBus(serviceProvider.GetService<IEventStore>(),
                serviceProvider.GetService<IMediator>());
            _notifications = serviceProvider.GetService<INotificationHandler<DomainNotification>>();
            var companyMembershipRepository = serviceProvider.GetService<ICompanyMembershipRepository>();

            var controller =
                new CompanyMembershipController(_notifications,
                    new CompanyMembershipService(mapper, bus, companyMembershipRepository));

            return controller;
        }


        [Fact]
        public void Should_Be_Validate_If_StartDate_Bigger_Than_EndDate(){
            //Arrange

            var model = new CompanyMembershipViewModel()
            {
                CompanyId = 525,
                MembershipTypeId = 1,
                MembershipPackageId = 1,
                StartDate = DateTime.Now.AddDays(2),
                EndDate = DateTime.Now,
                CreatedUserId = 495
            };

            var errorMessageList = new[]
            {
                "Startdate should not bigger than enddate"
            };

            //Act
            var actionResult = GetMembershipController().AddCompanyMembership(model);

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            var errors = ((DomainNotificationHandler) _notifications).GetNotifications();
            foreach (var item in errorMessageList){
                Assert.IsTrue(errors.Any(x => x.Value == item));
            }

            Assert.AreEqual(errorMessageList.Length, errors.Count);
        }

        [Fact]
        public void Should_Be_Validate_If_StartDate_And_EndDate_Are_Past_Date(){
            //Arrange

            var model = new CompanyMembershipViewModel()
            {
                CompanyId = 525,
                MembershipTypeId = 1,
                MembershipPackageId = 1,
                StartDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.Now.AddDays(-1),
                CreatedUserId = 495
            };

            var errorMessageList = new[]
            {
                $"'End Date' must be greater than or equal to '{DateTime.Now:M/dd/yyyy 00:00:00}'.",
                $"'Start Date' must be greater than or equal to '{DateTime.Now:M/dd/yyyy 00:00:00}'."
            };

            //Act
            var actionResult = GetMembershipController().AddCompanyMembership(model);

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            var errors = ((DomainNotificationHandler) _notifications).GetNotifications();
            Assert.AreEqual(errorMessageList.Length, errors.Count);
        }

        [Fact]
        public void Should_Be_Validate_If_All_Fields_Are_Null(){
            //Arrange

            var model = new CompanyMembershipViewModel()
            {
                MembershipPackageId = -1,
                MembershipTypeId = -1
            };

            var errorMessageList = new[]
            {
                "'Company Id' should not be empty.",
                "'Membership Package Id' should not be empty.",
                "'Membership Type Id' should not be empty.",
                "'Start Date' should not be empty.",
                $"'Start Date' must be greater than or equal to '{DateTime.Now:M/dd/yyyy 00:00:00}'.",
                "'End Date' should not be empty.",
                $"'End Date' must be greater than or equal to '{DateTime.Now:M/dd/yyyy 00:00:00}'.",
                "'Created User Id' should not be empty."
            };

            //Act
            var actionResult = GetMembershipController().AddCompanyMembership(model);

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            var errors = ((DomainNotificationHandler) _notifications).GetNotifications();
            Assert.AreEqual(errorMessageList.Length, errors.Count);
        }
    }
}