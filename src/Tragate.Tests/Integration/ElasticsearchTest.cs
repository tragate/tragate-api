using Nest;
using Xunit;
using FluentAssertions;
using Elasticsearch.Net;
using Tragate.Common.Library.Dto;
using Microsoft.Extensions.DependencyInjection;

namespace Tragate.Tests.Integration
{
    public class ElasticsearchTest : TestBase
    {
        private readonly ElasticClient _elasticClient;

        public ElasticsearchTest(){
            var provider = BuildServiceProvider();
            _elasticClient = provider.GetService<ElasticClient>();
        }

        [Fact]
        public void Should_Be_Alive(){
            var state = _elasticClient.ClusterState(c => c.Metric(ClusterStateMetric.All));
            Assert.True(state.IsValid);
        }

        [Fact]
        public void Should_Has_Be_Index_Name_As_Alias_Web(){
            var response = _elasticClient.Search<ProductDto>(s => s.Index("tr-web"));

            response.ApiCall.Uri.Should().NotBeNull();
            response.ApiCall.Uri.LocalPath.Should().StartWith("/tr-web/");
        }

        [Fact]
        public void Should_Has_Be_Index_Name_As_Company_Data(){
            var response = _elasticClient.Search<ProductDto>(s => s.Index("companydata"));

            response.ApiCall.Uri.Should().NotBeNull();
            response.ApiCall.Uri.LocalPath.Should().StartWith("/companydata/");
        }

        [Fact]
        public void Should_Be_Root_That_Type_Of_Alias_Web(){
            var resolver = new TypeNameResolver(_elasticClient.ConnectionSettings);
            var index = resolver.Resolve<Root>();
            index.Should().Be("root");
        }

        [Fact]
        public void Should_Be_Product_As_Child_That_Type_Of_Alias_Web(){
            var resolver = new TypeNameResolver(_elasticClient.ConnectionSettings);
            var index = resolver.Resolve<ProductDto>();
            index.Should().Be("product");
        }

        [Fact]
        public void Should_Be_Company_As_Parent_That_Type_Of_Alias_Web(){
            var resolver = new TypeNameResolver(_elasticClient.ConnectionSettings);
            var index = resolver.Resolve<CompanyDto>();
            index.Should().Be("company");
        }
    }
}