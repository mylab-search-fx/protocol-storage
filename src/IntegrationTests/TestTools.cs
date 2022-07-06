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
                User = "test",
                Password = "AICQ0kiYgjUcQNL1AfRA"
            };

            var connProvider = new LazyRabbitConnectionProvider(opts);
            ChannelProvider = new RabbitChannelProvider(connProvider);
        }
    }
}
