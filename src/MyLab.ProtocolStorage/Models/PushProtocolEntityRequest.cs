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
        /// Entity subject identifier
        /// </summary>
        [JsonProperty("subject")]
        public string Subject { get; set; }

        /// <summary>
        /// Sender application identifier
        /// </summary>
        [JsonProperty("sender")]
        public string? Sender { get; set; }

        /// <summary>
        /// Associated date time 
        /// </summary>
        [JsonProperty("dateTime")]
        public DateTime? DateTime { get; set; }

        public PushProtocolEntityRequest(string subject, JsonObject entity)
        {
            Entity = entity;
            Subject = subject;
        }
    }
}
