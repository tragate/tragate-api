using System.Collections.Generic;
using Tragate.Common.Library.Dto;

namespace Tragate.Application
{
    public interface IQuoteProductService
    {
        IEnumerable<QuoteProductDto> GetQuoteProductsById(int id);
    }
}