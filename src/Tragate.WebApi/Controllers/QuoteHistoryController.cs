using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tragate.Application;
using Tragate.Application.ViewModels;
using Tragate.Domain.Core.Notifications;

namespace Tragate.WebApi.Controllers
{
    public class QuoteHistoryController : ApiController
    {
        private readonly IQuoteHistoryService _quoteHistoryService;

        public QuoteHistoryController(INotificationHandler<DomainNotification> notifications,
            IQuoteHistoryService quoteHistoryService) : base(notifications){
            _quoteHistoryService = quoteHistoryService;
        }

        [HttpPost]
        [Route("quote-histories")]
        public IActionResult CreateQuoteHistory([FromBody] CreateQuoteHistoryViewModel model){
            _quoteHistoryService.CreateQuoteHistory(model);
            return Response(null, "Message has been created");
        }
    }
}    