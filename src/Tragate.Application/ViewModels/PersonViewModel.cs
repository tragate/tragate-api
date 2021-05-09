using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Tragate.Application.ViewModels
{
    public class PersonViewModel : AnonymUserViewModel
    {
        public string FullName { get; set; }
        public string Password { get; set; }
        public string PasswordMatch { get; set; }
        public string ProfileImagePath { get; set; }
        public int LocationId { get; set; }
        public int? TimezoneId { get; set; }
        public int? LanguageId { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int? CityId { get; set; }
        public int? DistrictId { get; set; }
        public string Phone { get; set; }
    }
}