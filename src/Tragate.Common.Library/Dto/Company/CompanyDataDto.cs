using System;
using Tragate.Common.Library.Enum;

namespace Tragate.Common.Library.Dto
{
    public class CompanyDataDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Tags { get; set; }
        public string AddedDate { get; set; }
        public string ProfileImagePath { get; set; }
        public string Country { get; set; }
        public string CompanyProfileLink { get; set; }
        public string Membership { get; set; }
        public byte StatusId { get; set; }
        public string ProfileDescription { get; set; }
        public string BussinessSegment { get; set; }
        public string TaxNumber { get; set; }
        public string Category { get; set; }
        public string Certificate { get; set; }
        public string EstablishedDateAndNumberOfStaff { get; set; }
        public string ReleatedCompanyAndBrand { get; set; }
        public string ContactPerson { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int? ReferenceUserId { get; set; }
        public string TotalRevenue { get; set; }
        public string Website { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}