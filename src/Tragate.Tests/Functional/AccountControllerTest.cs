using System;
using System.Linq;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Tragate.Application;
using Tragate.Application.ServiceUtility.Login;
using Tragate.Application.ViewModels;
using Tragate.Common.Library;
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
    public class AccountControllerTest : TestBase
    {
        private INotificationHandler<DomainNotification> _notifications;
        private IDistributedCache _distributedCache;
        private IUserRepository _userRepository;

        private AccountController GetAccountController(){
            var serviceProvider = base.BuildServiceProvider();
            var bus = new InMemoryBus(serviceProvider.GetService<IEventStore>(),
                serviceProvider.GetService<IMediator>());

            var mapper = serviceProvider.GetService<IMapper>();
            _userRepository = serviceProvider.GetService<IUserRepository>();
            _notifications = serviceProvider.GetService<INotificationHandler<DomainNotification>>();
            _distributedCache = serviceProvider.GetService<IDistributedCache>();
            var loginFactory = serviceProvider.GetService<ILoginFactory>();

            var controller =
                new AccountController(_notifications,
                    new UserService(mapper, bus, _userRepository, _distributedCache, loginFactory));

            return controller;
        }


        [Fact]
        public void Should_Be_User_Login_As_Admin(){
            var model = new LoginViewModel()
            {
                Email = "bilal-islam@hotmail.com", //system admin user
                Password = "123456",
                PlatformId = (int) PlatformType.Admin
            };

            var actionResult = GetAccountController().Login(model);
            Assert.AreEqual(null, ((actionResult as OkObjectResult)?.Value as dynamic)?.Message);
            Assert.AreEqual(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);
        }

        [Fact]
        public void Should_Be_User_Login_As_Standard(){
            var model = new LoginViewModel()
            {
                Email = "bilal.islam815@gmail.com",
                Password = "1234567",
                PlatformId = (int) PlatformType.Web
            };

            var actionResult = GetAccountController().Login(model);
            Assert.AreEqual(null, ((actionResult as OkObjectResult)?.Value as dynamic)?.Message);
            Assert.AreEqual(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);
        }

        [Fact]
        public void Should_Not_Be_Login_As_Admin_When_If_User_Is_Standard(){
            var model = new LoginViewModel()
            {
                Email = "bilal.islam815@gmail.com", //normal user
                Password = "1234",
                PlatformId = (int) PlatformType.Admin
            };

            var actionResult = GetAccountController().Login(model);
            var errors = ((DomainNotificationHandler) _notifications);
            Assert.AreEqual(400, ((BadRequestObjectResult) actionResult).StatusCode);
            Assert.IsTrue(errors.HasNotifications());
            Assert.AreEqual("Email doesn't exists", errors.GetNotifications().First().Value);
        }

        [Fact]
        public void Should_Be_Login_By_Token_If_Token_Is_Valid(){
            string token = Guid.NewGuid().ToString();
            var model = new LoginViewModel()
            {
                Email = "bilal.islam815@gmail.com",
                Password = "1234",
                PlatformId = (int) PlatformType.Web,
                AutoLogin = true,
                Token = token
            };

            var controller = GetAccountController();
            _distributedCache.SetString(token, _userRepository.GetByEmail(model.Email).Id.ToString());
            var actionResult = controller.Login(model);
            Assert.AreEqual(null, ((actionResult as OkObjectResult)?.Value as dynamic)?.Message);
            Assert.AreEqual(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);
            Assert.IsNull(_distributedCache.GetString(token));
        }


        [Fact]
        public void Should_Not_Be_Login_By_Token_If_Token_Is_InValid(){
            string token = Guid.NewGuid().ToString();
            var model = new LoginViewModel()
            {
                Email = "bilal.islam815@gmail.com",
                Password = "123",
                PlatformId = (int) PlatformType.Web,
                AutoLogin = true,
                Token = token
            };

            var controller = GetAccountController();
            var actionResult = controller.Login(model);
            var errors = ((DomainNotificationHandler) _notifications);
            Assert.AreEqual(400, ((BadRequestObjectResult) actionResult).StatusCode);
            Assert.IsTrue(errors.HasNotifications());
            Assert.AreEqual("Invalid token", errors.GetNotifications().First().Value);
        }


        [Fact]
        public void Should_Be_Login_User_If_Password_Is_Valid(){
            var model = new LoginViewModel()
            {
                Email = "bilal.islam815@gmail.com",
                Password = "1234567",
                PlatformId = (int) PlatformType.Web
            };

            var controller = GetAccountController();
            var actionResult = controller.Login(model);
            Assert.AreEqual(null, ((actionResult as OkObjectResult)?.Value as dynamic)?.Message);
            Assert.AreEqual(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);
        }

        [Fact]
        public void Should_Not_Be_Login_User_If_Password_Is_InValid(){
            var model = new LoginViewModel()
            {
                Email = "bilal.islam815@gmail.com",
                Password = "123",
                PlatformId = (int) PlatformType.Web
            };

            var controller = GetAccountController();
            var actionResult = controller.Login(model);
            var errors = ((DomainNotificationHandler) _notifications);
            Assert.AreEqual(400, ((BadRequestObjectResult) actionResult).StatusCode);
            Assert.IsTrue(errors.HasNotifications());
            Assert.AreEqual("Password is invalid", errors.GetNotifications().First().Value);
        }


        [Fact]
        public void Should_Be_Login_User_If_Email_Is_Exists(){
            var model = new LoginViewModel()
            {
                Email = "bilal.islam815@gmail.com",
                Password = "1234567",
                PlatformId = (int) PlatformType.Web
            };

            var controller = GetAccountController();
            var actionResult = controller.Login(model);
            Assert.AreEqual(null, ((actionResult as OkObjectResult)?.Value as dynamic)?.Message);
            Assert.AreEqual(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);
        }

        [Fact]
        public void Should__Not_Be_Login_User_If_Email_Is_Does_Not_Exists(){
            var model = new LoginViewModel()
            {
                Email = "testtest@gmail.com",
                Password = "123213123",
                PlatformId = (int) PlatformType.Web
            };

            var controller = GetAccountController();
            var actionResult = controller.Login(model);
            var errors = ((DomainNotificationHandler) _notifications);
            Assert.AreEqual(400, ((BadRequestObjectResult) actionResult).StatusCode);
            Assert.IsTrue(errors.HasNotifications());
            Assert.AreEqual("Email doesn't exists", errors.GetNotifications().First().Value);
        }

        [Fact]
        public void Should__Not_Be_Login_User_If_Platform_Id_Does_Not_Exists(){
            var model = new LoginViewModel()
            {
                Email = "bilal-islam@hotmail.com",
                Password = "123456"
            };

            var actionResult = GetAccountController().Login(model);
            var errors = ((DomainNotificationHandler) _notifications);
            Assert.AreEqual(400, ((BadRequestObjectResult) actionResult).StatusCode);
            Assert.IsTrue(errors.HasNotifications());
            Assert.AreEqual("You're not allowed", errors.GetNotifications().First().Value);
        }
    }
}