using MyLab.Search.Searcher.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#if SERVER
namespace MyLab.ProtocolStorage.Models
#else
namespace MyLab.ProtocolStorage.Client.Models
#endif
{
    public class SearchResult
    {
        [JsonProperty("events")]
        public FoundEntity<JObject>[] Events { get; set; }
        [JsonProperty("esRequest")]
        public object EsRequest { get; set; }
        [JsonProperty("total")]
        public long Total { get; set; }
    }
}
