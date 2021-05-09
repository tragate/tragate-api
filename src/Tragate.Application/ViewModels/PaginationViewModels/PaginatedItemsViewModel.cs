using System.Collections.Generic;

namespace Tragate.Application.ViewModels
{
    public class PaginatedItemsViewModel<T> where T : class
    {
        public int PageIndex { get; }
        public int PageSize { get; }
        public long Count { get; }
        public IEnumerable<T> DataList { get; }

        public PaginatedItemsViewModel(int pageIndex, int pageSize, long count, IEnumerable<T> dataList){
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
            DataList = dataList;
        }
    }
}