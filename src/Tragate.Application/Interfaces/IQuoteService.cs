using System;
using System.Collections.Generic;
using Tragate.Application.ViewModels;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;

namespace Tragate.Application
{
    public interface IQuoteService : IDisposable
    {
        IEnumerable<QuoteListDto> GetQuotes(int page, int pageSize, QuoteStatusType quoteStatus,
            OrderStatusType orderStatus, int? sellerUserId = null, int? sellerCompanyId = null,
            int? buyerUserId = null, int? buyerCompanyId = null);

        int CountQuotes(QuoteStatusType quoteStatus,
            OrderStatusType orderStatus, int? sellerUserId = null, int? sellerCompanyId = null,
            int? buyerUserId = null, int? buyerCompanyId = null);

        IEnumerable<QuoteListDto> GetQuotesByUserId(int page, int pageSize, QuoteStatusType quoteStatus,
            OrderStatusType orderStatus, int? sellerUserId = null, int? buyerUserId = null);

        int CountQuotesByUserId(QuoteStatusType quoteStatus, OrderStatusType orderStatus, int? sellerUserId = null,
            int? buyerUserId = null);

        IEnumerable<QuoteListDto> GetQuotesByCompanyId(int page, int pageSize, QuoteStatusType quoteStatus,
            OrderStatusType orderStatus, int? sellerCompanyId = null, int? buyerCompanyId = null);

        int CountQuotesByCompanyId(QuoteStatusType quoteStatus, OrderStatusType orderStatus,
            int? sellerCompanyId = null, int? buyerCompanyId = null);

        QuoteDto GetQuoteById(int id);
        QuoteCountDto GetNotificationCountByUserId(int id);
        void CreateQuote(CreateQuoteViewModel model);

        void UpdateQuoteContactStatus(int id, int buyerContactStatusId,
            int sellerContactStatusId);

        void UpdateQuoteStatus(int id, int quoteStatusId, int orderStatusId);
    }
}