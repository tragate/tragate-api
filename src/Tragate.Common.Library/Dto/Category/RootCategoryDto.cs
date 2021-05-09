using System.Collections.Generic;
using AutoMapper.Configuration.Conventions;

namespace Tragate.Common.Library.Dto {
    public class RootCategoryDto {
        public string CategoryTitle { get; set; }
        public string ImagePath { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public List<SubCategoryDto> SubCategoryList { get; set; }
    }
}