using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyLab.ProtocolStorage.Models
{
    public class PushProtocolEventRequest
    {
        /// <summary>
        /// Protocol event
        /// </summary>
        [JsonProperty("event")]
        public JObject Event { get; set; }

        /// <summary>
        /// Event subject identifier
        /// </summary>
        [JsonProperty("subject")]
        public string Subject { get; set; }

        /// <summary>
        /// Sender application identifier
        /// </summary>
        [JsonProperty("sender")]
        public string Sender { get; set; }

        /// <summary>
        /// Associated date time 
        /// </summary>
        [JsonProperty("dateTime")]
        public DateTime? DateTime { get; set; }
    }
}