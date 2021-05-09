using System.Collections.Generic;
using Tragate.Common.Library;
using Tragate.Domain.Models;

namespace Tragate.Domain.Interfaces
{
    public interface IQuoteHistoryRepository : IRepository<QuoteHistory>
    {
        IEnumerable<QuoteHistoryDto> GetQuoteHistoriesById(int id);
    }
}