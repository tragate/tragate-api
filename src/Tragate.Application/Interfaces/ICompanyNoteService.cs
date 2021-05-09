using System;
using System.Collections.Generic;
using Tragate.Application.ViewModels;
using Tragate.Common.Library.Dto;

namespace Tragate.Application
{
    public interface ICompanyNoteService:IDisposable
    {
        IEnumerable<CompanyNoteDto> GetCompanyNotes(int page, int pageSize, int? status);
        int CountCompanyNotes(int? status);

        IEnumerable<CompanyNoteDto> GetCompanyNotesByCompanyId(int id, int page, int pageSize, int? status);
        int CountCompanyNotesByCompanyId(int id, int? status);

        IEnumerable<CompanyNoteDto> GetCompanyNotesByUserId(int id, int page, int pageSize, int? status);
        int CountCompanyNotesByUserId(int id, int? status);

        void AddCompanyNote(CompanyNoteViewModel model);
        void DeleteCompanyNote(int id);
    }
}