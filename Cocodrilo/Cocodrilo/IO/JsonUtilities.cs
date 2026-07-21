using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Cocodrilo.IO
{
    /// Cross-platform replacement for System.Web.Script.Serialization.JavaScriptSerializer,
    /// which only exists on net48/Windows. Newtonsoft.Json works identically on net48 and net7.0.
    public static class JsonUtilities
    {
        static readonly JsonSerializerSettings PolymorphicSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        };

        public static string SerializePolymorphic(object value)
            => JsonConvert.SerializeObject(value, PolymorphicSettings);

        public static T DeserializePolymorphic<T>(string json)
            => JsonConvert.DeserializeObject<T>(json, PolymorphicSettings);

        static readonly JsonSerializerSettings KratosSettings = new JsonSerializerSettings
        {
            Converters = { new WholeNumberDoubleConverter() }
        };

        // Plain JSON with no .NET type metadata - used for Kratos solver input files,
        // which must stay clean, external-tool-readable JSON.
        public static string Serialize(object value)
            => JsonConvert.SerializeObject(value, KratosSettings);

        class WholeNumberDoubleConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType) => objectType == typeof(double) || objectType == typeof(double?);
            public override bool CanRead => false;

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                if (value == null) { writer.WriteNull(); return; }
                var d = (double)value;
                if (!double.IsInfinity(d) && !double.IsNaN(d) && d == Math.Floor(d) && Math.Abs(d) < 1e15)
                    writer.WriteValue((long)d);
                else
                    writer.WriteValue(d);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
                => throw new NotSupportedException();
        }

        public static Dictionary<string, object> DeserializeWeaklyTyped(string json)
            => (Dictionary<string, object>)ToWeaklyTyped(JToken.Parse(json));

        static object ToWeaklyTyped(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    var dict = new Dictionary<string, object>();
                    foreach (var prop in ((JObject)token).Properties())
                        dict[prop.Name] = ToWeaklyTyped(prop.Value);
                    return dict;
                case JTokenType.Array:
                    var list = new ArrayList();
                    foreach (var item in (JArray)token)
                        list.Add(ToWeaklyTyped(item));
                    return list;
                case JTokenType.Integer:
                    return token.Value<long>();
                case JTokenType.Float:
                    return token.Value<double>();
                case JTokenType.String:
                    return token.Value<string>();
                case JTokenType.Boolean:
                    return token.Value<bool>();
                case JTokenType.Null:
                case JTokenType.Undefined:
                    return null;
                default:
                    return token.ToObject<object>();
            }
        }
    }
}
