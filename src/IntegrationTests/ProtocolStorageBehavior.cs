using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyLab.ApiClient;
using MyLab.ApiClient.Test;
using MyLab.ProtocolStorage;
using MyLab.ProtocolStorage.Client;
using MyLab.ProtocolStorage.Client.Models;
using MyLab.RabbitClient;
using MyLab.RabbitClient.Model;
using MyLab.Search.EsTest;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests
{
    using ClientSearchRequest  = MyLab.ProtocolStorage.Client.Models.ClientSearchRequest;
    using FilterRef = MyLab.ProtocolStorage.Client.Models.FilterRef;

    public class ProtocolStorageBehavior : 
        IClassFixture<EsFixture<TestEsFxtStrategy>>,
        IClassFixture<TestApi<Startup, ITokenApiV1>>,
        IClassFixture<TestApi<Startup, IProtocolApiV1>>,
        IAsyncLifetime
    {
        private readonly ITestOutputHelper _output;
        private readonly RabbitQueue _queue;
        private readonly ITokenApiV1 _tokenApi;
        private readonly IProtocolApiV1 _protocolApi;
        private readonly EsFixture<TestEsFxtStrategy> _esFxt;

        public ProtocolStorageBehavior(
            EsFixture<TestEsFxtStrategy> esFxt,
            TestApi<Startup, ITokenApiV1> tokenApi,
            TestApi<Startup, IProtocolApiV1> protocolApi,
            ITestOutputHelper output)
        {
            _output = output;
            
            _queue = new RabbitQueue("protocol", TestTools.ChannelProvider);

            _esFxt = esFxt;

            tokenApi.Output = output; 
            tokenApi.ServiceOverrider = ConfigureTestApi;
            _tokenApi = tokenApi.StartWithProxy();
            
            protocolApi.Output = output;
            protocolApi.ServiceOverrider = ConfigureTestApi;
            _protocolApi = protocolApi.StartWithProxy();
        }

        [Fact]
        public async Task ShouldIndexEvent()
        {
            //Arrange
            var eventObj = new TestProtocolEvent
            {
                Id = Guid.NewGuid().ToString("N"),
                Message = "foo"
            };

            var eventObj2 = new TestProtocolEvent
            {
                Id = Guid.NewGuid().ToString("N"),
                Message = "foo2"
            };

            var searchReq = new ClientSearchRequest
            {
                Filters = new []
                {
                    new FilterRef
                    {
                        Id = "by-id",
                        Args = new Dictionary<string, string>
                        {
                            { "id", eventObj.Id }
                        }
                    }
                }
            };

            var searchToken = await _tokenApi.CreateTotalTokenAsync();

            //Act
            await _protocolApi.PostEventAsync("test", eventObj);
            await _protocolApi.PostEventAsync("test", eventObj2);
            await Task.Delay(1000);

            var searchRes=  await _protocolApi.SearchAsync("test", searchReq, searchToken);

            var foundEvent = searchRes.Events.Select(e => e.Content.ToObject<TestProtocolEvent>()).FirstOrDefault();

            //Assert
            Assert.Single(searchRes.Events);
            Assert.NotNull(foundEvent);
            Assert.Equal(eventObj.Id, foundEvent.Id);
            Assert.True(DateTime.Now.AddMinutes(-1) < foundEvent.DateTime && foundEvent.DateTime < DateTime.Now);
            Assert.Equal(eventObj.Subject, foundEvent.Subject);
        }

        void ConfigureTestApi(IServiceCollection services)
        {
            services
                .Configure<RabbitOptions>(opt =>
                {
                    opt.Host = "localhost";
                    opt.Port = 5672;
                    opt.User = "test";
                    opt.Password = "AICQ0kiYgjUcQNL1AfRA";
                    opt.DefaultPub = new PublishOptions
                    {
                        RoutingKey = "protocol"
                    };
                })
                .Configure<ApiClientsOptions>(opt =>
                {
                    opt.List.Add("indexer", new ApiConnectionOptions
                    {
                        Url = "http://localhost:8085"
                    });
                    opt.List.Add("searcher", new ApiConnectionOptions
                    {
                        Url = "http://localhost:8086"
                    });
                })
                .AddLogging(l =>
                {
                    l.ClearProviders()
                        .AddXUnit(_output)
                        .AddFilter(lvl => lvl >= LogLevel.Debug);
                });
        }

        class TestProtocolEvent : ProtocolEvent
        {
            public string Message { get; set; }
        }

        public Task InitializeAsync()
        {
            return _esFxt.IndexTools.PruneIndexAsync("test-index");
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}