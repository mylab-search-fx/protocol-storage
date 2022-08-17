using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using MyLab.Log.Dsl;
using MyLab.ProtocolStorage.Client;
using MyLab.ProtocolStorage.Client.Models;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests
{
    public class SafeProtocolIndexerV1Behavior
    {
        private readonly ITestOutputHelper _output;

        /// <summary>
        /// Initializes a new instance of <see cref="SafeProtocolIndexerV1Behavior"/>
        /// </summary>
        public SafeProtocolIndexerV1Behavior(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task ShouldNotThrowAnError()
        {
            //Arrange
            var api = new Mock<IProtocolApiV1>();
            api.Setup(p => p.PostEventAsync(
                    It.IsAny<string>(), 
                    It.IsAny<ProtocolEvent>()))
                .Throws(new Exception());

            var loggerFactory = new LoggerFactory().AddXUnit(_output);
            var logger = loggerFactory.CreateLogger("SafeProtocolIndexerV1Behaviors");
            var dslLogger = logger.Dsl();

            var indexer = new SafeProtocolIndexerV1(api.Object, dslLogger);

            var eventObj = new TestProtocolEvent
            {
                Account = "ololo@mytest.com",
                Action = "login"
            };

            //Act
            await indexer.PostEventAsync("foo", eventObj);

            //Assert

        }

        class TestProtocolEvent : ProtocolEvent
        {
            public string Account { get; set; }
            public string Action { get; set; }
        }
    }
}
