using System.Text.Json.Nodes;
using Newtonsoft.Json;

namespace MyLab.ProtocolStorage.Models
{
    public class PushProtocolEntityRequest
    {
        /// <summary>
        /// Protocol entity
        /// </summary>
        [JsonProperty("entity")]
        public JsonObject Entity { get; set; }

        /// <summary>
        /// Entity labels
        /// </summary>
        [JsonProperty("labels")]
        public ProtocolEntityLabels? Labels { get; set; }

        /// <summary>
        /// Sender identifier
        /// </summary>
        [JsonProperty("sender")]
        public string? Sender { get; set; }

        public PushProtocolEntityRequest(JsonObject entity)
        {
            Entity = entity;
        }
    }

    public class ProtocolEntityLabels : Dictionary<string,string>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ProtocolEntityLabels"/>
        /// </summary>
        public ProtocolEntityLabels()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ProtocolEntityLabels"/>
        /// </summary>
        public ProtocolEntityLabels(IDictionary<string,string> initial)
            :base(initial)
        {
            
        }
    }
}
