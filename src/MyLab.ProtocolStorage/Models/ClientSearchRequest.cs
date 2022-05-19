using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MyLab.ProtocolStorage.Models;

public class ClientSearchRequest
{
    [JsonProperty("query")]
    public string? Query { get; set; }
    [JsonProperty("filters")]
    public FilterRef[]? Filters { get; set; }
    [JsonProperty("offset")]
    public int Offset { get; set; }
    [JsonProperty("limit")]
    public int Limit { get; set; }
    [JsonProperty("queryMode")]
    [JsonConverter(typeof(StringEnumConverter))]
    public QuerySearchStrategy QuerySearchStrategy { get; set; }
}