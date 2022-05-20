using System;
using MyLab.ApiClient.Test;
using MyLab.ProtocolStorage;
using MyLab.RabbitClient.Model;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests
{
    public class ProtocolStorageBehavior : IDisposable
    {
        private readonly ITestOutputHelper _output;
        private readonly RabbitQueue _queue;
        private readonly TestApi<Startup, ITestApi> _api;

        public ProtocolStorageBehavior(ITestOutputHelper output)
        {
            _output = output;
            
            _queue = new RabbitQueue("login-protocol", TestTools.ChannelProvider);

            _api = new TestApi< Startup, ITestApi>
            {
                Output = output,
                //ServiceOverrider = services => {},
                //HttpClientTuner = client => {}
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
    }
}