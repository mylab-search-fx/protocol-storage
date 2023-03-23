using System;
using MyLab.ProtocolStorage.Tools;
using Newtonsoft.Json.Linq;
using Xunit;

namespace UnitTests
{
    public class ProtocolEventToolsBehavior
    {
        [Fact]
        public void ShouldSetIdIfNotDefined()
        {
            //Arrange
            var json = new JObject();

            //Act
            var set = ProtocolEventTools.SetRandomIdIfNotDefined(json, out var actualId);

            //Assert
            Assert.True(set);
            Assert.NotNull(actualId);
            Assert.Equal(actualId, json.Property(ProtocolEventTools.IdPropertyName)?.Value.Value<string>());
        }

        [Fact]
        public void ShouldSetIdIfNull()
        {
            //Arrange
            var json = new JObject
            {
                { ProtocolEventTools.IdPropertyName, null }
            };

            //Act
            var set = ProtocolEventTools.SetRandomIdIfNotDefined(json, out var actualId);

            //Assert
            Assert.True(set);
            Assert.NotNull(actualId);
            Assert.Equal(actualId, json.Property(ProtocolEventTools.IdPropertyName)?.Value.Value<string>());
        }

        [Fact]
        public void ShouldNotSetIdIfSpecified()
        {
            //Arrange
            var json = new JObject
            {
                { ProtocolEventTools.IdPropertyName, "foo" }
            };

            //Act
            var set = ProtocolEventTools.SetRandomIdIfNotDefined(json, out var actualId);

            //Assert
            Assert.False(set);
            Assert.Equal("foo", actualId);
            Assert.Equal("foo", json.Property(ProtocolEventTools.IdPropertyName)?.Value.Value<string>());
        }

        [Fact]
        public void ShouldSetDatetimeIfNotDefined()
        {
            //Arrange
            var json = new JObject();

            //Act
            var set = ProtocolEventTools.SetDateTimeNowIfNotDefined(json, out var actualDt);

            //Assert
            Assert.True(set);
            Assert.NotNull(actualDt);
            Assert.Equal(actualDt, json.Property(ProtocolEventTools.DatetimePropertyName)?.Value.Value<DateTime>());
        }

        [Fact]
        public void ShouldSetDatetimeIfNull()
        {
            //Arrange
            var json = new JObject
            {
                { ProtocolEventTools.DatetimePropertyName, null }
            };

            //Act
            var set = ProtocolEventTools.SetDateTimeNowIfNotDefined(json, out var actualDt);

            //Assert
            Assert.True(set);
            Assert.NotNull(actualDt);
            Assert.Equal(actualDt, json.Property(ProtocolEventTools.DatetimePropertyName)?.Value.Value<DateTime>());
        }

        [Fact]
        public void ShouldNotSetDatetimeSpecified()
        {
            //Arrange
            var now = DateTime.Now.AddMinutes(-1);
            var json = new JObject
            {
                { ProtocolEventTools.DatetimePropertyName, now }
            };

            //Act
            var set = ProtocolEventTools.SetDateTimeNowIfNotDefined(json, out var actualDt);

            //Assert
            Assert.False(set);
            Assert.Equal(now, actualDt);
            Assert.Equal(now, json.Property(ProtocolEventTools.DatetimePropertyName)?.Value.Value<DateTime>());
        }

        [Theory]
        [InlineData("00-80e1afed08e019fc1110464cfa66635c-7a085853722dc6d2-01")]
        [InlineData("80e1afed08e019fc1110464cfa66635c")]
        public void ShouldSetTraceIdIfNotDefined(string taraceparentHeaderValue)
        {
            //Arrange
            var json = new JObject();

            //Act
            var set = ProtocolEventTools.SetTraceIdIfNotDefined(json, taraceparentHeaderValue, out var actualTraceId);

            //Assert
            Assert.True(set);
            Assert.NotNull(actualTraceId);
            Assert.Equal("80e1afed08e019fc1110464cfa66635c", actualTraceId);
            Assert.Equal("80e1afed08e019fc1110464cfa66635c", json.Property(ProtocolEventTools.TraceIdPropertyName)?.Value);
        }

        [Fact]
        public void ShouldNotSetTraceIdIfDefined()
        {
            //Arrange
            var json = new JObject
            {
                { ProtocolEventTools.TraceIdPropertyName, "7a085853722dc6d2" }
            };

            //Act
            var set = ProtocolEventTools.SetTraceIdIfNotDefined(json, "80e1afed08e019fc1110464cfa66635c", out var actualTraceId);

            //Assert
            Assert.False(set);
            Assert.Equal("7a085853722dc6d2", actualTraceId);
            Assert.Equal("7a085853722dc6d2", json.Property(ProtocolEventTools.TraceIdPropertyName)?.Value);
        }
    }
}
