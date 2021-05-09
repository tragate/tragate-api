using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Tragate.Application;
using Tragate.Application.ServiceUtility.Login;
using Tragate.Application.ViewModels;
using Tragate.Common.Library;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Core.Notifications;
using Tragate.Domain.Interfaces;
using Tragate.Domain.Models;
using Tragate.WebApi.Controllers;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Tragate.Tests.Unit
{
    public class AccountControllerTest : TestBase
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IDistributedCache> _cacheMock;

        private readonly ILoginFactory _loginFactory;
        private readonly IMediatorHandler _bus;
        private readonly INotificationHandler<DomainNotification> _notifications;

        public AccountControllerTest(){
            _mapperMock = new Mock<IMapper>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _cacheMock = new Mock<IDistributedCache>();

            var provider = base.BuildServiceProvider();
            _loginFactory = provider.GetService<ILoginFactory>();
            _bus = provider.GetService<IMediatorHandler>();
            _notifications = provider.GetService<INotificationHandler<DomainNotification>>();
        }


        [Fact]
        public void Should_Be_User_Login_As_Admin(){
            //Arrange
            var controller = new AccountController(_notifications,
                new UserService(_mapperMock.Object, _bus,
                    _userRepositoryMock.Object,
                    _cacheMock.Object, _loginFactory));

            _userRepositoryMock.Setup(x => x.GetAdminByEmail(It.IsAny<string>())).Returns(new UserDto()
            {
                Password = "bf286c9c405c546b32c64f162ba04221ff37ecce8327bd605d9e5ea4a6bc004a",
                Salt = "a7162bdf8ebc34beb3a8eccea0f49055",
                UserStatus = true
            });

            //Act
            var actionResult = controller.Login(new LoginViewModel()
            {
                Email = "bilal.islam815@gmail.com",
                Password = "123",
                PlatformId = (int) PlatformType.Admin
            });


            //Assert
            Assert.AreEqual(200, ((actionResult as OkObjectResult)?.StatusCode));
            Assert.IsFalse(((DomainNotificationHandler) _notifications).HasNotifications());

            _userRepositoryMock.Verify(x => x.GetAdminByEmail(It.IsAny<string>()), Times.Once);
        }
    }
}