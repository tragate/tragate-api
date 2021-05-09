using System;
using System.ComponentModel.DataAnnotations.Schema;
using Tragate.Domain.Core.Models;

namespace Tragate.Domain.Models
{
    public class User : Entity
    {
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Email { get; set; }
        public bool EmailVerified { get; set; }
        public string FullName { get; set; }
        public string ProfileImagePath { get; set; }
        public string ExternalUserId { get; set; }
        public string ExternalUserToken { get; set; }
        public byte RegisterTypeId { get; set; }
        public byte UserTypeId { get; set; }
        public byte StatusId { get; set; }
        public int? LanguageId { get; set; }
        public int? TimezoneId { get; set; }
        public int? LocationId { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public int? DistrictId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Phone { get; set; }
    }
}