using System.Collections.Generic;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Models;

namespace Tragate.Domain.Interfaces
{
    public interface IQuoteRepository : IRepository<Quote>
    {
        IEnumerable<QuoteListDto> GetQuotes(int page, int pageSize, QuoteStatusType quoteStatus,
            OrderStatusType orderStatus,
            int? sellerUserId = null,
            int? sellerCompanyId = null,
            int? buyerUserId = null, int? buyerCompanyId = null);

        int CountQuotes(QuoteStatusType quoteStatus, OrderStatusType orderStatus, int? sellerUserId = null,
            int? sellerCompanyId = null, int? buyerUserId = null, int? buyerCompanyId = null);

        QuoteDto GetQuoteById(int id);
        QuoteCountDto GetNotificationCountByUserId(int id);
        Quote Add(User user, Quote quote, QuoteProduct quoteProduct);
    }
}