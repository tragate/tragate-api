using AutoMapper;
using MediatR;
using Tragate.Domain.Commands;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Core.Notifications;
using Tragate.Domain.Events;
using Tragate.Domain.Interfaces;
using Tragate.Domain.Models;

namespace Tragate.Domain.CommandHandlers
{
    public class CompanyMembershipCommandHandler : CommandHandler,
        INotificationHandler<AddNewCompanyMembershipCommand>
    {
        private readonly IMapper _mapper;
        private readonly ICompanyMembershipRepository _companyMembershipRepository;

        public CompanyMembershipCommandHandler(IUnitOfWork uow,
            IMediatorHandler bus,
            INotificationHandler<DomainNotification> notifications,
            ICompanyMembershipRepository companyMembershipRepository, IMapper mapper) :
            base(uow, bus, notifications){
            _companyMembershipRepository = companyMembershipRepository;
            _mapper = mapper;
        }

        public void Handle(AddNewCompanyMembershipCommand message){
            base.Validate(message);
            if (message.IsValid()){
                if (!_companyMembershipRepository.IsExistsActiveMembershipById(message.CompanyId, message.StartDate)){
                    var companyMembership = _mapper.Map<CompanyMembership>(message);
                    _companyMembershipRepository.Add(companyMembership);
                    if (base.Commit()){
                        base.SendCommand(new UpdateCompanyMembershipCommand()
                        {
                            Id = message.CompanyId,
                            MembershipTypeId = message.MembershipTypeId,
                            MembershipPackageId = message.MembershipPackageId
                        });

                        base.RaiseEvent(new CompanyMembershipCreatedEvent() {CompanyId = message.CompanyId});
                    }
                }
                else{
                    base.RaiseError(message,
                        $"Already has a active membership between {message.StartDate} and {message.EndDate}");
                }
            }
        }
    }
}