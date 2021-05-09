using AutoMapper;
using MediatR;
using Tragate.Common.Library.Enum;
using Tragate.Common.Library.Helpers;
using Tragate.Domain.Commands;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Core.Notifications;
using Tragate.Domain.Interfaces;
using Tragate.Domain.Models;

namespace Tragate.Domain.CommandHandlers {
    public class ContentCommandHandler : CommandHandler,
        INotificationHandler<AddNewContentCommand>,
        INotificationHandler<UpdateContentCommand> {
            private readonly IContentRepository _contentRepository;
            private readonly IMapper _mapper;

            public ContentCommandHandler (IMapper mapper,
                IContentRepository contentRepository,
                IUnitOfWork uow,
                IMediatorHandler bus,
                INotificationHandler<DomainNotification> notifications) : base (uow, bus, notifications) {
                _contentRepository = contentRepository;
                _mapper = mapper;
            }

            public void Handle (AddNewContentCommand message) {
                base.Validate (message);
                if (message.IsValid ()) {
                    if (_contentRepository.GetBySlug (message.Title.GenerateSlug (), (int) StatusType.Active) != null) {
                        base.RaiseEvent (new DomainNotification ("AddNewContentCommand", "Content already exists"));
                    }

                    var entity = _mapper.Map<Content> (message);
                    _contentRepository.Add (entity);
                    base.Commit ();
                }
            }

            public void Handle (UpdateContentCommand message) {
                base.Validate (message);
                if (message.IsValid ()) {
                    var content = _contentRepository.GetBySlug (message.Title.GenerateSlug (), (int) StatusType.Active);
                    if (content != null && content.Id != message.Id) {
                        base.RaiseEvent (new DomainNotification ("UpdateContentCommand", "Content already exists"));
                    }

                    var entity = _contentRepository.GetById (message.Id);
                    _mapper.Map (message, entity);
                    _contentRepository.Update (entity);
                    base.Commit ();
                }
            }
        }
}