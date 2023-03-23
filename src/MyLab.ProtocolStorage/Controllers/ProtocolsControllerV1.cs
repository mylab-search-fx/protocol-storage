using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyLab.ApiClient;
using MyLab.Log.Dsl;
using MyLab.ProtocolStorage.Models;
using MyLab.ProtocolStorage.Tools;
using MyLab.RabbitClient.Publishing;
using MyLab.Search.IndexerClient;
using MyLab.Search.SearcherClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyLab.ProtocolStorage.Controllers
{
    [Route("v1/protocols")]
    [ApiController]
    public class ProtocolsControllerV1 : ControllerBase
    {
        private readonly IRabbitPublisher _rabbitPublisher;
        private readonly ISearcherApiV3 _searcherApi;
        private readonly IDslLogger _log;

        /// <summary>
        /// Initializes a new instance of <see cref="ProtocolsControllerV1"/>
        /// </summary>
        public ProtocolsControllerV1(
            IRabbitPublisher rabbitPublisher,
            ISearcherApiV3 searcherApi,
            ILogger<ProtocolsControllerV1> logger)
        {
            _rabbitPublisher = rabbitPublisher;
            _searcherApi = searcherApi;
            _log = logger.Dsl();
        }

        [HttpPost("{protocolId}/collector")]
        public async Task<IActionResult> PushEvent([FromRoute] string protocolId)
        {
            if (string.IsNullOrWhiteSpace(protocolId))
                return BadRequest("ProtocolId is not specified");

            var protocolEvent = await ReadJsonFromRequestBodyAsync();

            ProtocolEventTools.SetRandomIdIfNotDefined(protocolEvent, out _);
            ProtocolEventTools.SetDateTimeNowIfNotDefined(protocolEvent, out _);
            ProtocolEventTools.SetTraceIdIfNotDefined(protocolEvent, Request.Headers["traceparent"], out _);

            var mwMsg = new IndexingMqMessage
            {
                IndexId = protocolId,
                Post = new[]
                {
                    protocolEvent
                }
            };

            _rabbitPublisher.IntoDefault().SetJsonContent(mwMsg).Publish();

            return Ok();
        }

        [HttpPost("{protocolId}/searcher")]
        public async Task<IActionResult> SearchAsync(
            [FromRoute] string protocolId,
            [FromBody] ClientSearchRequest request,
            [FromHeader(Name = "X-Search-Token")] string searchToken)
        {
            if (string.IsNullOrWhiteSpace(protocolId))
                return BadRequest("ProtocolId is not specified");

            var searchReq = new ClientSearchRequestV3
            {
                Offset = request.Offset,
                Limit = request.Limit,
                Query = request.Query,
                Filters = request.Filters?.Select(f =>
                    new Search.SearcherClient.FilterRef
                    {
                        Id = f.Id,
                        Args = f.Args
                    }
                ).ToArray(),
                QuerySearchStrategy = request.QuerySearchStrategy
            };
            
            FoundEntities<JObject> res;

            try
            {
                res = await _searcherApi.SearchAsync<JObject>(protocolId, searchReq, searchToken);
            }
            catch (ResponseCodeException e) when (e.StatusCode == HttpStatusCode.Forbidden)
            {
                return new ContentResult
                {
                    Content = e.ServerMessage,
                    ContentType = "text/plain",
                    StatusCode = 403
                };
            }
            
            return Ok(new SearchResult
            {
                EsRequest = res.EsRequest,
                Events = res.Entities,
                Total = res.Total
            });

        }

        private async Task<JObject> ReadJsonFromRequestBodyAsync()
        {
            JObject doc;

            using (TextReader txtRdr = new StreamReader(Request.Body))
            using (JsonReader jsonRdr = new JsonTextReader(txtRdr))
            {
                doc = await JObject.LoadAsync(jsonRdr);
            }

            return doc;
        }
    }
}
