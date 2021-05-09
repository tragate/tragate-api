using System.Linq;
using MediatR;
using Microsoft.Extensions.Options;
using Tragate.Common.Library;
using Tragate.Common.Library.Helpers;
using Tragate.Domain.Commands;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Core.Infrastructure;
using Tragate.Domain.Core.Notifications;
using Tragate.Domain.Events.ProductImage;
using Tragate.Domain.Interfaces;

namespace Tragate.Domain.CommandHandlers
{
    public class ProductImageCommandHandler : CommandHandler,
        INotificationHandler<AddNewProductImageCommand>,
        INotificationHandler<DeleteProductImageCommand>
    {
        private readonly IProductImageRepository _productImageRepository;
        private readonly IProductRepository _productRepository;
        private readonly ConfigSettings _settings;
        private readonly IFileUploadService _fileUploadService;


        public ProductImageCommandHandler(IUnitOfWork uow,
            IMediatorHandler bus,
            INotificationHandler<DomainNotification> notifications,
            IProductImageRepository productImageRepository,
            IOptions<ConfigSettings> settings,
            IProductRepository productRepository,
            IFileUploadService fileUploadService) : base(uow, bus, notifications){
            _productImageRepository = productImageRepository;
            _productRepository = productRepository;
            _fileUploadService = fileUploadService;
            _settings = settings.Value;
        }

        /// <summary>
        /// Yük altında sorun olursa js kendoupload chunk olarak değiştirilebilir.
        /// AWS lambda ile resize işlemi gerçekleşmektedir.
        /// small image resize edildikten sonra 1. image listimagepath olarak belirlenebilir.
        /// </summary>
        ///
        /// //TODO:_fileService.Upload çalışmalı sonucunda event raise edilmeli.
        public void Handle(AddNewProductImageCommand message){
            base.Validate(message);
            if (!message.IsValid()) return;
            var product = _productRepository.GetProductIdByUuId(message.UuId);
            var files = message.Files.ConvertImageFile(product.Slug);
            var result = _fileUploadService.Upload(files, _settings.S3.UploadPath);
            result.ContinueWith((x) =>
            {
                if (!x.IsFaulted){
                    base.SendCommand(new UpdateDefaultProductListImageCommand()
                    {
                        Id = product.Id,
                        ListImagePath = files.First().Key,
                        UpdatedUserId = message.UpdatedUserId
                    });

                    base.RaiseEvent(new ProductImageUploadedEvent()
                    {
                        ProductId = product.Id,
                        Files = files
                    });
                }
            });
        }

        /// <summary>
        /// TODO: Vitrin görseli ile ilgili geliştirme yapılana kadar hangi image'ın vitrin görseli oldugunu
        /// anlayıp bus'a remove command'ı raise ediliyor.Geçici olarak yaptık refactor edilecek 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(DeleteProductImageCommand message){
            base.Validate(message);
            var image = _productImageRepository.GetById(message.Id);
            if (image == null){
                base.RaiseEvent(new DomainNotification("DeleteProductImageCommand", "Image doesn't exits"));
                return;
            }

            if (message.IsValid()){
                var result = _fileUploadService.Delete(_settings.S3.FullImagePath, image.SmallImagePath.GetOldImagePath());
                result.ContinueWith((x) =>
                {
                    if (!x.IsFaulted){
                        base.SendCommand(new RemoveProductListImageCommand()
                        {
                            Id = image.ProductId,
                            SmallImagePath = image.SmallImagePath
                        });

                        base.RaiseEvent(new ProductImageDeletedEvent()
                        {
                            Id = message.Id
                        });
                    }
                });
            }
        }
    }
}