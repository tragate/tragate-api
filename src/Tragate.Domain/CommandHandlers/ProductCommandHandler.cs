using System;
using AutoMapper;
using MediatR;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Commands;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Core.Notifications;
using Tragate.Domain.Events;
using Tragate.Domain.Interfaces;
using Tragate.Domain.Models;

namespace Tragate.Domain.CommandHandlers
{
    public class ProductCommandHandler : CommandHandler,
        INotificationHandler<AddNewProductCommand>,
        INotificationHandler<UpdateProductCommand>,
        INotificationHandler<DeleteProductCommand>,
        INotificationHandler<UpdateStatusProductCommand>,
        INotificationHandler<UpdateDefaultProductListImageCommand>,
        INotificationHandler<UpdateProductListImageCommand>,
        INotificationHandler<RemoveProductListImageCommand>,
        INotificationHandler<UpdateCategoryProductCommand>
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public ProductCommandHandler(IMapper mapper,
            IUnitOfWork uow,
            IMediatorHandler bus,
            INotificationHandler<DomainNotification> notifications,
            IProductRepository productRepository) : base(uow,
            bus, notifications){
            _mapper = mapper;
            _productRepository = productRepository;
        }

        //TODO:(Historical log için)ProductRegisteredEvent sadece Id olarak değil tüm mesajı map et !
        public void Handle(AddNewProductCommand message){
            base.Validate(message);
            if (!message.IsValid()) return;
            var product = _mapper.Map<Product>(message);
            var result = _productRepository.Add(product, message.TagIds);
            if (result){
                var productRegisteredEvent = new ProductRegisteredEvent() {Id = product.Id};
                base.RaiseEvent(productRegisteredEvent);
            }
        }

        //TODO:(Historical log için)ProductRegisteredEvent sadece Id olarak değil tüm mesajı map et !
        public void Handle(UpdateProductCommand message){
            base.Validate(message);
            if (!message.IsValid()) return;
            var product = _productRepository.GetById(message.Id);
            _mapper.Map(message, product);
            var result = _productRepository.Update(product, message.TagIds);
            if (result){
                var productUpdateEvent = new ProductUpdatedEvent() {Id = product.Id};
                base.RaiseEvent(productUpdateEvent);
            }
        }

        public void Handle(DeleteProductCommand message){
            base.Validate(message);
            if (!message.IsValid()) return;
            var product = _productRepository.GetById(message.Id);
            product.StatusId = (int) StatusType.Deleted;
            if (base.Commit()){
                var productUpdateEvent = new ProductUpdatedEvent() {Id = product.Id};
                base.RaiseEvent(productUpdateEvent);
            }
        }

        public void Handle(UpdateStatusProductCommand message){
            base.Validate(message);
            if (message.IsValid()){
                var product = _productRepository.GetById(message.Id);
                _mapper.Map(message, product);
                _productRepository.Update(product);
                if (base.Commit()){
                    base.RaiseEvent(new ProductUpdatedEvent()
                    {
                        Id = message.Id
                    });
                }
            }
        }

        /// <summary>
        /// TODO:vitrin görseli hem default image hem de istege baglı güncellenirse olmaz
        /// null değilse güncelleme dedikten sonra istege bazlı null olmayacagından business komplekslesir
        /// handler'ın ayrılması lazım command message'a göre
        /// ayrıca AddNewProductImageCommand UuId ile image ve vitrin görseli alınmalıydı çünkü product image command
        /// handler product domain'ine giremez messagelar ile haberleşmesi saglanmalı
        /// </summary>
        /// <param name="message"></param>
        public void Handle(UpdateDefaultProductListImageCommand message){
            base.Validate(message);
            if (!message.IsValid()) return;
            var product = _productRepository.GetById(message.Id);
            if (string.IsNullOrEmpty(product.ListImagePath)){
                _mapper.Map(message, product);
                _productRepository.Update(product);
                if (base.Commit()){
                    base.RaiseEvent(new ProductUpdatedEvent()
                    {
                        Id = message.Id
                    });
                }
            }
        }

        public void Handle(UpdateProductListImageCommand message){
            base.Validate(message);
            if (!message.IsValid()) return;
            var product = _productRepository.GetById(message.Id);
            _mapper.Map(message, product);
            _productRepository.Update(product);
            if (base.Commit()){
                base.RaiseEvent(new ProductUpdatedEvent()
                {
                    Id = message.Id
                });
            }
        }

        /// <summary>
        /// TODO:Vitrin görseli ile ilgili geliştirme yapılana kadar hangi image'ın vitrin görseli oldugunu
        /// anlayıp bus'a remove command'ı raise ediliyor.Geçici olarak yaptık refactor edilecek 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(RemoveProductListImageCommand message){
            base.Validate(message);
            if (!message.IsValid()) return;
            var product = _productRepository.GetById(message.Id);
            if (product.ListImagePath == message.SmallImagePath){
                product.ListImagePath = null;
                product.UpdatedDate = DateTime.Now;
                _productRepository.Update(product);
                if (base.Commit()){
                    base.RaiseEvent(new ProductUpdatedEvent()
                    {
                        Id = message.Id
                    });
                }
            }
        }

        public void Handle(UpdateCategoryProductCommand message){
            base.Validate(message);
            if (!message.IsValid()) return;
            var product = _productRepository.GetById(message.Id);
            _mapper.Map(message, product);
            _productRepository.Update(product);
            if (base.Commit()){
                base.RaiseEvent(new ProductUpdatedEvent()
                {
                    Id = message.Id
                });
            }
        }
    }
}