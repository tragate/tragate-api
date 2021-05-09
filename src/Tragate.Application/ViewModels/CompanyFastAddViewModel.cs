using System;
using System.Collections.Generic;
using System.Text;

namespace Tragate.Application.ViewModels
{
    public class CompanyFastAddViewModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CompanyName { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyPhone { get; set; }
        public string BusinessType { get; set; }
        public int CategoryId { get; set; }
        public int CountryId { get; } = 1;
        public int CityId { get; set; }
    }
}
