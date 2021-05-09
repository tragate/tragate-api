using System;
using System.IO;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using Tragate.Common.Library;
using Tragate.Common.Library.Helpers;
using Tragate.Domain.Commands;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Core.Infrastructure;
using Tragate.Domain.Core.Notifications;
using Tragate.Domain.Events;
using Tragate.Domain.Events.Image;
using Tragate.Domain.Interfaces;
using Tragate.Domain.Models;

namespace Tragate.Domain.CommandHandlers
{
    public class CategoryCommandHandler : CommandHandler,
        INotificationHandler<AddNewCategoryCommand>,
        INotificationHandler<UpdateCategoryCommand>,
        INotificationHandler<UploadCategoryImageCommand>
    {
        private readonly IFileUploadService _fileUploadService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ConfigSettings _settings;
        private readonly IMapper _mapper;

        public CategoryCommandHandler(
            IFileUploadService fileUploadService,
            ICategoryRepository categoryRepository,
            IOptions<ConfigSettings> settings,
            IMapper mapper,
            IUnitOfWork uow,
            IMediatorHandler bus,
            INotificationHandler<DomainNotification> notifications
        ) : base(uow, bus, notifications){
            _fileUploadService = fileUploadService;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _settings = settings.Value;
        }

        public void Handle(AddNewCategoryCommand message){
            base.Validate(message);
            if (message.ParentId.HasValue){
                if (_categoryRepository.GetById(message.ParentId.Value) == null){
                    base.RaiseEvent(new DomainNotification("AddNewCategoryCommand",
                        "Doesn't exists category with this parentId"));
                    return;
                }
            }

            if (message.IsValid()){
                var entity = _mapper.Map<Category>(message);
                _categoryRepository.Add(entity);
                base.Commit();
            }
        }

        public void Handle(UpdateCategoryCommand message){
            base.Validate(message);
            if (message.IsValid()){
                var entity = _categoryRepository.GetById(message.Id);
                _mapper.Map(message, entity);
                _categoryRepository.Update(entity);
                base.Commit();
            }
        }

        public void Handle(UploadCategoryImageCommand message){
            base.Validate(message);
            if (message.IsValid()){
                var category = _categoryRepository.GetById(message.Id);
                var fileName = Helper.GetCategoryImageName(message.UploadedFile);
                var result = _fileUploadService.Upload(message.UploadedFile, _settings.S3.UploadPath, fileName);
                result.ContinueWith(x =>
                {
                    if (!x.IsFaulted){
                        base.RaiseEvent(new CategoryImageUploadedEvent()
                        {
                            Id = message.Id,
                            ImagePath = fileName
                        });

                        base.RaiseEvent(new ImageDeletedEvent()
                        {
                            BucketName = _settings.S3.FullImagePath,
                            Key = category.ImagePath.GetOldImagePath()
                        });
                    }
                });
            }
        }
    }
}