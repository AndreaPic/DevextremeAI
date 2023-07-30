using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DevExtremeAI.Utils
{
    internal sealed class JsonStringArgumentsDictionaryConverter
        : JsonConverter<IDictionary<string, object>>
    {
        public override IDictionary<string, object>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Dictionary<string, object> ret = null;

            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException();
            }

            var dictionary = new Dictionary<string, object>();

            var arguments = reader.GetString();

            if (!string.IsNullOrEmpty(arguments))
            {
                ret = JsonSerializer.Deserialize<Dictionary<string, object>>(arguments);
            }
            else
            {
                ret = new Dictionary<string, object>();
            }

            return ret;
        }

        public override void Write(Utf8JsonWriter writer, IDictionary<string, object> dictionary, JsonSerializerOptions options)
        {
            var json = JsonSerializer.Serialize<IDictionary<string,object>>(dictionary, options);

            writer.WriteStringValue(json);
            
        }
    }
}
