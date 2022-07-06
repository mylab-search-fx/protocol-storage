using System;
using Newtonsoft.Json;

namespace MyLab.ProtocolStorage.Client.Models
{
    /// <summary>
    /// Contains protocol event data
    /// </summary>
    public class ProtocolEvent
    {
        /// <summary>
        /// Event identifier
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Event subject identifier
        /// </summary>
        [JsonProperty("subject")]
        public string Subject { get; set; }

        /// <summary>
        /// Associated date time 
        /// </summary>
        [JsonProperty("datetime")]
        public DateTime? DateTime { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ProtocolEvent"/>
        /// </summary>
        protected ProtocolEvent()
        {
            
        }
    }
}
