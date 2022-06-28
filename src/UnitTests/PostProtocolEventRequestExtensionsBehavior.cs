using System;
using System.Linq;
using MyLab.ProtocolStorage.Models;
using Newtonsoft.Json.Linq;
using Xunit;

namespace UnitTests
{
    public class PostProtocolEventRequestExtensionsBehavior
    {
        [Fact]
        public void ShouldConvertRequestToJson()
        {
            //Arrange
            var eventObj = new EventObj
            {
                Value = Guid.NewGuid().ToString("N")
            };

            var req = new PostProtocolEventRequest
            {
                Subject = "foo",
                Sender = "bar",
                Event = JObject.FromObject(eventObj),
                DateTime = DateTime.Now
            };

            //Act
            var json = req.ToJsonWithMetadata(DateTime.Now);

            var metadata = ProtocolEventMetadata.FromJsonEvent(json);
            ProtocolEventMetadata.CleanupJson(json);

            var jsonProps = json.Properties().ToArray();

            var eventProp = jsonProps.FirstOrDefault(p => p.Name == nameof(eventObj.Value));

            //Assert
            Assert.NotNull(metadata);
            Assert.Equal(req.Sender,metadata.Sender);
            Assert.Equal(req.Subject,metadata.Subject);
            Assert.Equal(req.DateTime,metadata.DateTime);

            Assert.Single(jsonProps);

            Assert.NotNull(eventProp);
            Assert.Equal(eventObj.Value, eventProp.Value.ToString());
        }

        class EventObj
        {
            public string Value { get; set; }
        }
    }
}
