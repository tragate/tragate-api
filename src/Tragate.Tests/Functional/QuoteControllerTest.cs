using System.Linq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tragate.Application;
using Tragate.WebApi.Controllers;
using Tragate.Domain.Core.Notifications;
using Microsoft.Extensions.DependencyInjection;
using Tragate.Application.ViewModels;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Tragate.Tests.Functional
{
    public class QuotationControllerTest : TestBase
    {
        private INotificationHandler<DomainNotification> _notifications;

        private QuoteController GetQuotationController(){
            var serviceProvider = base.BuildServiceProvider();
            _notifications = serviceProvider.GetService<INotificationHandler<DomainNotification>>();
            var quoteService = serviceProvider.GetService<IQuoteService>();
            var quoteHistoryService = serviceProvider.GetService<IQuoteHistoryService>();
            var quoteProductService = serviceProvider.GetService<IQuoteProductService>();
            var controller =
                new QuoteController(_notifications, quoteService, quoteHistoryService, quoteProductService);
            return controller;
        }

        [Fact]
        public void Should_Be_Validate_When_Create_Quote(){
            //Arrange
            var model = new CreateQuoteViewModel();
            var errorMessageList = new[]
            {
                "'Title' should not be empty.",
                "'User Message' should not be empty.",
                "Buyer User Email should not be empty",
                "'Seller Company Id' should not be empty.",
                "'Seller Company Id' must be greater than '0'."
            };

            //Act
            var actionResult = GetQuotationController().CreateQuote(model);

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            var errors = ((DomainNotificationHandler) _notifications).GetNotifications();
            foreach (var item in errorMessageList){
                Assert.IsTrue(errors.Any(x => x.Value == item));
            }

            Assert.AreEqual(errorMessageList.Length, errors.Count);
        }

        [Fact]
        public void Should_Be_Validate_Buyer_User_Email_If_Buyer_User_Id_Null(){
            //Arrange
            var model = new CreateQuoteViewModel()
            {
                Title = "test",
                UserMessage = "test",
                SellerCompanyId = 525
            };
            var errorMessageList = new[]
            {
                "Buyer User Email should not be empty",
            };

            //Act
            var actionResult = GetQuotationController().CreateQuote(model);

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            var errors = ((DomainNotificationHandler) _notifications).GetNotifications();
            foreach (var item in errorMessageList){
                Assert.IsTrue(errors.Any(x => x.Value == item));
            }

            Assert.AreEqual(errorMessageList.Length, errors.Count);
        }

        [Fact]
        public void Should_Be_Validate_Buyer_User_Country_And_State_If_Buyer_Email_Not_Null(){
            //Arrange
            var model = new CreateQuoteViewModel()
            {
                Title = "test",
                UserMessage = "test",
                BuyerUserEmail = "test",
                SellerCompanyId = 525
            };
            var errorMessageList = new[]
            {
                "Buyer User CountryId should not be empty",
                "Buyer User StateId should not be empty",
                "'Buyer User Email' is not a valid email address."
            };

            //Act
            var actionResult = GetQuotationController().CreateQuote(model);

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            var errors = ((DomainNotificationHandler) _notifications).GetNotifications();
            foreach (var item in errorMessageList){
                Assert.IsTrue(errors.Any(x => x.Value == item));
            }

            Assert.AreEqual(errorMessageList.Length, errors.Count);
        }


        [Fact]
        public void Should_Be_Validate_Created_User_Id_When_Defined_Buyer_User_Id_But_Created_User_Id_Undefined(){
            //Arrange
            var model = new CreateQuoteViewModel()
            {
                Title = "test",
                UserMessage = "test",
                BuyerUserId = 495,
                SellerCompanyId = 525
            };
            var errorMessageList = new[]
            {
                "Created User Id should not be empty"
            };

            //Act
            var actionResult = GetQuotationController().CreateQuote(model);

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            var errors = ((DomainNotificationHandler) _notifications).GetNotifications();
            foreach (var item in errorMessageList){
                Assert.IsTrue(errors.Any(x => x.Value == item));
            }

            Assert.AreEqual(errorMessageList.Length, errors.Count);
        }

        [Fact]
        public void Should_Be_Validate_Created_User_Id_When_Defined_Seller_User_Id_But_Created_User_Id_Undefined(){
            //Arrange
            var model = new CreateQuoteViewModel()
            {
                Title = "test",
                UserMessage = "test",
                BuyerUserEmail = "test@gmail.com",
                BuyerUserCountryId = 1,
                BuyerUserStateId = 2,
                SellerUserId = 495,
                SellerCompanyId = 525
            };
            var errorMessageList = new[]
            {
                "Created User Id should not be empty"
            };

            //Act
            var actionResult = GetQuotationController().CreateQuote(model);

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