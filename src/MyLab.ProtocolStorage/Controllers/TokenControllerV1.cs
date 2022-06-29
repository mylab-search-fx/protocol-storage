using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyLab.Search.Searcher.Client;

namespace MyLab.ProtocolStorage.Controllers
{
    [Route("v1/search-token")]
    [ApiController]
    public class TokenControllerV1 : ControllerBase
    {
        private readonly ISearcherApiV3 _searcherApi;

        /// <summary>
        /// Initializes a new instance of <see cref="TokenControllerV1"/>
        /// </summary>
        public TokenControllerV1(ISearcherApiV3 searcherApi)
        {
            _searcherApi = searcherApi;
        }

        [HttpPost("total")]
        public async Task<IActionResult> CreateTotalTokenAsync()
        {
            var resp = await _searcherApi.CreateSearchTokenAsync(new TokenRequestV3
            {
                Namespaces = new[]
                {
                    new NamespaceSettingsV3
                    {
                        Name = "*"
                    }
                }
            });

            return Ok(resp);
        }

        [HttpPost("for-subject/{subjectId}")]
        public async Task<IActionResult> CreateTokenForSubject(string subjectId)
        {
            if (string.IsNullOrWhiteSpace(subjectId))
                return BadRequest("SubjectId is not specified");

            var resp = await _searcherApi.CreateSearchTokenAsync(new TokenRequestV3
            {
                Namespaces = new[]
                {
                    new NamespaceSettingsV3
                    {
                        Name = "*",
                        Filters = new []
                        {
                            new FilterRef
                            {
                                Id = "by-subject",
                                Args = new Dictionary<string, string>
                                {
                                    {"subject", subjectId}
                                }
                            }
                        }
                    }
                }
            });

            return Ok(resp);
        }
    }
}