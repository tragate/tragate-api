using System.Runtime.InteropServices.ComTypes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tragate.Application;
using Tragate.Application.ViewModels;
using Tragate.Common.Library.Dto;
using Tragate.Domain.Core.Notifications;

namespace Tragate.WebApi.Controllers
{
    public class CompanyNoteController : ApiController
    {
        private readonly ICompanyNoteService _companyNoteService;

        public CompanyNoteController(INotificationHandler<DomainNotification> notifications,
            ICompanyNoteService companyNoteService) : base(notifications){
            _companyNoteService = companyNoteService;
        }

        [HttpGet]
        [Route("company-notes/page={page:int:min(1)}/pageSize={pageSize:int:max(10)}")]
        public IActionResult GetCompanyNotes(int page, int pageSize, int? status){
            var result = _companyNoteService.GetCompanyNotes(page, pageSize, status);
            var count = _companyNoteService.CountCompanyNotes(status);
            var model = new PaginatedItemsViewModel<CompanyNoteDto>(
                page, pageSize, count, result);
            return Response(model);
        }

        [HttpPost]
        [Route("company-notes")]
        public IActionResult AddCompanyNote([FromBody] CompanyNoteViewModel model){
            _companyNoteService.AddCompanyNote(model);
            return Response(null, "Company note has been added");
        }


        [HttpDelete]
        [Route("company-notes/{id}")]
        public IActionResult DeleteCompanyNote(int id){
            _companyNoteService.DeleteCompanyNote(id);
            return Response(null, "Company note has been deleted");
        }
    }
}