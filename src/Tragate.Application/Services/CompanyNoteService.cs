using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Tragate.Application.ViewModels;
using Tragate.Common.Library.Dto;
using Tragate.Domain.Commands;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Interfaces;

namespace Tragate.Application
{
    public class CompanyNoteService : ICompanyNoteService
    {
        private readonly ICompanyNoteRepository _companyNoteRepository;
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _bus;

        public CompanyNoteService(ICompanyNoteRepository companyNoteRepository, IMapper mapper, IMediatorHandler bus){
            _companyNoteRepository = companyNoteRepository;
            _mapper = mapper;
            _bus = bus;
        }

        public IEnumerable<CompanyNoteDto> GetCompanyNotes(int page, int pageSize, int? status){
            return _companyNoteRepository.GetCompanyNotes(page, pageSize, status).ToList();
        }

        public int CountCompanyNotes(int? status){
            return _companyNoteRepository.CountCompanyNotes(status);
        }

        public IEnumerable<CompanyNoteDto> GetCompanyNotesByCompanyId(int id, int page, int pageSize, int? status){
            return _companyNoteRepository.GetCompanyNotes(page, pageSize, status, id);
        }

        public int CountCompanyNotesByCompanyId(int id, int? status){
            return _companyNoteRepository.CountCompanyNotes(status, id);
        }

        public IEnumerable<CompanyNoteDto> GetCompanyNotesByUserId(int id, int page, int pageSize, int? status){
            return _companyNoteRepository.GetCompanyNotes(page, pageSize, status, null, id);
        }

        public int CountCompanyNotesByUserId(int id, int? status){
            return _companyNoteRepository.CountCompanyNotes(status, null, id);
        }

        public void AddCompanyNote(CompanyNoteViewModel model){
            var registerCommand = _mapper.Map<AddNewCompanyNoteCommand>(model);
            _bus.SendCommand(registerCommand);
        }

        public void DeleteCompanyNote(int id){
            _bus.SendCommand(new DeleteCompanyNoteCommand() {Id = id});
        }

        public void Dispose(){
            GC.SuppressFinalize(this);
        }
    }
}