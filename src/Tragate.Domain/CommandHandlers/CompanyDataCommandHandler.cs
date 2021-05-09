using AutoMapper;
using MediatR;
using Tragate.Domain.Commands;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Core.Notifications;
using Tragate.Domain.Events.CompanyData;
using Tragate.Domain.Interfaces;

namespace Tragate.Domain.CommandHandlers
{
    public class CompanyDataCommandHandler : CommandHandler,
        INotificationHandler<UpdateCompanyDataCommand>,
        INotificationHandler<UpdateReferenceCompanyDataCommand>
    {
        private readonly ICompanyDataRepository _companyDataRepository;
        private readonly IMapper _mapper;

        public CompanyDataCommandHandler(IMapper mapper,
            ICompanyDataRepository companyDataRepository,
            IUnitOfWork uow,
            IMediatorHandler bus,
            INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications){
            _companyDataRepository = companyDataRepository;
            _mapper = mapper;
        }

        public void Handle(UpdateCompanyDataCommand message){
            base.Validate(message);
            if (message.IsValid()){
                var entity = _companyDataRepository.GetById(message.Id);
                _mapper.Map(message, entity);
                _companyDataRepository.Update(entity);
                base.Commit();
            }
        }

        public void Handle(UpdateReferenceCompanyDataCommand message){
            var entity = _companyDataRepository.GetById(message.Id);
            _mapper.Map(message, entity);
            _companyDataRepository.Update(entity);
            if (base.Commit()){
                base.RaiseEvent(new CompanyDataReferenceUpdatedEvent()
                {
                    Id = message.Id
                });
            }
        }
    }
}