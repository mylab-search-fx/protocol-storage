using System;
using Elasticsearch.Net;
using MyLab.Search.EsTest;

namespace IntegrationTests
{

    public class EsTestConnProvider : IConnectionProvider
    {
        public IConnectionPool Provide()
        {
            return new SingleNodeConnectionPool(new Uri("http://localhost:9200"));
        }
    }
}