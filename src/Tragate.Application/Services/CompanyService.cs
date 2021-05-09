using System;
using System.Collections.Generic;
using AutoMapper;
using Tragate.Application.ViewModels;
using Tragate.Common.Library;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Commands;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Interfaces;

namespace Tragate.Application
{
    public class CompanyService : ICompanyService
    {
        private readonly IMapper _mapper;
        private readonly ICompanyRepository _companyRepository;
        private readonly IMediatorHandler _bus;

        public CompanyService(
            IMapper mapper,
            IMediatorHandler bus,
            ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
            _bus = bus;
        }

        public CompanyDto GetCompanyDetailById(int id)
        {
            return _companyRepository.GetCompanyDetailById(id);
        }

        public CompanyDto GetCompanyDetailBySlug(string slug)
        {
            return _companyRepository.GetCompanyDetailBySlug(slug);
        }

        public IEnumerable<CompanyDto> GetCompaniesByStatus(int page, int pageSize, string name, StatusType status,
            int? categoryGroupId)
        {
            return _companyRepository.GetCompaniesByStatus(page, pageSize, name, status, categoryGroupId);
        }

        public IEnumerable<CompanySiteMapDto> GetCompanySiteMap()
        {
            return _companyRepository.GetCompanySiteMap();
        }

        public int CountCompaniesByStatus(string name, StatusType status, int? categoryGroupId)
        {
            return _companyRepository.CountCompaniesByStatus(name, status, categoryGroupId);
        }

        public void AddCompany(CompanyViewModel model)
        {
            var registerCommand = _mapper.Map<RegisterNewCompanyCommand>(model);
            _bus.SendCommand(registerCommand);
        }

        public void UpdateCompany(int id, CompanyViewModel model)
        {
            var updateCommand = _mapper.Map<UpdateCompanyCommand>(model);
            updateCommand.Id = id;
            _bus.SendCommand(updateCommand);
        }

        public void UpdateOwnerAndContactUser(int id, int ownerUserId, int contactUserId)
        {
            var company = _companyRepository.GetByUserId(id);
            company.OwnerUserId = ownerUserId;
            company.ContactUserId = contactUserId;

            _companyRepository.Update(company);
            _companyRepository.SaveChanges();
        }

        public void RemoveCompany(int id)
        {
            var removeCommand = new RemoveCompanyCommand() { Id = id };
            _bus.SendCommand(removeCommand);
        }

        public void AddCompanyFast(CompanyFastAddViewModel model)
        {
            var registerCommand = _mapper.Map<RegisterFastNewCompanyCommand>(model);
            _bus.SendCommand(registerCommand);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}