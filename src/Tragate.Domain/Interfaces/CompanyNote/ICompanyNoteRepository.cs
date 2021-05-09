using System.Collections.Generic;
using Tragate.Common.Library.Dto;
using Tragate.Domain.Models;

namespace Tragate.Domain.Interfaces
{
    public interface ICompanyNoteRepository : IRepository<CompanyNote>
    {
        IEnumerable<CompanyNoteDto> GetCompanyNotes(int page, int pageSize, int? status, int? companyId = null,
            int? userId = null);

        int CountCompanyNotes(int? status, int? companyId = null, int? userId = null);
    }
}