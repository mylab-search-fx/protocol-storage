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
using MyLab.Search.EsAdapter.Inter;
using MyLab.Search.EsTest;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests
{
    public partial class ProtocolStorageBehavior :
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

        private void ConfigureTestApi(IServiceCollection services)
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

        private class TestProtocolEvent : ProtocolEvent
        {
            public string Message { get; set; }
        }

        public async Task InitializeAsync()
        {
            try
            {
                await _esFxt.IndexTools.DeleteIndexAsync("test-test");
            }
            catch (EsException e) when (e.Response.ServerError.Status == 404)
            {
            }

            _queue.Purge();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}