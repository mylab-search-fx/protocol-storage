using MyLab.RabbitClient;
using MyLab.RabbitClient.Connection;

namespace IntegrationTests
{
    internal static class TestTools
    {
        public static RabbitChannelProvider ChannelProvider { get; }

        static TestTools()
        {
            var opts = new RabbitOptions
            {
                Host = "localhost",
                Port = 5672,
                User = "guest",
                Password = "guest"
            };

            var connProvider = new LazyRabbitConnectionProvider(opts);
            ChannelProvider = new RabbitChannelProvider(connProvider);
        }
    }
}
