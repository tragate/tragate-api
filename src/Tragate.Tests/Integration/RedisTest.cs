using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Tragate.Tests.Integration
{
    public class RedisTest : TestBase
    {
        private readonly IDistributedCache _distributedCache;

        public RedisTest(){
            _distributedCache = BuildServiceProvider().GetService<IDistributedCache>();
        }

        [Fact]
        public void Should_Be_Alive(){
            _distributedCache.SetString("ping", "pong");
            _distributedCache.GetString("ping").Should().Be("pong");
            _distributedCache.Remove("ping");
        }
    }
}