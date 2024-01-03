using DevExtremeAI.OpenAIDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DevExtremeAI.Utils
{
    public class ContentItemListJsonConverter : JsonConverter<List<ContentItem>>
    {

        //TODO: https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/converters-how-to?source=recommendations&pivots=dotnet-8-0#support-polymorphic-deserialization

        public override bool CanConvert(Type typeToConvert) => true;
            //typeof(List<ChatCompletionRoleRequestMessage>).IsAssignableFrom(typeToConvert);

        public override List<ContentItem> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            List<ContentItem>? ret = null;
            Utf8JsonReader originalReader = reader;
            Utf8JsonReader specializedReader = reader;
            try
            {
                Utf8JsonReader readerClone = reader;
                ret = new List<ContentItem>();

                while (reader.Read())
                {

                    if (reader.TokenType == JsonTokenType.EndArray)
                    {
                        break;
                    }

                    if (reader.TokenType == JsonTokenType.StartObject)
                    {
                        var item = JsonSerializer.Deserialize<ContentItem>(ref reader);
                        if (item != null)
                        {
                            ret.Add(item);
                        }
                    }
                }

                return ret;

            }
            finally
            {
                //reader = originalReader;
            }

        }

        public override void Write(Utf8JsonWriter writer, List<ContentItem> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            if (value != null)
            {
                foreach (ContentItem item in value)
                {
                    JsonSerializer.Serialize(writer, item, options);
                }
            }

            writer.WriteEndArray();

        }
    }

}
