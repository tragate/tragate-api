using Nest;
using Tragate.Common.Library.Constants;
using Tragate.Common.Library.Dto;
using Tragate.Domain.Interfaces;

namespace Tragate.Domain.EventHandlers.Base
{
    public class BaseProductEventHandler
    {
        private readonly ElasticClient _elasticClient;
        private readonly IProductRepository _productRepository;

        protected BaseProductEventHandler(ElasticClient elasticClient, IProductRepository productRepository){
            _elasticClient = elasticClient;
            _productRepository = productRepository;
        }

        protected virtual void IndexDocument(int id){
            var product = _productRepository.GetProductById(id);
            product.JoinField = JoinField.Link<ProductDto>(product.CompanyId);
            _elasticClient.Index<Root>(product, x => x.Index(TragateConstants.ALIAS));
        }
    }
}