using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Cocodrilo.IO
{
    /// <summary>
    /// Cross-platform replacement for System.Web.Script.Serialization.JavaScriptSerializer,
    /// which only exists on net48/Windows. Newtonsoft.Json works identically on net48 and net7.0.
    /// </summary>
    public static class JsonUtilities
    {
        // TypeNameHandling.Auto embeds "$type" metadata for polymorphic members/collections
        // (e.g. List<Material>, List<Analysis>) so they deserialize back to their concrete
        // subclass, matching what JavaScriptSerializer's SimpleTypeResolver used to do.
        static readonly JsonSerializerSettings PolymorphicSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        };

        public static string SerializePolymorphic(object value)
            => JsonConvert.SerializeObject(value, PolymorphicSettings);

        public static T DeserializePolymorphic<T>(string json)
            => JsonConvert.DeserializeObject<T>(json, PolymorphicSettings);

        // Plain JSON with no .NET type metadata - used for Kratos solver input files,
        // which must stay clean, external-tool-readable JSON.
        public static string Serialize(object value)
            => JsonConvert.SerializeObject(value);

        // Deserializes loosely-typed JSON (e.g. Kratos-produced geometry/result files) the way
        // JavaScriptSerializer used to: JSON objects become Dictionary<string, object>, JSON
        // arrays become ArrayList. Newtonsoft's own default for "object" members is JObject/
        // JArray, which existing call sites (expecting ArrayList via "as" casts) don't understand,
        // so this walks the parsed token tree and converts it to match the old shape.
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
