using System;
using System.Collections.Generic;
using AutoMapper;
using Tragate.Application.ViewModels;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Commands;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Core.Notifications;
using Tragate.Domain.Interfaces;

namespace Tragate.Application
{
    public class CompanyAdminService : ICompanyAdminService
    {
        private readonly IMapper _mapper;
        private readonly ICompanyAdminRepository _companyAdminRepository;
        private readonly IMediatorHandler _bus;

        public CompanyAdminService(
            IMapper mapper,
            IMediatorHandler bus,
            ICompanyAdminRepository companyAdminRepository){
            _companyAdminRepository = companyAdminRepository;
            _mapper = mapper;
            _bus = bus;
        }

        public IEnumerable<CompanyAdminUserDto> GetCompanyAdminsByCompanyId(int page, int pageSize, int companyId,
            StatusType status){
            return _companyAdminRepository.GetCompanyAdminsByCompanyId(page, pageSize, companyId, status);
        }

        public int CountCompanyAdminsByCompanyId(int companyId, StatusType status){
            return _companyAdminRepository.CountCompanyAdminsByCompanyId(companyId, status);
        }

        public IEnumerable<CompanyAdminCompanyDto> GetCompanyAdminsByUserId(int page, int pageSize, int userId,
            StatusType status, string name){
            return _companyAdminRepository.GetCompanyAdminsByUserId(page, pageSize, userId, status, name);
        }

        public int CountCompanyAdminsByUserId(int userId, StatusType status, string name){
            return _companyAdminRepository.CountCompanyAdminsByUserId(userId, status, name);
        }

        public bool IsAdminOfCompany(int companyId, int loggedUserId){
            var result = _companyAdminRepository.IsAdminOfCompany(companyId, loggedUserId);
            if (!result){
                _bus.RaiseEvent(new DomainNotification("IsAdminOfCompany", "Access denied"));
            }

            return result;
        }

        public CompanyDashboardDto GetCompanyDashboardById(int id){
            return _companyAdminRepository.GetCompanyDashboardById(id);
        }

        public void AddCompanyAdmin(CompanyAdminViewModel model){
            var registerCommand = _mapper.Map<AddNewCompanyAdminCommand>(model);
            _bus.SendCommand(registerCommand);
        }

        public void UpdateCompanyAdmin(int id, CompanyAdminViewModel model){
            var updateCommand = _mapper.Map<UpdateCompanyAdminCommand>(model);
            updateCommand.Id = id;
            _bus.SendCommand(updateCommand);
        }

        public void RemoveCompanyAdmin(int id){
            var removeCommand = new RemoveCompanyAdminCommand() {Id = id};
            _bus.SendCommand(removeCommand);
        }

        public void Dispose(){
            GC.SuppressFinalize(this);
        }
    }
}