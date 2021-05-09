using MediatR;
using Tragate.Domain.Events;
using Tragate.Domain.Interfaces;

namespace Tragate.Domain.EventHandlers
{
    public class CompanyMembershipEventHandler : INotificationHandler<CompanyMembershipCreatedEvent>
    {
        private readonly ICompanyMembershipRepository _companyMembershipRepository;
        private readonly EmailHandler _emailHandler;

        public CompanyMembershipEventHandler(
            EmailHandler emailHandler, ICompanyMembershipRepository companyMembershipRepository){
            _emailHandler = emailHandler;
            _companyMembershipRepository = companyMembershipRepository;
        }

        public void Handle(CompanyMembershipCreatedEvent message){
            var cm = _companyMembershipRepository.GetCompanyMembershipDetailByCompanyId(message.CompanyId);
            _emailHandler.Execute(cm.MembershipPackageType, cm);
        }
    }
}