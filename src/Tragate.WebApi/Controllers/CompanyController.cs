using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tragate.Application;
using Tragate.Application.ViewModels;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Core.Notifications;

namespace Tragate.WebApi.Controllers
{
    public class CompanyController : ApiController
    {
        private readonly ICompanyService _companyService;
        private readonly IProductService _productService;
        private readonly ICompanyMembershipService _companyMembershipService;
        private readonly ICompanyNoteService _companyNoteService;
        private readonly ICompanyTaskService _companyTaskService;
        private readonly IQuoteService _quoteService;

        public CompanyController(
            INotificationHandler<DomainNotification> notifications,
            ICompanyService companyService,
            IProductService productService,
            ICompanyNoteService companyNoteService,
            ICompanyMembershipService companyMembershipService,
            ICompanyTaskService companyTaskService,
            IQuoteService quoteService) : base(notifications)
        {
            _companyService = companyService;
            _productService = productService;
            _companyNoteService = companyNoteService;
            _companyMembershipService = companyMembershipService;
            _companyTaskService = companyTaskService;
            _quoteService = quoteService;
        }

        [HttpGet]
        [Route("companies/page={page:int:min(1)}/pageSize={pageSize:int:max(20)}")]
        public IActionResult GetCompaniesByStatus(int page, int pageSize, int status, string name,
            int? categoryGroupId)
        {
            var result =
                _companyService.GetCompaniesByStatus(page, pageSize, name, (StatusType)status, categoryGroupId);
            var count = _companyService.CountCompaniesByStatus(name, (StatusType)status, categoryGroupId);
            var model = new PaginatedItemsViewModel<CompanyDto>(
                page, pageSize, count, result);

            return Response(model);
        }

        [HttpGet]
        [Route("companies/{id}/products/page={page:int:min(1)}/pageSize={pageSize:int:max(28)}")]
        public IActionResult GetProductsByCompanyId(int id, int page, int pageSize, string name, int status)
        {
            var result = _productService.GetProductsByCompanyId(id, page, pageSize, name, (StatusType)status);
            var count = _productService.CountProductsByCompanyId(id, name, (StatusType)status);
            var model = new PaginatedItemsViewModel<CompanyProductDto>(
                page, pageSize, count, result);

            return Response(model);
        }

        [HttpGet]
        [Route("companies/{id}/company-memberships")]
        public IActionResult GetCompanyMembershipsById(int id)
        {
            return Response(_companyMembershipService.GetCompanyMembershipsByCompanyId(id));
        }

        [HttpGet]
        [Route("companies/{id}/company-notes/page={page:int:min(1)}/pageSize={pageSize:int:max(10)}")]
        public IActionResult GetCompanyNotesByCompanyId(int id, int page, int pageSize, int? status)
        {
            var result = _companyNoteService.GetCompanyNotesByCompanyId(id, page, pageSize, status);
            var count = _companyNoteService.CountCompanyNotesByCompanyId(id, status);
            var model = new PaginatedItemsViewModel<CompanyNoteDto>(
                page, pageSize, count, result);
            return Response(model);
        }

        [HttpGet]
        [Route("companies/{id}/company-tasks/page={page:int:min(1)}/pageSize={pageSize:int:max(10)}")]
        public IActionResult GetCompanyTasksByCompanyId(int id, int page, int pageSize, int status)
        {
            var result = _companyTaskService.GetCompanyTasksByCompanyId(id, page, pageSize, (StatusType)status);
            var count = _companyTaskService.CountCompanyTasksByCompanyId(id, (StatusType)status);
            var model = new PaginatedItemsViewModel<CompanyTaskDto>(
                page, pageSize, count, result);
            return Response(model);
        }

        [HttpGet]
        [Route("companies/{id}/buyer-quotes/page={page:int:min(1)}/pageSize={pageSize:int:max(10)}")]
        public IActionResult
            GetQuotesByBuyerCompanyId(int id, int page, int pageSize, int quoteStatus, int orderStatus)
        {
            var result = _quoteService.GetQuotesByCompanyId(page, pageSize, (QuoteStatusType)quoteStatus,
                (OrderStatusType)orderStatus, null, id);
            var count = _quoteService.CountQuotesByCompanyId((QuoteStatusType)quoteStatus,
                (OrderStatusType)orderStatus, null, id);
            var model = new PaginatedItemsViewModel<QuoteListDto>(
                page, pageSize, count, result);
            return Response(model);
        }

        [HttpGet]
        [Route("companies/{id}/seller-quotes/page={page:int:min(1)}/pageSize={pageSize:int:max(10)}")]
        public IActionResult GetQuotesBySellerCompanyId(int id, int page, int pageSize, int quoteStatus,
            int orderStatus)
        {
            var result = _quoteService.GetQuotesByCompanyId(page, pageSize, (QuoteStatusType)quoteStatus,
                (OrderStatusType)orderStatus, id);
            var count = _quoteService.CountQuotesByCompanyId((QuoteStatusType)quoteStatus,
                (OrderStatusType)orderStatus, id);
            var model = new PaginatedItemsViewModel<QuoteListDto>(
                page, pageSize, count, result);
            return Response(model);
        }

        [HttpGet]
        [Route("companies/id={id:int:min(1)}")]
        public IActionResult GetCompanyById(int id)
        {
            return Response(_companyService.GetCompanyDetailById(id));
        }

        [HttpGet]
        [Route("companies/{slug}")]
        public IActionResult GetCompanyBySlug(string slug)
        {
            return Response(_companyService.GetCompanyDetailBySlug(slug));
        }

        [HttpGet]
        [Route("companies/sitemap")]
        public IActionResult GetCompanySiteMap()
        {
            return Response(_companyService.GetCompanySiteMap());
        }

        [HttpPost]
        [Route("companies")]
        public IActionResult AddCompany([FromBody] CompanyViewModel model)
        {
            _companyService.AddCompany(model);
            return Response(null, "Firma başarıyla oluşturuldu");
        }

        [HttpPut]
        [Route("companies/id={id:int:min(1)}")]
        public IActionResult UpdateCompany(int id, [FromBody] CompanyViewModel model)
        {
            _companyService.UpdateCompany(id, model);
            return Response(null, "Firma başarıyla güncellendi");
        }

        [HttpDelete]
        [Route("companies/id={id:int:min(1)}")]
        public IActionResult DeleteCompany(int id)
        {
            _companyService.RemoveCompany(id);
            return Response(null, "Firma başarıyla silindi");
        }

        [HttpPatch]
        [Route("companies/{id}/update-owner-and-contactUser")]
        public IActionResult UpdateOwnerAndContactUser(int id, int ownerUserId, int contactUserId)
        {
            _companyService.UpdateOwnerAndContactUser(id, ownerUserId, contactUserId);
            return Response(result: true, message: "Owner ve Contact User başarıyla güncellendi");
        }

        [HttpPost]
        [Route("companies/add-fast-company")]
        public IActionResult AddFastCompany([FromBody] CompanyFastAddViewModel model) {
            _companyService.AddCompanyFast(model);
            return Response(null, "Firma başarıyla eklendi");
        } 
    }
}