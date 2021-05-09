using Nest;
using Newtonsoft.Json;

namespace Tragate.Common.Library.Dto
{
    public class Root
    {
        [JsonIgnore]
        public JoinField JoinField { get; set; }

        public string Slug { get; set; }

        /// <summary>
        ///This field using for hierarchical search
        /// </summary>
        [JsonIgnore]
        public string CategoryPath { get; set; }

        [JsonIgnore]
        public string[] CategoryTags { get; set; }

        /// <summary>
        /// This field using for full text search
        /// </summary>
        [JsonIgnore]
        public string CategoryText { get; set; }

        public string Title { get; set; }
    }
}