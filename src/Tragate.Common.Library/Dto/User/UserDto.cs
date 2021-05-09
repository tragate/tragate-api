using System;
using Nest;
using Newtonsoft.Json;
using Tragate.Common.Library.Enum;

namespace Tragate.Common.Library.Dto
{
    public class UserDto
    {
        [Text(Ignore = true)]
        public int Id { get; set; }

        [Text(Ignore = true)]
        public string Email { get; set; }

        [Text(Ignore = true)]
        public string Phone { get; set; }

        [Text(Ignore = true)]
        [JsonIgnore]
        public string Password { get; set; }

        [Text(Ignore = true)]
        [JsonIgnore]
        public string Salt { get; set; }

        public string FullName { get; set; }

        [Text(Ignore = true)]
        public UserType UserType { get; set; }

        [Text(Ignore = true)]
        public string UserTypeName { get; set; }

        [Text(Ignore = true)]
        public RegisterType RegisterType { get; set; }

        [Text(Ignore = true)]
        public string RegisterTypeName { get; set; }

        [Text(Ignore = true)]
        public StatusType StatusType { get; set; }

        [Text(Ignore = true)]
        public string Status { get; set; }

        [Text(Ignore = true)]
        [JsonIgnore]
        public bool UserStatus { get; set; }

        [Text(Ignore = true)]
        public bool EmailVerified { get; set; }

        public string ProfileImagePath { get; set; }

        [Text(Ignore = true)]
        public DateTime CreatedDate { get; set; }

        [Text(Ignore = true)]
        public int? LocationId { get; set; }

        [Text(Ignore = true)]
        public int? TimezoneId { get; set; }

        [Text(Ignore = true)]
        public int? LanguageId { get; set; }

        [Text(Ignore = true)]
        public int? CountryId { get; set; }

        [Text(Ignore = true)]
        public int? StateId { get; set; }

        public LocationDto Location { get; set; }

        [Text(Ignore = true)]
        public LocationDto Country { get; set; }
    }
}