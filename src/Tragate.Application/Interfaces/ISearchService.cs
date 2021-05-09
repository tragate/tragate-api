using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Dto.Search;

namespace Tragate.Application
{
    public interface ISearchService
    {
        SearchResponseDto<SearchAllDto> SearchAll(int page, int pageSize, string key);

        SearchResponseDto<ProductDto> SearchProduct(int page, int pageSize, string key, string categorySlug,
            string companySlug, int? categoryGroupId);

        SearchResponseDto<CompanySearchDto> SearchCompany(int page, int pageSize, string key,
            string categoryTag);

        SearchResponseDto<CompanyDataSearchDto> SearchCompanyData(int page, int pageSize, string key,
            int? status,
            int? companyId = null);
    }
}