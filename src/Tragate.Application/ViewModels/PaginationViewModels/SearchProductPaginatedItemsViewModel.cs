using System.Collections.Generic;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Dto.Search;

namespace Tragate.Application.ViewModels
{
    public class SearchProductPaginatedItemsViewModel : PaginatedItemsViewModel<ProductDto>
    {
        public IEnumerable<CategoryAggsDto> CategoryAggs { get; }

        public SearchProductPaginatedItemsViewModel(int pageIndex, int pageSize, long count,
            IEnumerable<ProductDto> dataList, IEnumerable<CategoryAggsDto> categoryAggs)
            : base(pageIndex, pageSize, count, dataList){
            CategoryAggs = categoryAggs;
        }
    }
}