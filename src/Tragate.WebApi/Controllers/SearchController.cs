using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tragate.Application;
using Tragate.Application.ViewModels;
using Tragate.Common.Library.Dto;
using Tragate.Domain.Core.Notifications;

namespace Tragate.WebApi.Controllers
{
    public class SearchController : ApiController
    {
        private readonly ISearchService _searchService;

        public SearchController(INotificationHandler<DomainNotification> notifications, ISearchService searchService) :
            base(notifications){
            _searchService = searchService;
        }

        [HttpGet]
        [Route("search/all/page={page:int:min(1)}/pageSize={pageSize:int:max(28)}")]
        public IActionResult SearchAll(int page, int pageSize, string key){
            var result = _searchService.SearchAll(page, pageSize, key);
            var model = new PaginatedItemsViewModel<SearchAllDto>(
                page, pageSize, result.Total, result.Documents);
            return Response(model);
        }

        [HttpGet]
        [Route("search/product/page={page:int:min(1)}/pageSize={pageSize:int:max(28)}")]
        public IActionResult SearchProduct(int page, int pageSize, string key, string categorySlug, string companySlug,
            int? categoryGroupId){
            var result = _searchService.SearchProduct(page, pageSize, key, categorySlug, companySlug, categoryGroupId);
            var model = new SearchProductPaginatedItemsViewModel(
                page, pageSize, result.Total, result.Documents, result.CategoryAggs);

            return Response(model);
        }

        [HttpGet]
        [Route("search/company/page={page:int:min(1)}/pageSize={pageSize:int:max(28)}")]
        public IActionResult SearchCompany(int page, int pageSize, string key, string categoryTag){
            var result = _searchService.SearchCompany(page, pageSize, key, categoryTag);
            var model = new PaginatedItemsViewModel<CompanySearchDto>(
                page, pageSize, result.Total, result.Documents);
            return Response(model);
        }
    }
}