using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tragate.Infra.Data.Context;
using Xunit;

namespace Tragate.Tests.Integration
{
    public class SqlserverTest : TestBase
    {
        protected readonly TragateContext Db;

        public SqlserverTest(){
            Db = BuildServiceProvider().GetService<TragateContext>();
        }

        [Fact]
        public void Should_Be_Alive(){
            Db.Database.OpenConnection();
            var conn = Db.Database.GetDbConnection();
            Assert.Equal(ConnectionState.Open, conn.State);
            Db.Database.CloseConnection();
        }
    }
}