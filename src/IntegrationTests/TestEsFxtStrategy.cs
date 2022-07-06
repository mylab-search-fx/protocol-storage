using System;
using Elasticsearch.Net;
using MyLab.Search.EsTest;

namespace IntegrationTests
{
    public class TestEsFxtStrategy : EsFixtureStrategy
    {
        public override IConnectionPool ProvideConnection()
        {
            return new SingleNodeConnectionPool(new Uri("http://localhost:9200"));
        }
    }
}