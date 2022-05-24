using System;
using Microsoft.Extensions.DependencyInjection;
using MyLab.ApiClient;
using MyLab.ApiClient.Test;
using MyLab.ProtocolStorage;
using MyLab.ProtocolStorage.Client;
using MyLab.RabbitClient;
using MyLab.RabbitClient.Model;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests
{
    public class ProtocolStorageBehavior : IDisposable
    {
        private readonly ITestOutputHelper _output;
        private readonly RabbitQueue _queue;
        private readonly TestApi<Startup, ITokenApiV1> _tokenApi;
        private readonly TestApi<Startup, IProtocolApiV1> _protocolApi;

        public ProtocolStorageBehavior(ITestOutputHelper output)
        {
            _output = output;
            
            _queue = new RabbitQueue("login-protocol", TestTools.ChannelProvider);

            _tokenApi = new TestApi<Startup, ITokenApiV1>
            {
                Output = output,
                ServiceOverrider = ConfigureTestApi
            };

            _protocolApi = new TestApi<Startup, IProtocolApiV1>
            {
                Output = output,
                ServiceOverrider = ConfigureTestApi
            };
        }

        [Fact]
        public void ShouldIndexEvent()
        {

        }

        public void Dispose()
        {
            _queue.Purge();
        }

        void ConfigureTestApi(IServiceCollection services)
        {
            services
                .Configure<RabbitOptions>(opt =>
                {
                    opt.Host = "localhost";
                    opt.Port = 5672;
                })
                .Configure<ApiClientsOptions>(opt =>
                {
                    opt.List.Add("searcher", new ApiConnectionOptions
                    {
                        Url = "http://localhost:8086"
                    });
                });
        }
    }
}