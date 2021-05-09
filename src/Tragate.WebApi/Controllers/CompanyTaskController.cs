using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tragate.Application;
using Tragate.Application.ViewModels;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Core.Notifications;

namespace Tragate.WebApi.Controllers
{
    public class CompanyTaskController : ApiController
    {
        private readonly ICompanyTaskService _companyTaskService;

        public CompanyTaskController(INotificationHandler<DomainNotification> notifications,
            ICompanyTaskService companyTaskService) : base(notifications){
            _companyTaskService = companyTaskService;
        }

        [HttpGet]
        [Route("company-tasks/page={page:int:min(1)}/pageSize={pageSize:int:max(10)}")]
        public IActionResult GetCompanyTasks(int page, int pageSize, int status, int? companyId, int? responsibleUserId,
            int? createdUserId){
            var result = _companyTaskService.GetCompanyTasks(page, pageSize, (StatusType) status, companyId,
                responsibleUserId, createdUserId);
            var count = _companyTaskService.CountCompanyTasks((StatusType) status, companyId, responsibleUserId,
                createdUserId);
            var model = new PaginatedItemsViewModel<CompanyTaskDto>(
                page, pageSize, count, result);

            return Response(model);
        }

        [HttpPost]
        [Route("company-tasks")]
        public IActionResult AddCompanyTask([FromBody] CompanyTaskViewModel model){
            _companyTaskService.AddCompanyTask(model);
            return Response(null, "Your task has been added");
        }

        [HttpPatch]
        [Route("company-tasks/{id:int:min(1)}/status")]
        public IActionResult UpdateStatusCompanyTask(int id, [FromBody] CompanyTaskStatusViewModel model){
            _companyTaskService.UpdateStatusCompanyTask(id, model);
            return Response(null, "Your task status has been updated");
        }

        [HttpDelete]
        [Route("company-tasks/id={id:int:min(1)}")]
        public IActionResult DeleteCompanyTask(int id){
            _companyTaskService.DeleteCompanyTask(id);
            return Response(null, "Your task has been deleted");
        }
    }
}