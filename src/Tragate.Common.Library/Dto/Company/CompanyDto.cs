using System;
using System.Collections.Generic;
using Nest;
using Tragate.Common.Library.Enum;

namespace Tragate.Common.Library.Dto
{
    [ElasticsearchType(Name = "company")]
    public class CompanyDto : Root
    {
        public int Id { get; set; }

        [Text(Ignore = true)]
        public string Description { get; set; }

        [Text(Ignore = true)]
        public string BusinessType { get; set; }

        public string EstablishmentYear { get; set; }

        [Text(Ignore = true)]
        public string TaxOffice { get; set; }

        [Text(Ignore = true)]
        public string TaxNumber { get; set; }

        [Text(Ignore = true)]
        public string Address { get; set; }

        [Text(Ignore = true)]
        public string Website { get; set; }

        [Text(Ignore = true)]
        public string Skype { get; set; }

        [Text(Ignore = true)]
        public string Phone { get; set; }

        public string ResponseRate { get; set; }
        public string ResponseTime { get; set; }
        public int? TransactionAmount { get; set; }
        public int? TransactionCount { get; set; }

        [Text(Ignore = true)]
        public int EmployeeCountId { get; set; }

        [Text(Ignore = true)]
        public int AnnualRevenueId { get; set; }

        public int? MembershipTypeId { get; set; }

        [Text(Ignore = true)]
        public int? MembershipPackageId { get; set; }

        public int? VerificationTypeId { get; set; }
        public byte StatusId { get; set; }

        [Text(Ignore = true)]
        public string Status { get; set; }

        [Text(Ignore = true)]
        public int UserId { get; set; }

        [Text(Ignore = true)]
        public int OwnerUserId { get; set; }

        [Text(Ignore = true)]
        public int ContactUserId { get; set; }

        [Text(Ignore = true)]
        public string OwnerUser { get; set; }

        [Text(Ignore = true)]
        public DateTime UpdatedDate { get; set; }

        public UserDto User { get; set; }

        [Text(Ignore = true)]
        public CompanyDataDto CompanyData { get; set; }

        [Text(Ignore = true)]
        public List<CategoryDto> CategoryList { get; set; }

        [Text(Ignore = true)]
        public string BusinessTypes { get; set; }

        [Text(Ignore = true)]
        public string EmployeeCount { get; set; }

        [Text(Ignore = true)]
        public string AnnualRevenue { get; set; }

        [Text(Ignore = true)]
        public string MembershipPackage { get; set; }

        public string MembershipType { get; set; }
        public string VerificationType { get; set; }

        [Text(Ignore = true)]
        public int ProductCount { get; set; }

        [Text(Ignore = true)]
        public int? WebsitePoint { get; set; }

        [Text(Ignore = true)]
        public int? WebsiteProductCount { get; set; }
    }
}