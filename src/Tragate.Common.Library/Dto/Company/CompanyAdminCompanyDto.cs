using System;
using Tragate.Common.Library.Enum;

namespace Tragate.Common.Library.Dto
{
    public class CompanyAdminCompanyDto
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string ProfileImagePath { get; set; }
        public string Slug { get; set; }
    }
}