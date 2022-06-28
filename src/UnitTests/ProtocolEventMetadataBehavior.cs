using System;
using MyLab.ProtocolStorage.Models;
using Newtonsoft.Json.Linq;
using Xunit;

namespace UnitTests
{
    public class ProtocolEventMetadataBehavior
    {
        [Fact]
        public void ShouldCreateFromPostRequestWithoutDt()
        {
            //Arrange
            var req = new PostProtocolEventRequest
            {
                Subject = "foo",
                Sender = "bar",
                Event = JObject.FromObject(new object()),
                DateTime = null
            };

            var defaultDatetime = DateTime.Now;

            //Act
            var metadata = ProtocolEventMetadata.FromPostRequest(req, defaultDatetime);

            //Assert
            Assert.Equal(req.Sender, metadata.Sender);
            Assert.Equal(req.Subject, metadata.Subject);
            Assert.Equal(defaultDatetime, metadata.DateTime);
        }

        [Fact]
        public void ShouldCreateFromPostRequestWithDt()
        {
            //Arrange
            var req = new PostProtocolEventRequest
            {
                Subject = "foo",
                Sender = "bar",
                Event = JObject.FromObject(new object()),
                DateTime = DateTime.Now.AddMinutes(-1)
            };

            var defaultDatetime = DateTime.Now;

            //Act
            var metadata = ProtocolEventMetadata.FromPostRequest(req, defaultDatetime);

            //Assert
            Assert.Equal(req.Sender, metadata.Sender);
            Assert.Equal(req.Subject, metadata.Subject);
            Assert.Equal(req.DateTime, metadata.DateTime);
        }

        [Fact]
        public void ShouldAddInJson()
        {
            //Arrange
            var eventObj = JObject.Parse("{\"Prop\":\"Val\"}");

            var metadata = new ProtocolEventMetadata
            {
                Subject = "foo",
                Sender = "bar",
                DateTime = DateTime.Now
            };

            //Act
            metadata.AddToJson(eventObj);

            var actual = ProtocolEventMetadata.FromJsonEvent(eventObj);

            //Assert
            Assert.Equal(metadata.Subject, actual.Subject);
            Assert.Equal(metadata.Sender, actual.Sender);
            Assert.Equal(metadata.DateTime, actual.DateTime);
        }

        [Fact]
        public void ShouldCleanupJson()
        {
            //Arrange
            var eventObj = JObject.Parse("{\"Prop\":\"Val\"}");

            var metadata = new ProtocolEventMetadata
            {
                Subject = "foo",
                Sender = "bar",
                DateTime = DateTime.Now
            };

            metadata.AddToJson(eventObj);

            //Act
            ProtocolEventMetadata.CleanupJson(eventObj);

            var actual = ProtocolEventMetadata.FromJsonEvent(eventObj);

            //Assert
            Assert.Null(actual);
        }
    }
}
