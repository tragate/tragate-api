using System.Linq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Tragate.Application;
using Tragate.Application.ViewModels;
using Tragate.Domain.Core.Notifications;
using Tragate.WebApi.Controllers;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Tragate.Tests.Functional
{
    public class QuoteHistoryControllerTest : TestBase
    {
        private INotificationHandler<DomainNotification> _notifications;

        private QuoteHistoryController GetQuotationHistoryController(){
            var serviceProvider = base.BuildServiceProvider();
            _notifications = serviceProvider.GetService<INotificationHandler<DomainNotification>>();
            var quoteHistoryService = serviceProvider.GetService<IQuoteHistoryService>();

            var controller =
                new QuoteHistoryController(_notifications, quoteHistoryService);
            return controller;
        }


        [Fact]
        public void Should_Be_Validate_When_Create_Quote_History(){
            //Arrange
            var model = new CreateQuoteHistoryViewModel();
            var errorMessageList = new[]
            {
                "'Quote Id' should not be empty.",
                "'Quote Id' must be greater than '0'.",
                "'Description' should not be empty.",
                "'Created User Id' should not be empty.",
                "'Created User Id' must be greater than '0'."
            };

            //Act
            var actionResult = GetQuotationHistoryController().CreateQuoteHistory(model);

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