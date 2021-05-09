using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tragate.Application;
using Tragate.Application.ViewModels;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Core.Notifications;

namespace Tragate.WebApi.Controllers
{
    public class CompanyAdminController : ApiController
    {
        private readonly ICompanyAdminService _companyAdminService;

        public CompanyAdminController(
            INotificationHandler<DomainNotification> notifications,
            ICompanyAdminService companyAdminService) :
            base(notifications){
            _companyAdminService = companyAdminService;
        }

        [HttpGet]
        [Route("companyadmins/{companyId}/users/page={page:int:min(1)}/pageSize={pageSize:int:max(100)}")]
        public IActionResult GetCompanyAdminsByCompanyId(int page, int pageSize, int companyId, int status){
            var result =
                _companyAdminService.GetCompanyAdminsByCompanyId(page, pageSize, companyId, (StatusType) status);
            var count = _companyAdminService.CountCompanyAdminsByCompanyId(companyId, (StatusType) status);
            var model = new PaginatedItemsViewModel<CompanyAdminUserDto>(
                page, pageSize, count, result);
            return Response(model);
        }

        [HttpGet]
        [Route("companyadmins/{userId}/companies/page={page:int:min(1)}/pageSize={pageSize:int:max(10)}")]
        public IActionResult GetCompanyAdminsByUserId(int page, int pageSize, int userId, int status, string name){
            var result =
                _companyAdminService.GetCompanyAdminsByUserId(page, pageSize, userId, (StatusType) status, name);
            var count = _companyAdminService.CountCompanyAdminsByUserId(userId, (StatusType) status, name);
            var model = new PaginatedItemsViewModel<CompanyAdminCompanyDto>(
                page, pageSize, count, result);
            return Response(model);
        }

        [HttpGet]
        [Route("companyadmins/{id}/dashboard")]
        public IActionResult GetCompanyDashboard(int id){
            return Response(_companyAdminService.GetCompanyDashboardById(id));
        }

        [HttpGet]
        [Route("companyadmins/companyId={companyId:int:min(1)}/loggedUserId={loggedUserId:int:min(0)}")]
        public IActionResult IsAdminOfCompany(int companyId, int loggedUserId){
            return Response(_companyAdminService.IsAdminOfCompany(companyId, loggedUserId));
        }


        [HttpPost]
        [Route("companyadmins")]
        public IActionResult AddCompanyAdmin([FromBody] CompanyAdminViewModel model){
            _companyAdminService.AddCompanyAdmin(model);
            return Response(null, "CompanyAdmin has been created");
        }

        [HttpPut]
        [Route("companyadmins/{id}")]
        public IActionResult UpdateCompanyAdmin(int id, [FromBody] CompanyAdminViewModel model){
            _companyAdminService.UpdateCompanyAdmin(id, model);
            return Response(null, "CompanyAdmin has been updated");
        }

        [HttpDelete]
        [Route("companyadmins/{id}")]
        public IActionResult RemoveCompanyAdmin(int id){
            _companyAdminService.RemoveCompanyAdmin(id);
            return Response(null, "CompanyAdmin has been deleted");
        }
    }
}