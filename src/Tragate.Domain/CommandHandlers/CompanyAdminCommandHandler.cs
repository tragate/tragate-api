using AutoMapper;
using MediatR;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Commands;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Core.Notifications;
using Tragate.Domain.Interfaces;
using Tragate.Domain.Models;

namespace Tragate.Domain.CommandHandlers {
    public class CompanyAdminCommandHandler : CommandHandler,
        INotificationHandler<AddNewCompanyAdminCommand>,
        INotificationHandler<UpdateCompanyAdminCommand>,
        INotificationHandler<RemoveCompanyAdminCommand> {

            private readonly IMapper _mapper;
            private readonly ICompanyAdminRepository _companyAdminRepository;
            private readonly ICompanyRepository _companyRepository;
        private readonly IUserRepository _userRepository;

            public CompanyAdminCommandHandler (IMapper mapper,
                ICompanyAdminRepository companyAdminRepository,
                ICompanyRepository companyRepository,
                IUserRepository userRepository,
                IUnitOfWork uow,
                IMediatorHandler bus,
                INotificationHandler<DomainNotification> notifications) : base (uow, bus, notifications) {
                _mapper = mapper;
                _companyAdminRepository = companyAdminRepository;
                _companyRepository = companyRepository;
            _userRepository = userRepository;
            }

            public void Handle (AddNewCompanyAdminCommand message) {
                base.Validate (message);
                if (message.IsValid ()) {
                    var user = _userRepository.GetByEmail (message.Email);
                    if (user != null) {
                        message.PersonId = user.Id;
                        var companyAdmin = _mapper.Map<CompanyAdmin> (message);
                        _companyAdminRepository.Add (companyAdmin);
                        base.Commit ();
                    } else {
                        base.RaiseEvent (new DomainNotification ("AddNewCompanyAdminCommand", "User not found"));
                    }
                }
            }

            public void Handle (UpdateCompanyAdminCommand message) {
                base.Validate (message);
                if (message.IsValid ()) {
                    var user = _userRepository.GetByEmail (message.Email);
                    var company = _userRepository.GetById (message.CompanyId);
                    if (user != null && company != null && company.UserTypeId == (int) UserType.Company) {
                        message.PersonId = user.Id;
                        var companyAdmin = _companyAdminRepository.GetById (message.Id);
                        _mapper.Map (message, companyAdmin);
                        _companyAdminRepository.Update (companyAdmin);
                        base.Commit ();
                    } else {
                        base.RaiseEvent (new DomainNotification ("UpdateCompanyAdminCommand", "User or Company not found"));
                    }
                }
            }

        public void Handle(RemoveCompanyAdminCommand message)
        {
            base.Validate(message);
            if (message.IsValid())
            {
                var companyAdmin = _companyAdminRepository.GetById(message.Id);
                var company = _companyRepository.GetCompanyDetailById(companyAdmin.CompanyId);

                if (company.OwnerUserId == companyAdmin.PersonId && companyAdmin.CompanyAdminRoleId == (int)CompanyAdminRoleType.Owner)
                {
                    base.RaiseEvent(new DomainNotification("RemoveCompanyAdminCommand", "Owner user doesn't remove"));
                }
                else
                {
                    companyAdmin.StatusId = (int)StatusType.Deleted;
                    _companyAdminRepository.Update(companyAdmin);
                    base.Commit();
                }
            }
        }
    }
}