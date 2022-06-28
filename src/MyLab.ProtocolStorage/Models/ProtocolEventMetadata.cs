using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyLab.ProtocolStorage.Models
{
    public class ProtocolEventMetadata
    {
        const string MetadataPropertyName = "_protocol";

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
        public DateTime DateTime { get; set; }

        public void AddToJson(JObject target)
        {
            target.Add(MetadataPropertyName, JObject.FromObject(this));
        }

        public static ProtocolEventMetadata FromPostRequest(PostProtocolEventRequest request, DateTime defaultDateTime)
        {
            return new ProtocolEventMetadata
            {
                Sender = request.Sender,
                DateTime = request.DateTime ?? defaultDateTime,
                Subject = request.Subject
            };
        }

        public static ProtocolEventMetadata FromJsonEvent(JObject jsonEvent)
        {
            var prop = jsonEvent.Property(MetadataPropertyName);

            if (prop == null || !prop.HasValues || prop.Value.Type == JTokenType.Null)
                return null;

            return prop.Value.ToObject<ProtocolEventMetadata>();
        }

        public static void CleanupJson(JObject obj)
        {
            obj.Remove(MetadataPropertyName);
        }
    }
}
