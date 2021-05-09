using System.Net.Http;
using Xunit;

namespace Tragate.Tests.Integration
{
    public class ImageResizerApiTest : TestBase
    {
        [Fact]
        public void Should_Be_Alive(){
            var httpClient = new HttpClient();
            var result = httpClient.GetAsync("http://apiresizertest.tragate.com/");
            Assert.Equal("OK", result.Result.Content.ReadAsStringAsync().Result);
        }
    }
}