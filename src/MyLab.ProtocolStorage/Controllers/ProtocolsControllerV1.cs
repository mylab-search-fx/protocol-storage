using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using MyLab.Log.Dsl;
using MyLab.ProtocolStorage.Models;
using MyLab.RabbitClient.Publishing;
using MyLab.Search.Searcher.Client;
using QuerySearchStrategy = MyLab.Search.Searcher.Client.QuerySearchStrategy;

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
        public IActionResult PushEntity([FromQuery] string protocolId, [FromBody] PushProtocolEntityRequest request)
        {
            if (string.IsNullOrWhiteSpace(protocolId))
                return BadRequest("ProtocolId is not specified");
            
            request.DateTime ??= DateTime.Now;

            _rabbitPublisher.IntoDefault().SendJson(request).Publish();

            return Ok();
        }

        [HttpPost("{protocolId}/searcher")]
        public async Task<IActionResult> SearchAsync(
            [FromQuery] string protocolId, 
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
                    new Search.Searcher.Client.FilterRef
                    {
                        Id = f.Id,
                        Args = f.Args
                    }
                ).ToArray(),
                QuerySearchStrategy = (QuerySearchStrategy)request.QuerySearchStrategy
            };

            var res = await _searcherApi.SearchAsync<JsonObject>(protocolId, searchReq, searchToken);

            return Ok(res);

        }
    }
}
