using System.Collections.Generic;
using Nest;

namespace Tragate.Application.ServiceUtility.Search
{
    public class QueryBuilder : IQueryBuilder
    {
        public MatchQuery CreateMatchQuery(string field, string query){
            return new MatchQuery
            {
                Field = field,
                Query = query
            };
        }

        public ScriptQuery CreateScriptQuery(string function){
            return new ScriptQuery
            {
                Source = function
            };
        }

        public SimpleQueryStringQuery CreateSimpleQueryString(string query, string[] fields){
            return new SimpleQueryStringQuery
            {
                Query = !string.IsNullOrEmpty(query) ? $"*{query}*" : "*",
                Fields = fields,
                AnalyzeWildcard = true
            };
        }

        public TermQuery CreateTermQuery(string field, string value){
            return new TermQuery
            {
                Field = field,
                Value = value
            };
        }

        public HasChildQuery CreateHasChildWithMatchQuery(string type, string field, string value){
            return new HasChildQuery()
            {
                Type = type,
                Query = CreateMatchQuery(field, value)
            };
        }
        
        /// <summary>
        /// It must be score weight is 4. number as this order [title,category,tag,membership,brand] 
        /// </summary>
        /// <param name="parentType"></param>
        /// <param name="mustQueries"></param>
        /// <param name="filterQueries"></param>
        /// <returns></returns>
        public HasParentQuery CreateHasParentWithBoolQuery(string parentType, IEnumerable<QueryContainer> mustQueries,
            IEnumerable<QueryContainer> filterQueries){
            return new HasParentQuery
            {
                ParentType = parentType,
                Score = true,
                Query = new FunctionScoreQuery()
                {
                    Query = CreateBoolQueryWithMustAndFilter(mustQueries, filterQueries),
                    Functions = new[]
                    {
                        new ScriptScoreFunction()
                        {
                            Script = CreateScriptQuery("doc['membershipTypeId'].value^10")
                        }
                    }
                },
                InnerHits = new InnerHits()
            };
        }

        public HasChildQuery CreateHasChildQuery(string type, string key, string[] fields){
            return new HasChildQuery()
            {
                Type = type,
                Query = new SimpleQueryStringQuery()
                {
                    Query = $"*{key}*",
                    Fields = fields,
                    AnalyzeWildcard = true
                }
            };
        }

        public BoolQuery CreateBoolQueryWithMustAndFilter(IEnumerable<QueryContainer> mustQueries,
            IEnumerable<QueryContainer> filterQueries){
            return new BoolQuery
            {
                Must = mustQueries,
                Filter = filterQueries
            };
        }

        public BoolQuery CreateBoolQueryWithShouldAndFilter(IEnumerable<QueryContainer> shouldQueries,
            IEnumerable<QueryContainer> filterQueries){
            return new BoolQuery()
            {
                Should = shouldQueries,
                Filter = filterQueries
            };
        }

        public BoolQuery CreateBoolQueryWithShould(IEnumerable<QueryContainer> shouldQueries){
            return new BoolQuery()
            {
                Should = shouldQueries,
            };
        }

        public SortDescriptor<T> CreateSortQuery<T>(Dictionary<string, SortOrder> sortOrders) where T : class{
            var sortList = new SortDescriptor<T>();
            foreach (var item in sortOrders){
                sortList.Field(item.Key, item.Value);
            }

            return sortList;
        }
    }
}