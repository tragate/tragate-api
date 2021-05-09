using System.Collections.Generic;
using Nest;

namespace Tragate.Application.ServiceUtility.Search
{
    public interface IQueryBuilder
    {
        MatchQuery CreateMatchQuery(string field, string query);
        SimpleQueryStringQuery CreateSimpleQueryString(string query, string[] fields);
        TermQuery CreateTermQuery(string field, string value);
        HasChildQuery CreateHasChildWithMatchQuery(string type, string field, string value);
        HasChildQuery CreateHasChildQuery(string type, string key, string[] fields);

        HasParentQuery CreateHasParentWithBoolQuery(string parentType, IEnumerable<QueryContainer> mustQueries,
            IEnumerable<QueryContainer> filterQueries);

        BoolQuery CreateBoolQueryWithMustAndFilter(IEnumerable<QueryContainer> mustQueries,
            IEnumerable<QueryContainer> filterQueries);

        BoolQuery CreateBoolQueryWithShouldAndFilter(IEnumerable<QueryContainer> shouldQueries,
            IEnumerable<QueryContainer> filterQueries);

        BoolQuery CreateBoolQueryWithShould(IEnumerable<QueryContainer> shouldQueries);
        SortDescriptor<T> CreateSortQuery<T>(Dictionary<string, SortOrder> sortOrders) where T : class;
    }
}