using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyLab.ProtocolStorage.Models
{
    static class ProtocolEventTools
    {
        public static bool SetRandomIdIfNotDefined(JObject json, out string actualId)
        {
            var prop = json.Property("id", StringComparison.InvariantCultureIgnoreCase);

            if (prop == null)
            {
                json.Add("id", null);
                prop = json.Property("id");
            }

            if (prop == null)
                throw new InvalidOperationException("Cant create 'id' property for protocol event");

            if (prop.Value.Type == JTokenType.Null)
            {
                actualId = Guid.NewGuid().ToString("N");
                prop.Value = JToken.FromObject(actualId);

                return true;
            }
            
            actualId = prop.Value.Value<string>();
            return false;
        }

        public static bool SetDateTimeNowIfNotDefined(JObject json, out DateTime? actualDateTime)
        {
            var prop = json.Property("datetime", StringComparison.InvariantCultureIgnoreCase);

            if (prop == null)
            {
                json.Add("datetime", null);
                prop = json.Property("datetime");
            }

            if (prop == null)
                throw new InvalidOperationException("Cant create 'id' property for protocol event");

            if (prop.Value.Type == JTokenType.Null)
            {
                actualDateTime = DateTime.Now;
                prop.Value = JToken.FromObject(actualDateTime);

                return true;
            }

            actualDateTime = prop.Value.Value<DateTime>();
            return false;
        }
    }
}
