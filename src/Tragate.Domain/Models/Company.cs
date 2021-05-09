using System;
using System.ComponentModel.DataAnnotations.Schema;
using Tragate.Domain.Core.Models;

namespace Tragate.Domain.Models
{
    public class Company : Entity
    {
        public string Description { get; set; }
        public string BusinessType { get; set; }
        public string EstablishmentYear { get; set; }
        public string TaxOffice { get; set; }
        public string TaxNumber { get; set; }
        public string Address { get; set; }
        public string Website { get; set; }
        public string Skype { get; set; }
        public string ResponseRate { get; set; }
        public string ResponseTime { get; set; }
        public int? TransactionAmount { get; set; }
        public int? TransactionCount { get; set; }
        public int? EmployeeCountId { get; set; }
        public int? AnnualRevenueId { get; set; }
        public int? MembershipTypeId { get; set; }
        public int? MembershipPackageId { get; set; }
        public int? VerificationTypeId { get; set; }
        public byte StatusId { get; set; }
        public int UserId { get; set; }
        public int OwnerUserId { get; set; }
        public int ContactUserId { get; set; }
        public string Slug { get; set; }
        public string Phone { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int? WebsitePoint { get; set; }
        public int? WebsiteProductCount { get; set; }
    }
}