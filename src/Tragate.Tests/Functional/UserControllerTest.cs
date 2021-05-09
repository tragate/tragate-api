using System.Linq;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Tragate.Application;
using Tragate.Application.ServiceUtility.Login;
using Tragate.Application.ViewModels;
using Tragate.Common.Library.Dto;
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
    public class UserControllerTest : TestBase
    {
        private INotificationHandler<DomainNotification> _notifications;
        private IMapper _mapper;
        private readonly Mock<IDistributedCache> _distributedCacheMock;
        private readonly Mock<ILoginFactory> _loginFactoryMock;

        public UserControllerTest(){
            _distributedCacheMock = new Mock<IDistributedCache>();
            _loginFactoryMock = new Mock<ILoginFactory>();
        }

        private UserController GetUserController(){
            var serviceProvider = base.BuildServiceProvider();
            var bus = new InMemoryBus(serviceProvider.GetService<IEventStore>(),
                serviceProvider.GetService<IMediator>());

            _mapper = serviceProvider.GetService<IMapper>();
            var userRepository = serviceProvider.GetService<IUserRepository>();
            _notifications = serviceProvider.GetService<INotificationHandler<DomainNotification>>();
            var productService = serviceProvider.GetService<IProductService>();
            var companyNoteService = serviceProvider.GetService<ICompanyNoteService>();
            var companyTaskService = serviceProvider.GetService<ICompanyTaskService>();
            var quoteService = serviceProvider.GetService<IQuoteService>();

            var controller =
                new UserController(_notifications,
                    new UserService(_mapper, bus, userRepository, _distributedCacheMock.Object,
                        _loginFactoryMock.Object), productService, companyNoteService, companyTaskService,
                    quoteService);

            return controller;
        }

        [Theory]
        [InlineData("bilal-islam@hotmail.com")]
        public void Get_User_By_Email_As_Success(string email){
            var actionResult = GetUserController().GetUserByEmail(email);
            actionResult.Should().BeOfType<OkObjectResult>();

            Assert.AreEqual(null, ((actionResult as OkObjectResult)?.Value as dynamic)?.Message);
            Assert.AreEqual(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);

            UserDto user = ((actionResult as OkObjectResult)?.Value as dynamic)?.Data;
            user.Email.Should().Be(email);
        }

        [Theory]
        [InlineData(1)]
        public void Get_User_By_Id_As_Success(int id){
            var actionResult = GetUserController().GetUserById(id);
            actionResult.Should().BeOfType<OkObjectResult>();

            Assert.AreEqual(null, ((actionResult as OkObjectResult)?.Value as dynamic)?.Message);
            Assert.AreEqual(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);

            UserDto user = ((actionResult as OkObjectResult)?.Value as dynamic)?.Data;
            user.Id.Should().Be(1);
        }

        [Theory]
        [InlineData(495, 1, 10, null)]
        [InlineData(495, 1, 10, (int) StatusType.Active)]
        public void Get_Company_Note_By_Id_Success(int id, int page, int pageSize, int status){
            var actionResult = GetUserController().GetCompanyNotesByUserId(id, page, pageSize, status);
            actionResult.Should().BeOfType<OkObjectResult>();
            Assert.AreEqual(null, ((actionResult as OkObjectResult)?.Value as dynamic)?.Message);
            Assert.AreEqual(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);
        }

        [Theory]
        [InlineData(495, 1, 10, null)]
        [InlineData(495, 1, 10, (int) StatusType.Active)]
        public void Get_Company_Task_By_Id_Success(int id, int page, int pageSize, int status){
            var actionResult = GetUserController().GetCompanyTasksByUserId(id, page, pageSize, status);
            actionResult.Should().BeOfType<OkObjectResult>();
            Assert.AreEqual(null, ((actionResult as OkObjectResult)?.Value as dynamic)?.Message);
            Assert.AreEqual(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);
        }

        [Fact]
        public void Should_Not_Be_Validate_Change_Email_When_Required_Model_InCorrect(){
            //Arrange
            var model = new UserChangeEmailViewModel() { };
            var errorMessageList = new[]
            {
                "Please ensure you have entered the Id",
                "Please ensure you have entered the Email ",
                "Please ensure you have entered the Password"
            };

            //Act
            var actionResult = GetUserController().ChangeEmail(0, model);

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            var errors = ((DomainNotificationHandler) _notifications).GetNotifications();
            foreach (var item in errorMessageList){
                Assert.IsTrue(errors.Any(x => x.Value == item));
            }

            Assert.AreEqual(errorMessageList.Length, errors.Count);
        }

        [Fact]
        public void Should_Not_Be_Validate_Change_Email_When_User_Not_Found(){
            //Arrange
            var model = new UserChangeEmailViewModel()
            {
                Email = "bilal-islam@hotmail.com",
                Password = "123456"
            };
            var errorMessageList = new[]
            {
                "User not found",
            };

            //Act
            var actionResult = GetUserController().ChangeEmail(12312312, model);

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            var errors = ((DomainNotificationHandler) _notifications).GetNotifications();
            foreach (var item in errorMessageList){
                Assert.IsTrue(errors.Any(x => x.Value == item));
            }

            Assert.AreEqual(errorMessageList.Length, errors.Count);
        }

        [Fact]
        public void Should_Not_Be_Validate_Change_Email_When_Password_InValid(){
            //Arrange
            var model = new UserChangeEmailViewModel()
            {
                Email = "bilal-islam@test.com",
                Password = "123123123"
            };

            var errorMessageList = new[]
            {
                "Password is invalid",
            };

            //Act
            var actionResult = GetUserController().ChangeEmail(495, model);

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            var errors = ((DomainNotificationHandler) _notifications).GetNotifications();
            foreach (var item in errorMessageList){
                Assert.IsTrue(errors.Any(x => x.Value == item));
            }

            Assert.AreEqual(errorMessageList.Length, errors.Count);
        }


        [Fact]
        public void Should_Not_Be_Validate_Change_Email_When_Email_Already_Exists(){
            //Arrange
            var model = new UserChangeEmailViewModel()
            {
                Email = "bilal-islam@hotmail.com",
                Password = "123456"
            };

            var errorMessageList = new[]
            {
                "Email already exists",
            };

            //Act
            var actionResult = GetUserController().ChangeEmail(495, model);

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            var errors = ((DomainNotificationHandler) _notifications).GetNotifications();
            foreach (var item in errorMessageList){
                Assert.IsTrue(errors.Any(x => x.Value == item));
            }

            Assert.AreEqual(errorMessageList.Length, errors.Count);
        }

        [Fact]
        public void Should_Not_Be_Validate_Send_Activation_Email_When_Email_Does_Not_Exists(){
            //Arrange       
            var model = new UserSendActivationEmailViewModel()
            {
                Email = "bilaltest@gmail.com"
            };

            var errorMessageList = new[]
            {
                "Email doesn't exists",
            };

            //Act
            var actionResult = GetUserController().SendActivationEmail(model);

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            var errors = ((DomainNotificationHandler) _notifications).GetNotifications();
            foreach (var item in errorMessageList){
                Assert.IsTrue(errors.Any(x => x.Value == item));
            }

            Assert.AreEqual(errorMessageList.Length, errors.Count);
        }

        [Fact]
        public void Should_Not_Be_Validate_Send_Activation_Email_When_Email_Is_Null(){
            //Arrange           

            var model = new UserSendActivationEmailViewModel();

            var errorMessageList = new[]
            {
                "Please ensure you have entered the Email ",
            };

            //Act
            var actionResult = GetUserController().SendActivationEmail(model);

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            var errors = ((DomainNotificationHandler) _notifications).GetNotifications();
            foreach (var item in errorMessageList){
                Assert.IsTrue(errors.Any(x => x.Value == item));
            }

            Assert.AreEqual(errorMessageList.Length, errors.Count);
        }

        [Fact]
        public void Should_Be_Success_Get_TodoList_Of_User(){
            //Act
            var actionResult = GetUserController().GetTodoList(495);

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
            var errors = ((DomainNotificationHandler) _notifications).GetNotifications();
            Assert.IsFalse(errors.Any());
        }

        [Fact]
        public void Should_Be_Success_Get_TodoList_Of_Company(){
            //Act
            var actionResult = GetUserController().GetTodoList(525);

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
            var errors = ((DomainNotificationHandler) _notifications).GetNotifications();
            Assert.IsFalse(errors.Any());
        }

        [Fact]
        public void Should_Be_Return_User_Not_Found_When_User_Not_Exists(){
            //Arrange

            var errorMessageList = new[]
            {
                "User not found",
            };

            //Act
            var actionResult = GetUserController().GetTodoList(123123);

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