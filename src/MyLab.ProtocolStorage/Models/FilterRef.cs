using System.Collections.Generic;
using Newtonsoft.Json;

#if SERVER
namespace MyLab.ProtocolStorage.Models
#else
namespace MyLab.ProtocolStorage.Client.Models
#endif
{
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
        public Dictionary<string, string> Args { get; set; }
    }
}