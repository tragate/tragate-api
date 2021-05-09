using System.Collections.Generic;

namespace Tragate.Common.Library.Dto.Search
{
    public class SearchResponseDto<T>
    {
        public List<T> Documents { get; set; }
        public long Total { get; set; }
        public IEnumerable<CategoryAggsDto> CategoryAggs { get; set; }
    }
}