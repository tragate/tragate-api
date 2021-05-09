using System;
using System.Collections.Generic;
using AutoMapper;
using Tragate.Application.ViewModels;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Commands;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Interfaces;

namespace Tragate.Application
{
    public class CompanyDataService : ICompanyDataService
    {
        private readonly IMapper _mapper;
        private readonly ICompanyDataRepository _companyDataRepository;
        private readonly IMediatorHandler _bus;

        public CompanyDataService(
            IMapper mapper,
            IMediatorHandler bus,
            ICompanyDataRepository companyDataRepository){
            _companyDataRepository = companyDataRepository;
            _mapper = mapper;
            _bus = bus;
        }

        public int CountByCompaniesDataByStatus(StatusType status, string name, int? companyId){
            return _companyDataRepository.CountByCompaniesDataByStatus(name, status, companyId);
        }

        public IEnumerable<CompanyDataDto> GetCompaniesDataByStatus(int page, int pageSize, string name,
            StatusType status, int? companyId){
            var result = _companyDataRepository.GetCompaniesDataByStatus(page, pageSize, name, status, companyId);
            return _mapper.Map<IEnumerable<CompanyDataDto>>(result);
        }

        public CompanyDataDto GetCompanyDataById(int id){
            var result = _companyDataRepository.GetById(id);
            return _mapper.Map<CompanyDataDto>(result);
        }

        public void UpdateStatusOfCompanyData(CompanyDataStatusViewModel model){
            var updateCommand = new UpdateReferenceCompanyDataCommand()
            {
                Id = model.Id,
                StatusType = (StatusType) model.Status,
                CompanyId = model.CompanyId,
                UpdatedUserId = model.UpdatedUserId,
                UpdatedDate = DateTime.Now
            };
            _bus.SendCommand(updateCommand);
        }

        public void UpdateCompanyData(CompanyDataViewModel model){
            var updateCommand = _mapper.Map<UpdateCompanyDataCommand>(model);
            _bus.SendCommand(updateCommand);
        }

        public void Dispose(){
            GC.SuppressFinalize(this);
        }
    }
}