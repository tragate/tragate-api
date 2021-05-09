using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tragate.Application;
using Tragate.Application.ViewModels;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Core.Notifications;

namespace Tragate.WebApi.Controllers
{
    public class CompanyDataController : ApiController
    {
        private readonly ICompanyDataService _companyDataService;
        private readonly ISearchService _searchService;

        public CompanyDataController(
            INotificationHandler<DomainNotification> notifications,
            ICompanyDataService companyDataService, ISearchService searchService) : base(notifications){
            _companyDataService = companyDataService;
            _searchService = searchService;
        }

        [HttpGet]
        [Route("companydata/page={page:int:min(1)}/pageSize={pageSize:int:max(25)}")]
        public IActionResult GetCompaniesDataByStatus(int page, int pageSize, int? status, string name,
            int? companyId){
            var result = _searchService.SearchCompanyData(page, pageSize, name, status, companyId);
            var model = new PaginatedItemsViewModel<CompanyDataSearchDto>(
                page, pageSize, result.Total, result.Documents);

            return Response(model);
        }

        [HttpGet]
        [Route("companydata/id={id:int:min(1)}")]
        public IActionResult GetCompanyDataById(int id){
            return Response(_companyDataService.GetCompanyDataById(id));
        }

        [HttpPatch]
        [Route("companydata")]
        public IActionResult UpdateCompanyDataStatus([FromBody] CompanyDataStatusViewModel model){
            _companyDataService.UpdateStatusOfCompanyData(model);
            return Response(null, "Firma Bilgisi Durumu başarıyla güncellendi");
        }

        [HttpPut]
        [Route("companydata")]
        public IActionResult UpdateCompanyData([FromBody] CompanyDataViewModel model){
            _companyDataService.UpdateCompanyData(model);
            return Response(null, "Firma Bilgileri başarıyla güncellendi");
        }
    }
}