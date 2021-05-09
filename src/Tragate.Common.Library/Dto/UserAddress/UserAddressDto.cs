using System;
using Tragate.Common.Library.Enum;

namespace Tragate.Common.Library.Dto
{
    public class UserAddressDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string ContactPerson { get; set; }
        public string CompanyTitle { get; set; }
        public string TaxNumber { get; set; }
        public string TaxOffice { get; set; }
        public string Address { get; set; }
        public int? CountryId { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string ZipCode { get; set; }
        public StatusType StatusId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}