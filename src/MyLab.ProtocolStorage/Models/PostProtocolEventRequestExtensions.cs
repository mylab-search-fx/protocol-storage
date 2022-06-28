using System;
using Newtonsoft.Json.Linq;

namespace MyLab.ProtocolStorage.Models
{
    static class PostProtocolEventRequestExtensions
    {
        public static JObject ToJsonWithMetadata(this PostProtocolEventRequest req, DateTime defaultDateTime)
        {
            var json = new JObject(req.Event);

            var metadata = ProtocolEventMetadata.FromPostRequest(req, defaultDateTime);
            metadata.AddToJson(json);

            return json;
        }
    }
}
