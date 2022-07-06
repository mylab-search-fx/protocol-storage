using MyLab.Search.Searcher.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

#if SERVER
namespace MyLab.ProtocolStorage.Models
#else
namespace MyLab.ProtocolStorage.Client.Models
#endif
{
    public class ClientSearchRequest
    {
        [JsonProperty("query")]
        public string Query { get; set; }
        [JsonProperty("filters")]
        public FilterRef[] Filters { get; set; }
        [JsonProperty("offset")]
        public int Offset { get; set; }
        [JsonProperty("limit")]
        public int Limit { get; set; }
        [JsonProperty("queryMode")]
        [JsonConverter(typeof(StringEnumConverter))]
        public QuerySearchStrategy QuerySearchStrategy { get; set; }
    }
}
