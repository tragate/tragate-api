using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nest;
using Tragate.Application.ServiceUtility.Search;
using Tragate.Common.Library.Constants;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Dto.Search;
using Tragate.Common.Library.Enum;

namespace Tragate.Application
{
    public class SearchService : ISearchService
    {
        private readonly ElasticClient _elasticClient;
        private readonly IQueryBuilder _queryBuilder;

        public SearchService(ElasticClient elasticClient, IQueryBuilder queryBuilder){
            _elasticClient = elasticClient;
            _queryBuilder = queryBuilder;
        }


        public SearchResponseDto<SearchAllDto> SearchAll(int page, int pageSize, string key){
            var sortDescriptor = _queryBuilder.CreateSortQuery<CompanySearchDto>(new Dictionary<string, SortOrder>()
            {
                {"membershipTypeId", SortOrder.Descending},
                {"_score", SortOrder.Descending}
            });

            var query = _elasticClient.Search<SearchAllDto>(x => x
                .Index(TragateConstants.ALIAS)
                .Query(q => q.FunctionScore(fs => fs.Query(fq =>
                        fq.Bool(bo =>
                            bo.Must(m => m.SimpleQueryString(sq =>
                                    sq.Query(!string.IsNullOrEmpty(key) ? $"*{key}*" : "*")
                                        .AnalyzeWildcard()))
                                .Filter(f => f.Term(t =>
                                    t.Field("statusId").Value(((byte) StatusType.Active).ToString())))))
                    .Functions(f =>
                        f.RandomScore(rs => rs.Field("_seq_no").Seed((long) DateTime.Today.DayOfWeek)))))
                .Sort(s => sortDescriptor)
                .From((page - 1) * pageSize)
                .Size(pageSize));

            var response = new SearchResponseDto<SearchAllDto>()
            {
                Documents = query.Documents.ToList(),
                Total = query.Total
            };

            return response;
        }

        /// <summary>
        /// random scoring ->
        /// https://www.elastic.co/guide/en/elasticsearch/reference/6.x/query-dsl-function-score-query.html#function-weight
        /// haftanın günlerinin sayısı arasında (1-7) için (0-0)'a oranlanmasının sonucunca (0-1) arasında fuzzy logic
        /// olarak score belirlenecek böylece hergün reindexden sonra random sıralı bir şekilde sorgulanabilir.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="key"></param>
        /// <param name="categorySlug"></param>
        /// <param name="companySlug"></param>
        /// <param name="categoryGroupId"></param>
        /// <returns></returns>
        public SearchResponseDto<ProductDto> SearchProduct(int page, int pageSize, string key, string categorySlug,
            string companySlug, int? categoryGroupId){
            var query = _elasticClient.Search<ProductDto>(x => x
                .Aggregations(a => a
                    .Terms("category_agg", st => st
                        .Field(BuildCategoryAggregationField(categoryGroupId))
                        .Size(25)
                    )
                )
                .Index(TragateConstants.ALIAS)
                .Query(q => q.FunctionScore(fs => fs.Query(fq =>
                        fq.Bool(bo => bo.Must(
                            BuildSearchProductQuery(key, categorySlug, companySlug)
                        )))
                    .Functions(f =>
                        f.RandomScore(rs => rs.Field("_seq_no").Seed((long) DateTime.Today.DayOfWeek)))))
                .From((page - 1) * pageSize)
                .Size(pageSize));

            foreach (var product in query.Hits){
                product.Source.ProductCompany = product.InnerHits["company"].Documents<CompanySearchDto>().First();
            }

            var response = new SearchResponseDto<ProductDto>()
            {
                Documents = query.Documents.ToList(),
                Total = query.Total,
                CategoryAggs = query.Aggregations.Terms("category_agg").Buckets.Select(x => new CategoryAggsDto()
                {
                    Key = x.Key,
                    Count = x.DocCount.Value
                }).ToList()
            };

            return response;
        }

