using System;
using Tragate.Common.Library.Enum;

namespace Tragate.Domain.Commands
{
    public abstract class UserCompanyCommand : Command
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ProfileImagePath { get; set; }
        public bool EmailVerified { get; set; }
        public string Token { get; set; }
        public string ExternalUserId { get; set; }
        public string Password { get; set; }
        public string PasswordMatch { get; set; }
        public int PersonId { get; set; }
        public StatusType StatusType { get; set; }
        public string Description { get; set; }
        public RegisterType RegisterTypeId { get; set; }
        public string BusinessType { get; set; }
        public string EstablishmentYear { get; set; }
        public string TaxOffice { get; set; }
        public string TaxNumber { get; set; }
        public string Address { get; set; }
        public string Website { get; set; }
        public int? EmployeeCountId { get; set; }
        public int? AnnualRevenueId { get; set; }
        public int? VerificationTypeId { get; set; }
        public int? LanguageId { get; set; }
        public int? TimezoneId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int? CompanyDataId { get; set; }
        public int LocationId { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int? CityId { get; set; }
        public int? DistrictId { get; set; }
        public int[] CompanyCategoryIds { get; set; }
        public string Phone { get; set; }
        public int? WebsitePoint { get; set; }
        public int? WebsiteProductCount { get; set; }
        public int MembershipTypeId { get; set; }
        public int MembershipPackageId { get; set; }
    }
}