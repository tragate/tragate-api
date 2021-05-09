using System.Collections.Generic;
using Tragate.Common.Library.Dto;
using Tragate.Domain.Interfaces;

namespace Tragate.Application
{
    public class QuoteProductService : IQuoteProductService
    {
        private readonly IQuoteProductRepository _quoteProductRepository;

        public QuoteProductService(IQuoteProductRepository quoteProductRepository){
            _quoteProductRepository = quoteProductRepository;
        }

        public IEnumerable<QuoteProductDto> GetQuoteProductsById(int id){
            return _quoteProductRepository.GetQuoteProductsById(id);
        }
    }
}