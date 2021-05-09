using MediatR;
using Nest;
using Tragate.Domain.EventHandlers.Base;
using Tragate.Domain.Events;
using Tragate.Domain.Interfaces;

namespace Tragate.Domain.EventHandlers
{
    public class ProductEventHandler : BaseProductEventHandler,
        INotificationHandler<ProductRegisteredEvent>,
        INotificationHandler<ProductUpdatedEvent>
    {
        public ProductEventHandler(ElasticClient elasticClient,
            IProductRepository productRepository) : base(
            elasticClient, productRepository){
        }

        /// <summary>
        /// TODO:Command message can map to event message for tracking historical log
        /// child product
        /// </summary>
        /// <param name="message"></param>
        public void Handle(ProductRegisteredEvent message){
            base.IndexDocument(message.Id);
        }

        /// <summary>
        /// TODO:Command message can map to event message for tracking historical log
        /// /// child product
        /// </summary>
        /// <param name="message"></param>
        public void Handle(ProductUpdatedEvent message){
            base.IndexDocument(message.Id);
        }
    }
}