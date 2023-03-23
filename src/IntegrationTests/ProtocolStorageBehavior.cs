using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyLab.Search.SearcherClient;
using Xunit;

namespace IntegrationTests
{
    using ClientSearchRequest  = MyLab.ProtocolStorage.Client.Models.ClientSearchRequest;
    using FilterRef = MyLab.ProtocolStorage.Client.Models.FilterRef;

    public partial class ProtocolStorageBehavior 
    {
        [Fact]
        public async Task ShouldIndexEvent()
        {
            //Arrange
            var eventObj = new TestProtocolEvent
            {
                Id = Guid.NewGuid().ToString("N"),
                Message = "foo"
            };

            var eventObj2 = new TestProtocolEvent
            {
                Id = Guid.NewGuid().ToString("N"),
                Message = "foo2"
            };

            var searchReq = new ClientSearchRequest
            {
                Filters = new []
                {
                    new FilterRef
                    {
                        Id = "by-id",
                        Args = new Dictionary<string, string>
                        {
                            { "id", eventObj.Id }
                        }
                    }
                }
            };

            var searchToken = await _tokenApi.CreateTotalTokenAsync();

            //Act
            await _protocolApi.PostEventAsync("test", eventObj);
            await _protocolApi.PostEventAsync("test", eventObj2);
            await Task.Delay(1000);

            var searchRes=  await _protocolApi.SearchAsync("test", searchReq, searchToken);

            var foundEvent = searchRes.Events.Select(e => e.Content.ToObject<TestProtocolEvent>()).FirstOrDefault();

            //Assert
            Assert.Single(searchRes.Events);
            Assert.NotNull(foundEvent);
            Assert.Equal(eventObj.Id, foundEvent.Id);
            Assert.True(DateTime.Now.AddMinutes(-1) < foundEvent.DateTime && foundEvent.DateTime < DateTime.Now);
            Assert.Equal(eventObj.Subject, foundEvent.Subject);
        }

        [Fact]
        public async Task ShouldFilterBySubject()
        {
            //Arrange
            var fooSubjectEvent = new TestProtocolEvent
            {
                Id = Guid.NewGuid().ToString("N"),
                Message = "foo",
                Subject = "subject-foo",
                DateTime = DateTime.Now
            };

            var barSubjectEvent = new TestProtocolEvent
            {
                Id = Guid.NewGuid().ToString("N"),
                Message = "bar",
                Subject = "subject-bar"
            };

            var searchReq = new ClientSearchRequest();

            var searchToken = await _tokenApi.CreateTokenForSubjectAsync("subject-foo");

            //Act
            await _protocolApi.PostEventAsync("test", fooSubjectEvent);
            await _protocolApi.PostEventAsync("test", barSubjectEvent);
            await Task.Delay(1000);

            var searchRes = await _protocolApi.SearchAsync("test", searchReq, searchToken);

            var foundEvent = searchRes.Events
                .Select(e => e.Content.ToObject<TestProtocolEvent>())
                .FirstOrDefault();

            //Assert
            Assert.Single(searchRes.Events);
            Assert.NotNull(foundEvent);
            Assert.Equal(fooSubjectEvent.Id, foundEvent.Id);
            Assert.Equal(fooSubjectEvent.DateTime, foundEvent.DateTime);
            Assert.Equal(fooSubjectEvent.Subject, foundEvent.Subject);
        }

        [Fact]
        public async Task ShouldFilterByTrace()
        {
            //Arrange
            var fooSubjectEvent = new TestProtocolEvent
            {
                Id = Guid.NewGuid().ToString("N"),
                Message = "foo",
                Subject = "subject-foo",
                TraceId = "11CBE9EB5B9245BF8A9F30CFD2AF6D3E",
                DateTime = DateTime.Now
            };

            var barSubjectEvent = new TestProtocolEvent
            {
                Id = Guid.NewGuid().ToString("N"),
                Message = "bar",
                Subject = "subject-bar",
                TraceId = "DC5478B70EB2451E8C58C7880DA39B7D",
                DateTime = DateTime.Now
            };

            var searchReq = new ClientSearchRequest
            {
                Filters = new []
                {
                    new FilterRef
                    { 
                        Id = "by-trace", 
                        Args = new Dictionary<string, string>
                        {
                            { "trace_id", "DC5478B70EB2451E8C58C7880DA39B7D" }
                        }
                    }
                }
            };

            var searchToken = await _tokenApi.CreateTotalTokenAsync();

            //Act
            await _protocolApi.PostEventAsync("test", fooSubjectEvent);
            await _protocolApi.PostEventAsync("test", barSubjectEvent);
            await Task.Delay(1000);

            var searchRes = await _protocolApi.SearchAsync("test", searchReq, searchToken);

            var foundEvent = searchRes.Events.Select(e => e.Content.ToObject<TestProtocolEvent>()).FirstOrDefault();

            //Assert
            Assert.Single(searchRes.Events);
            Assert.NotNull(foundEvent);
            Assert.Equal(barSubjectEvent.Id, foundEvent.Id);
            Assert.Equal(barSubjectEvent.DateTime, foundEvent.DateTime);
            Assert.Equal(barSubjectEvent.Subject, foundEvent.Subject);
        }

    }
}