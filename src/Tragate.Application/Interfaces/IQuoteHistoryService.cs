using System;
using System.Collections.Generic;
using Tragate.Application.ViewModels;
using Tragate.Common.Library;

namespace Tragate.Application
{
    public interface IQuoteHistoryService : IDisposable
    {
        IEnumerable<QuoteHistoryDto> GetQuoteHistoriesById(int id);
        void CreateQuoteHistory(CreateQuoteHistoryViewModel model);
    }
}