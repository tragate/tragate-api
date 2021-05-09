using System.Linq;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Tragate.Application;
using Tragate.Application.ViewModels;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;
using Tragate.Common.Library.Helpers;
using Tragate.Domain.Core.Notifications;
using Tragate.WebApi.Controllers;
using Xunit;

namespace Tragate.Tests.Functional
{
    public class CompanyAdminControllerTest : TestBase
    {
        private CompanyAdminController GetCompanyAdminController(){
            var serviceProvider = base.BuildServiceProvider();
            var notifications = serviceProvider.GetService<INotificationHandler<DomainNotification>>();
            var companyAdminService = serviceProvider.GetService<ICompanyAdminService>();
            var productService = serviceProvider.GetService<IProductService>();
            return new CompanyAdminController(notifications, companyAdminService);
        }

        [Theory]
        [InlineData(1, 10, 495, (int) StatusType.All, null)]
        [InlineData(1, 10, 495, (int) StatusType.Active, null)]
        [InlineData(1, 10, 495, (int) StatusType.Active, "bilkan oto")]
        [InlineData(1, 10, 495, (int) StatusType.Passive, null)]
        public void Should_Be_Success_When_Company_Admins_By_User_Id_Search(int page, int pageSize, int userId,
            int statusId, string name){
            var actionResult = GetCompanyAdminController()
                .GetCompanyAdminsByUserId(page, pageSize, userId, statusId, name);

            actionResult.Should().BeOfType<OkObjectResult>();
            Assert.Equal(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);
        }

        [Theory]
        [InlineData(1, 10, 495, (int) StatusType.Active, "bilkan oto")]
        public void Should_Be_Return_Correct_Data_When_Company_Admins_By_User_Id_Search(int page, int pageSize,
            int userId,
            int statusId, string name){
            var actionResult = GetCompanyAdminController()
                .GetCompanyAdminsByUserId(page, pageSize, userId, statusId, name);
            Assert.Equal(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);
            PaginatedItemsViewModel<CompanyAdminCompanyDto>
                companyAdminList = ((actionResult as OkObjectResult)?.Value as dynamic)?.Data;
            Assert.True(companyAdminList.DataList.Any(x => x.Slug.Contains("bilkan oto".GenerateSlug())));
        }

        //TODO: dogru data Id'si verilecek test ve prod eşit değil çözülecek
//        [Theory]
//        [InlineData(505, 1, 10, "lenovo", StatusType.Active)]
//        [InlineData(505, 1, 10, "", StatusType.Active)]
//        public void Should_Be_Return_Correct_Data_When_Company_Admin_Products_By_Company_Id_Search(int id, int page,
//            int pageSize,
//            string name, StatusType status){
//            var actionResult = GetCompanyAdminController()
//                .GetProductsByCompanyId(id, page, pageSize, name, (int) status);
//            Assert.Equal(true, ((actionResult as OkObjectResult)?.Value as dynamic)?.Success);
//            PaginatedItemsViewModel<CompanyAdminProductDto> companyAdminProductList =
//                ((actionResult as OkObjectResult)?.Value as dynamic)?.Data;
//            Assert.True(companyAdminProductList.DataList.Any());
//        }
    }
}