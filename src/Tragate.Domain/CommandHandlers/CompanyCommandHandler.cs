using System;
using AutoMapper;
using MediatR;
using Tragate.Common.Library.Enum;
using Tragate.Common.Library.Helpers;
using Tragate.Domain.Commands;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Core.Notifications;
using Tragate.Domain.Events;
using Tragate.Domain.Interfaces;
using Tragate.Domain.Models;

namespace Tragate.Domain.CommandHandlers
{
    public class CompanyCommandHandler : CommandHandler,
        INotificationHandler<RegisterNewCompanyCommand>,
        INotificationHandler<UpdateCompanyCommand>,
        INotificationHandler<RemoveCompanyCommand>,
        INotificationHandler<UpdateCompanyMembershipCommand>,
        INotificationHandler<RegisterFastNewCompanyCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public CompanyCommandHandler(
            IUserRepository userRepository,
            ICompanyRepository companyRepository,
            IMapper mapper,
            IUnitOfWork uow,
            IMediatorHandler bus,
            INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)
        {
            _companyRepository = companyRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        //TODO:Refactor transaction scope into Add method that used below
        public void Handle(RegisterNewCompanyCommand message)
        {
            base.Validate(message);

            //TODO:generic olması lazım
            if (_userRepository.GetById(message.PersonId) == null)
            {
                base.RaiseEvent(new DomainNotification("RegisterNewCompanyCommand", "User not found"));
                return;
            }

            //TODO:generic olması lazım
            if (_companyRepository.GetCompanyDetailBySlug(message.FullName?.GenerateSlug()) != null)
            {
                base.RaiseEvent(new DomainNotification("RegisterNewCompanyCommand", "Company name must be unique"));
                return;
            }

            if (!message.IsValid()) return;
            var user = _mapper.Map<User>(message);
            var company = _mapper.Map<Company>(message);

            var ok = _userRepository.Add(user, company, message.CompanyCategoryIds, message.PersonId);
            if (ok)
            {
                base.RaiseEvent(new CompanyRegisteredEvent() { Id = company.UserId });
                if (message.CompanyDataId.HasValue)
                {
                    base.SendCommand(new UpdateReferenceCompanyDataCommand()
                    {
                        Id = message.CompanyDataId.Value,
                        StatusType = StatusType.Transferred,
                        CompanyId = company.UserId,
                        UpdatedDate = DateTime.Now,
                        UpdatedUserId = message.PersonId
                    });
                }
            }
        }

        //TODO:Refactor transaction scope into Add method that used below
        public void Handle(UpdateCompanyCommand message)
        {
            base.Validate(message);
            if (!message.IsValid()) return;
            var result = _companyRepository.GetCompanyDetailBySlug(message.FullName?.GenerateSlug());
            if (result != null && result.UserId != message.Id)
            {
                base.RaiseEvent(new DomainNotification("UpdateCompanyCommand", "Company name must be unique"));
                return;
            }

            var user = _userRepository.GetById(message.Id);
            var company = _companyRepository.GetByUserId(user.Id);
            _mapper.Map(message, user);
            _mapper.Map(message, company);
            var ok = _userRepository.Update(user, message.CompanyCategoryIds, company);
            if (ok)
            {
                base.RaiseEvent(new CompanyUpdatedEvent() { Id = company.UserId });
            }
        }

        public void Handle(RemoveCompanyCommand message)
        {
            base.Validate(message);
            if (!message.IsValid()) return;
            var entity = _companyRepository.GetByUserId(message.Id);
            entity.StatusId = (int)StatusType.Deleted;
            _companyRepository.Update(entity);
            if (base.Commit())
            {
                base.RaiseEvent(new CompanyUpdatedEvent() { Id = message.Id });
            }
        }

        public void Handle(UpdateCompanyMembershipCommand message)
        {
            base.Validate(message);
            if (message.IsValid())
            {
                var company = _companyRepository.GetByUserId(message.Id);
                _mapper.Map(message, company);
                _companyRepository.Update(company);
                if (base.Commit())
                {
                    base.RaiseEvent(new CompanyUpdatedEvent() { Id = message.Id });
                }
            }
        }

        public void Handle(RegisterFastNewCompanyCommand message)
        {
            base.Validate(message);
            if (!message.IsValid()) return;
            var user = _mapper.Map<User>(message);
            if (_userRepository.GetByEmail(user.Email) != null)
            {
                base.RaiseEvent(new DomainNotification("RegisterFastNewCompanyCommand", "Already exists this email"));
                return;
            }

            _userRepository.Add(user);

            if (base.Commit())
            {
                base.SendCommand(new RegisterNewCompanyCommand()
                {
                    PersonId = user.Id,
                    CountryId = message.CountryId,
                    CityId = message.CityId,
                    BusinessType = message.BusinessType,
                    CompanyCategoryIds = message.CompanyCategoryIds,
                    Email = message.CompanyEmail,
                    FullName = message.CompanyName,
                    Phone = message.CompanyPhone,
                    LocationId = message.CityId ?? 0,
                    StateId = message.StateId,
                    StatusType = StatusType.Active
                });

                var company = _companyRepository.GetByOwnerUserId(user.Id);
                if (company != null)
                {
                    base.RaiseEvent(new UserRegisteredEvent()
                    {
                        Email = message.Email,
                        FullName = message.FullName,
                        UserId = user.Id
                    });
                    base.RaiseEvent(new CompanyFastAddedEvent()
                    {
                        Email = message.Email,
                        Fullname = message.FullName
                    });

                    base.RaiseEvent(new UserForgotPasswordEvent
                    {
                        Email = message.Email,
                        UserId = user.Id
                    });
                }
                else
                {
                    base.RaiseEvent(new DomainNotification("RegisterFastNewCompanyCommand", "Something went wrong while save company"));
                    _userRepository.Remove(user.Id);
                    return;
                }
            }
            else
            {
                base.RaiseEvent(new DomainNotification("RegisterFastNewCompanyCommand", "Something went wrong while save user"));
                return;
            }
        }
    }
}