using System.Linq;
using System.Net;
using AutoMapper;
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
    public class CompanyNoteControllerTest : TestBase
    {
        private INotificationHandler<DomainNotification> _notifications;

        private CompanyNoteController GetCompanyNoteController(){
            var serviceProvider = base.BuildServiceProvider();
            var bus = new InMemoryBus(serviceProvider.GetService<IEventStore>(),
                serviceProvider.GetService<IMediator>());

            var mapper = serviceProvider.GetService<IMapper>();
            var noteRepository = serviceProvider.GetService<ICompanyNoteRepository>();
            _notifications = serviceProvider.GetService<INotificationHandler<DomainNotification>>();

            var controller =
                new CompanyNoteController(_notifications,
                    new CompanyNoteService(noteRepository, mapper, bus));

            return controller;
        }


        [Theory]
        [InlineData(1, 10, null)]
        [InlineData(1, 10, (int) StatusType.Active)]
        public void Should_Be_Success_When_Company_Notes(int page, int pageSize, int? status){
            var actionResult = GetCompanyNoteController().GetCompanyNotes(page, pageSize, status);
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
            Assert.AreEqual(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);
        }

        [Fact]
        public void Add_Company_Note_Should_Be_Failed_If_Fields_Are_Null(){
            //Arrange
            var errorMessageList = new[]
            {
                "'Company Id' should not be empty.",
                "'Description' should not be empty.",
                "'Created User Id' should not be empty.",
                "'Created User Id' must be greater than '0'."
            };

            //Act
            var actionResult = GetCompanyNoteController().AddCompanyNote(new CompanyNoteViewModel());
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            Assert.AreEqual(HttpStatusCode.BadRequest,
                (HttpStatusCode) ((BadRequestObjectResult) actionResult).StatusCode);

            var errors = ((DomainNotificationHandler) _notifications).GetNotifications();

            //Assert
            foreach (var item in errorMessageList){
                Assert.IsTrue(errors.Any(x => x.Value == item));
            }

            Assert.AreEqual(errorMessageList.Length, errors.Count);
        }

        [Fact]
        public void Delete_Company_Note_Should_Be_Failed_If_Id_Is_Null(){
            //Arrange
            var errorMessageList = new[]
            {
                "'Id' should not be empty.",
                "'Id' must be greater than '0'."
            };

            //Act
            var actionResult = GetCompanyNoteController().DeleteCompanyNote(0);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            Assert.AreEqual(HttpStatusCode.BadRequest,
                (HttpStatusCode) ((BadRequestObjectResult) actionResult).StatusCode);

            var errors = ((DomainNotificationHandler) _notifications).GetNotifications();

            //Assert
            foreach (var item in errorMessageList){
                Assert.IsTrue(errors.Any(x => x.Value == item));
            }

            Assert.AreEqual(errorMessageList.Length, errors.Count);
        }
    }
}