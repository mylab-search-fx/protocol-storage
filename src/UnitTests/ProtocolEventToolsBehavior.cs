using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLab.ProtocolStorage.Models;
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
            Assert.Equal(actualId, json.Property("id")?.Value.Value<string>());
        }

        [Fact]
        public void ShouldSetIdIfNull()
        {
            //Arrange
            var json = new JObject
            {
                { "id", null }
            };

            //Act
            var set = ProtocolEventTools.SetRandomIdIfNotDefined(json, out var actualId);

            //Assert
            Assert.True(set);
            Assert.NotNull(actualId);
            Assert.Equal(actualId, json.Property("id")?.Value.Value<string>());
        }

        [Fact]
        public void ShouldNotSetIdSpecified()
        {
            //Arrange
            var json = new JObject
            {
                { "id", "foo" }
            };

            //Act
            var set = ProtocolEventTools.SetRandomIdIfNotDefined(json, out var actualId);

            //Assert
            Assert.False(set);
            Assert.Equal("foo", actualId);
            Assert.Equal("foo", json.Property("id")?.Value.Value<string>());
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
            Assert.Equal(actualDt, json.Property("datetime")?.Value.Value<DateTime>());
        }

        [Fact]
        public void ShouldSetDatetimeIfNull()
        {
            //Arrange
            var json = new JObject
            {
                { "datetime", null }
            };

            //Act
            var set = ProtocolEventTools.SetDateTimeNowIfNotDefined(json, out var actualDt);

            //Assert
            Assert.True(set);
            Assert.NotNull(actualDt);
            Assert.Equal(actualDt, json.Property("datetime")?.Value.Value<DateTime>());
        }

        [Fact]
        public void ShouldNotSetDatetimeSpecified()
        {
            //Arrange
            var now = DateTime.Now.AddMinutes(-1);
            var json = new JObject
            {
                { "datetime", now }
            };

            //Act
            var set = ProtocolEventTools.SetDateTimeNowIfNotDefined(json, out var actualDt);

            //Assert
            Assert.False(set);
            Assert.Equal(now, actualDt);
            Assert.Equal(now, json.Property("datetime")?.Value.Value<DateTime>());
        }
    }
}
