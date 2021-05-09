using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tragate.Application;
using Tragate.Application.ViewModels;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Core.Notifications;

namespace Tragate.WebApi.Controllers
{
    public class QuoteController : ApiController
    {
        private readonly IQuoteService _quoteService;
        private readonly IQuoteHistoryService _quoteHistoryService;
        private readonly IQuoteProductService _quoteProductService;

        public QuoteController(INotificationHandler<DomainNotification> notifications, IQuoteService quoteService,
            IQuoteHistoryService quoteHistoryService, IQuoteProductService quoteProductService) :
            base(notifications){
            _quoteService = quoteService;
            _quoteHistoryService = quoteHistoryService;
            _quoteProductService = quoteProductService;
        }

        [HttpGet]
        [Route("quotes/page={page:int:min(1)}/pageSize={pageSize:int:max(10)}")]
        public IActionResult GetQuotes(int page, int pageSize, int quoteStatus, int orderStatus,
            int? sellerCompanyId, int? buyerUserId){
            var result = _quoteService.GetQuotes(page, pageSize, (QuoteStatusType) quoteStatus,
                (OrderStatusType) orderStatus, null, sellerCompanyId, buyerUserId);
            var count = _quoteService.CountQuotes((QuoteStatusType) quoteStatus, (OrderStatusType) orderStatus, null,
                sellerCompanyId, buyerUserId);
            var model = new PaginatedItemsViewModel<QuoteListDto>(
                page, pageSize, count, result);
            return Response(model);
        }

        [HttpGet]
        [Route("quotes/{id:int:min(1)}")]
        public IActionResult GetQuoteById(int id){
            return Response(_quoteService.GetQuoteById(id));
        }

        [HttpGet]
        [Route("quotes/{id:int:min(1)}/quote-histories")]
        public IActionResult GetQuoteHistoriesById(int id){
            return Response(_quoteHistoryService.GetQuoteHistoriesById(id));
        }

        [HttpGet]
        [Route("quotes/{id:int:min(1)}/quote-products")]
        public IActionResult GetQuoteProductsById(int id){
            return Response(_quoteProductService.GetQuoteProductsById(id));
        }

        [HttpPost]
        [Route("quotes")]
        public IActionResult CreateQuote([FromBody] CreateQuoteViewModel model){
            _quoteService.CreateQuote(model);
            return Response(null, "Quote has been created");
        }

        [HttpPatch]
        [Route("quotes/{id}/quotes-contact-status")]
        public IActionResult UpdateQuoteContactStatus(int id, int buyerContactStatusId, int sellerContactStatusId){
            _quoteService.UpdateQuoteContactStatus(id, buyerContactStatusId, sellerContactStatusId);
            return Response(null, "Quote contact status has been updated");
        }

        [HttpPatch]
        [Route("quotes/{id}/quote-status")]
        public IActionResult UpdateQuoteStatus(int id, int quoteStatusId, int orderStatusId){
            _quoteService.UpdateQuoteStatus(id, quoteStatusId, orderStatusId);
            return Response(null, "Quote status has been updated");
        }
    }
}