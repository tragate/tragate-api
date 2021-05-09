using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Tragate.Application.ViewModels
{
    public class CompanyViewModel
    {
        public int PersonId { get; set; }
        public string ProfileImagePath { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string BusinessType { get; set; }
        public string EstablishmentYear { get; set; }
        public string TaxOffice { get; set; }
        public string TaxNumber { get; set; }
        public int? AnnualRevenueId { get; set; }
        public int? EmployeeCountId { get; set; }
        public int? VerificationTypeId { get; set; }
        public string Address { get; set; }
        public string Website { get; set; }
        public int StatusId { get; set; }
        public int? CompanyDataId { get; set; }
        public int LocationId { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int? CityId { get; set; }
        public int? DistrictId { get; set; }
        public string Phone { get; set; }
        public int? WebsitePoint { get; set; }
        public int? WebsiteProductCount { get; set; }
        public int[] CategoryIds { get; set; }
    }
}