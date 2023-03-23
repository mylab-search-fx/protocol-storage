using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyLab.ProtocolStorage.Tools
{
    static class ProtocolEventTools
    {
        public const string IdPropertyName = "id";
        public const string DatetimePropertyName = "datetime";
        public const string TraceIdPropertyName = "trace_id";

        public static bool SetRandomIdIfNotDefined(JObject json, out string actualId)
        {
            var prop = json.Property(IdPropertyName, StringComparison.InvariantCultureIgnoreCase);

            if (prop == null)
            {
                json.Add(IdPropertyName, null);
                prop = json.Property(IdPropertyName);
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
            var prop = json.Property(DatetimePropertyName, StringComparison.InvariantCultureIgnoreCase);

            if (prop == null)
            {
                json.Add(DatetimePropertyName, null);
                prop = json.Property(DatetimePropertyName);
            }

            if (prop == null)
                throw new InvalidOperationException("Cant create 'datetime' property for protocol event");

            if (prop.Value.Type == JTokenType.Null)
            {
                actualDateTime = DateTime.Now;
                prop.Value = JToken.FromObject(actualDateTime);

                return true;
            }

            actualDateTime = prop.Value.Value<DateTime>();
            return false;
        }

        public static bool SetTraceIdIfNotDefined(JObject json, string traceParentHeaderValue, out string actualTraceId)
        {
            string traceFromHeader = null;

            if (traceParentHeaderValue != null)
            {
                var traceParentParts = traceParentHeaderValue.Split('-');

                if (traceParentParts.Length != 0)
                {
                    traceFromHeader = traceParentParts.Length == 1
                        ? traceParentParts[0]
                        : traceParentParts[1];
                }
            }

            var prop = json.Property(TraceIdPropertyName, StringComparison.InvariantCultureIgnoreCase);

            if (prop == null)
            {
                if (traceFromHeader == null)
                {
                    actualTraceId = null;
                    return false;
                }

                json.Add(TraceIdPropertyName, null);
                prop = json.Property(TraceIdPropertyName);
            }

            if (prop == null)
                throw new InvalidOperationException("Cant create 'trace_id' property for protocol event");

            if (prop.Value.Type == JTokenType.Null)
            {
                if (traceFromHeader == null)
                {
                    actualTraceId = null;
                    return false;
                }

                prop.Value = JToken.FromObject(traceFromHeader);
                actualTraceId = traceFromHeader;

                return true;
            }

            actualTraceId = prop.Value.Value<string>();
            return false;
        }
    }
}
