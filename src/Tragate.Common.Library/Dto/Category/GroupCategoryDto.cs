using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tragate.Common.Library.Dto
{
    public class GroupCategoryDto
    {
        [JsonIgnore]
        public string CategoryGroup { get; set; }

        public string CategoryTitle { get; set; }

        [JsonIgnore]
        public string SubCategoryTitle { get; set; }

        public string CategoryGroupImagePath { get; set; }
        
        [JsonIgnore]
        public string CategoryImagePath { get; set; }

        [JsonIgnore]
        public string SubCategoryImagePath { get; set; }

        [JsonIgnore]
        public string CategorySlug { get; set; }

        [JsonIgnore]
        public string SubCategorySlug { get; set; }
        
        [JsonIgnore]
        public string CategoryDescription { get; set; }

        public List<RootCategoryDto> CategoryList { get; set; }
    }
}