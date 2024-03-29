﻿using System;
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
        /// Event type
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Trace identifier
        /// </summary>
        [JsonProperty("trace_id")]
        public string TraceId { get; set; }

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
