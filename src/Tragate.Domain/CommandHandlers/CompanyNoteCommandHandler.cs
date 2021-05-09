using AutoMapper;
using MediatR;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Commands;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Core.Notifications;
using Tragate.Domain.Interfaces;
using Tragate.Domain.Models;

namespace Tragate.Domain.CommandHandlers
{
    public class CompanyNoteCommandHandler : CommandHandler,
        INotificationHandler<AddNewCompanyNoteCommand>,
        INotificationHandler<DeleteCompanyNoteCommand>
    {
        private readonly ICompanyNoteRepository _companyNoteRepository;
        private readonly IMapper _mapper;

        public CompanyNoteCommandHandler(IUnitOfWork uow, IMediatorHandler bus,
            INotificationHandler<DomainNotification> notifications, ICompanyNoteRepository companyNoteRepository,
            IMapper mapper) :
            base(uow, bus, notifications){
            _companyNoteRepository = companyNoteRepository;
            _mapper = mapper;
        }

        public void Handle(AddNewCompanyNoteCommand message){
            base.Validate(message);
            if (message.IsValid()){
                var note = _mapper.Map<CompanyNote>(message);
                _companyNoteRepository.Add(note);
                base.Commit();
            }
        }

        public void Handle(DeleteCompanyNoteCommand message){
            base.Validate(message);
            if (message.IsValid()){
                var note = _companyNoteRepository.GetById(message.Id);
                note.StatusId = (int) StatusType.Deleted;
                _companyNoteRepository.Update(note);
                base.Commit();
            }
        }
    }
}