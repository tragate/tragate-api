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
    public class CompanyTaskService : ICompanyTaskService
    {
        private readonly ICompanyTaskRepository _companyTaskRepository;
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _bus;

        public CompanyTaskService(ICompanyTaskRepository companyTaskRepository, IMapper mapper, IMediatorHandler bus){
            _companyTaskRepository = companyTaskRepository;
            _mapper = mapper;
            _bus = bus;
        }

        public IEnumerable<CompanyTaskDto> GetCompanyTasks(int page, int pageSize, StatusType status, int? companyId,
            int? responsibleUserId, int? createdUserId){
            return _companyTaskRepository.GetCompanyTasks(page, pageSize, status, companyId, responsibleUserId,
                createdUserId);
        }

        public int CountCompanyTasks(StatusType status, int? companyId, int? responsibleUserId, int? createdUserId){
            return _companyTaskRepository.CountCompanyTasks(status, companyId, responsibleUserId, createdUserId);
        }

        public IEnumerable<CompanyTaskDto>
            GetCompanyTasksByCompanyId(int id, int page, int pageSize, StatusType status){
            return _companyTaskRepository.GetCompanyTasks(page, pageSize, status, id);
        }

        public int CountCompanyTasksByCompanyId(int id, StatusType status){
            return _companyTaskRepository.CountCompanyTasks(status, id);
        }

        public IEnumerable<UserTaskDto> GetCompanyTasksByUserId(int id, int page, int pageSize, StatusType status){
            return _mapper.Map<IEnumerable<UserTaskDto>>(
                _companyTaskRepository.GetCompanyTasks(page, pageSize, status, null, id));
        }

        public int CountCompanyTasksByUserId(int id, StatusType status){
            return _companyTaskRepository.CountCompanyTasks(status, null, id);
        }

        public void AddCompanyTask(CompanyTaskViewModel model){
            var registerCommand = _mapper.Map<AddNewCompanyTaskCommand>(model);
            _bus.SendCommand(registerCommand);
        }

        public void UpdateStatusCompanyTask(int id, CompanyTaskStatusViewModel model){
            var updateStatusCommand = _mapper.Map<UpdateStatusCompanyTaskCommand>(model);
            updateStatusCommand.Id = id;
            _bus.SendCommand(updateStatusCommand);
        }

        public void DeleteCompanyTask(int id){
            var deleteCommand = new DeleteCompanyTaskCommand() {Id = id};
            _bus.SendCommand(deleteCommand);
        }

        public void Dispose(){
            GC.SuppressFinalize(this);
        }
    }
}