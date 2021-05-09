using MediatR;
using Tragate.Domain.Events.ProductImage;
using Tragate.Domain.Interfaces;

namespace Tragate.Domain.EventHandlers
{
    public class ProductImageEventHandler :
        INotificationHandler<ProductImageUploadedEvent>,
        INotificationHandler<ProductImageDeletedEvent>
    {
        private readonly IProductImageRepository _productImageRepository;

        public ProductImageEventHandler(IProductImageRepository productImageRepository){
            _productImageRepository = productImageRepository;
        }

        public void Handle(ProductImageUploadedEvent message){
            _productImageRepository.AddList(message.ProductId, message.Files);
        }

        public void Handle(ProductImageDeletedEvent message){
            _productImageRepository.Remove(message.Id);
            _productImageRepository.SaveChanges();
        }
    }
}