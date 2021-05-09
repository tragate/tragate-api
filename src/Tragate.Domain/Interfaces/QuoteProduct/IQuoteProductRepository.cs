using System.Collections.Generic;
using Tragate.Common.Library.Dto;
using Tragate.Domain.Models;

namespace Tragate.Domain.Interfaces
{
    public interface IQuoteProductRepository : IRepository<QuoteProduct>
    {
        IEnumerable<QuoteProductDto> GetQuoteProductsById(int id);
    }
}