using Newtonsoft.Json;

namespace MyLab.ProtocolStorage.Models;

/// <summary>
/// Reference to filter
/// </summary>
public class FilterRef
{
    /// <summary>
    /// Filter identifier
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }
    /// <summary>
    /// Named filter args
    /// </summary>
    [JsonProperty("args")]
    public Dictionary<string, string>? Args { get; set; }

    public FilterRef(string id)
    {
        Id = id;
    }
}