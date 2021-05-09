using System;
using System.Collections.Generic;
using AutoMapper;
using Tragate.Application.ViewModels;
using Tragate.Common.Library.Dto.CompanyMembership;
using Tragate.Domain.Commands;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Interfaces;

namespace Tragate.Application
{
    public class CompanyMembershipService : ICompanyMembershipService
    {
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _bus;
        private readonly ICompanyMembershipRepository _companyMembershipRepository;

        public CompanyMembershipService(IMapper mapper, IMediatorHandler bus,
            ICompanyMembershipRepository companyMembershipRepository){
            _mapper = mapper;
            _bus = bus;
            _companyMembershipRepository = companyMembershipRepository;
        }

        public IEnumerable<CompanyMembershipDto> GetCompanyMembershipsByCompanyId(int id){
            return _companyMembershipRepository.GetCompanyMembershipsByCompanyId(id);
        }

        public void AddCompanyMembership(CompanyMembershipViewModel model){
            var registerCompand = _mapper.Map<AddNewCompanyMembershipCommand>(model);
            _bus.SendCommand(registerCompand);
        }

        public void Dispose(){
            GC.SuppressFinalize(this);
        }
    }
}