        /// <summary>
        /// random scoring ->
        /// https://www.elastic.co/guide/en/elasticsearch/reference/6.x/query-dsl-function-score-query.html#function-weight
        /// haftanın günlerinin sayısı arasında (1-7) için (0-0)'a oranlanmasının sonucunca (0-1) arasında fuzzy logic
        /// olarak score belirlenecek böylece hergün reindexden sonra random sıralı bir şekilde sorgulanabilir.
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="key"></param>
        /// <param name="categoryTag"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public SearchResponseDto<CompanySearchDto>
            SearchCompany(int page, int pageSize, string key, string categoryTag){
            var companyFilters = new List<QueryContainer>
            {
                _queryBuilder.CreateTermQuery("statusId", ((byte) StatusType.Active).ToString()),
                _queryBuilder.CreateTermQuery("categoryTags", categoryTag),
                _queryBuilder.CreateTermQuery("joinField", "company")
            };

            var sortDescriptor = _queryBuilder.CreateSortQuery<CompanySearchDto>(new Dictionary<string, SortOrder>()
            {
                {"membershipTypeId", SortOrder.Descending},
                {"_score", SortOrder.Descending}
            });


            var query = _elasticClient.Search<CompanySearchDto>(x => x
                .Index(TragateConstants.ALIAS)
                .Query(q => q.FunctionScore(fs => fs.Query(fq =>
                        fq.Bool(bo =>
                            bo.Must(mu =>
                                    mu.DisMax(dm => dm.Queries(BuildSearchCompanyQuery(key))))
                                .Filter(companyFilters.ToArray())))
                    .Functions(f =>
                        f.RandomScore(rs => rs.Field("_seq_no").Seed((long) DateTime.Today.DayOfWeek)))))
                .Sort(s => sortDescriptor)
                .From((page - 1) * pageSize)
                .Size(pageSize));

            var response = new SearchResponseDto<CompanySearchDto>()
            {
                Documents = query.Documents.ToList(),
                Total = query.Total
            };

            return response;
        }


        public SearchResponseDto<CompanyDataSearchDto> SearchCompanyData(int page, int pageSize, string key,
            int? status, int? companyId = null){
            var companyDataFilters = new List<QueryContainer>
            {
                _queryBuilder.CreateTermQuery("statusId", status?.ToString()),
                _queryBuilder.CreateTermQuery("companyId", companyId?.ToString())
            };

            var query = _elasticClient.Search<CompanyDataSearchDto>(x => x
                .Index(TragateConstants.COMPANY_DATA)
                .Query(q => q.Bool(bo => bo.Must(
                    m => m.QueryString(qs => qs.Query($"*{key ?? string.Empty}*")
                        .Fields(new[] {"title", "membership"}))
                ).Filter(companyDataFilters.ToArray())))
                .From((page - 1) * pageSize)
                .Size(pageSize));

            var response = new SearchResponseDto<CompanyDataSearchDto>()
            {
                Documents = query.Documents.ToList(),
                Total = query.Total
            };

            return response;
        }

        /// <summary>
        /// dynamic builder query pattern
        /// It must be score weight as this order [title,category,tag,membership,brand]
        /// </summary>
        /// <param name="key"></param>
        /// <param name="categorySlug"></param>
        /// <param name="companySlug"></param>
        /// <returns></returns>
        private QueryContainer[] BuildSearchProductQuery(string key, string categorySlug, string companySlug){
            var qc = new List<QueryContainer>();
            var companyMustQueries = new List<QueryContainer>
            {
                _queryBuilder.CreateMatchQuery("statusId", ((byte) StatusType.Active).ToString())
            };

            var companyFilters = new List<QueryContainer>()
            {
                _queryBuilder.CreateTermQuery("slug", companySlug)
            };

            var productmustQueries = new List<QueryContainer>
            {
                _queryBuilder.CreateSimpleQueryString(key, new[] {"title^9", "brand^7", "tags^1", "categoryText^1"})
            };

            var productFilters = new List<QueryContainer>
            {
                _queryBuilder.CreateTermQuery("statusId", ((byte) StatusType.Active).ToString()),
                _queryBuilder.CreateTermQuery("categoryPath", categorySlug)
            };

            qc.Add(_queryBuilder.CreateHasParentWithBoolQuery("company", companyMustQueries, companyFilters));
            qc.Add(_queryBuilder.CreateBoolQueryWithMustAndFilter(productmustQueries, productFilters));


            return qc.ToArray();
        }

        /// <summary>
        /// This method creates nested category tree for category aggregation like;
        /// by categoryGroupId that sent from client by user selected category tree dropdownlists
        ///      "aggs": {
        ///         "category": {
        ///             "terms": {
        ///                 "field": "categoryTree.right.right.....value.keyword",
        ///                 "size": 25
        ///             }
        ///        }
        ///    }
        /// </summary>
        /// <param name="categoryGroupId"></param>
        /// <returns></returns>
        private string BuildCategoryAggregationField(int? categoryGroupId){
            var sb = new StringBuilder();
            sb.Append("categoryTree");
            if (categoryGroupId.HasValue){
                categoryGroupId++;
                for (int i = 0; i < categoryGroupId; i++){
                    sb.Append(".right");
                }
            }

            sb.Append(".value.keyword");

            return sb.ToString();
        }

        /// <summary>
        /// It must be score weight as this order [title,membership,category,location]
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private QueryContainer[] BuildSearchCompanyQuery(string key){
            var qc = new List<QueryContainer>
            {
                _queryBuilder.CreateHasChildQuery("product", key, new[] {"title", "brand", "tags", "categoryText"}),
                _queryBuilder.CreateSimpleQueryString(key,
                    new[] {"title^4", "user.location.name^1", "categoryText^2"})
            };


            return qc.ToArray();
        }
    }
}