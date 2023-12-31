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
    public class MessageListJsonConverter : JsonConverter<List<ChatCompletionRoleRequestMessage>>
    {

        //TODO: https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/converters-how-to?source=recommendations&pivots=dotnet-8-0#support-polymorphic-deserialization

        //public override ChatCompletionRoleRequestMessage ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        //{
        //    return base.ReadAsPropertyName(ref reader, typeToConvert, options);
        //}

        //public override bool Equals(object? obj)
        //{
        //    return base.Equals(obj);
        //}

        public override bool CanConvert(Type typeToConvert) => true;
            //typeof(List<ChatCompletionRoleRequestMessage>).IsAssignableFrom(typeToConvert);

        public override List<ChatCompletionRoleRequestMessage> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            List<ChatCompletionRoleRequestMessage>? ret = null;
            Utf8JsonReader originalReader = reader;
            Utf8JsonReader specializedReader = reader;
            try
            {
                Utf8JsonReader readerClone = reader;
                ret = new List<ChatCompletionRoleRequestMessage>();

                while (readerClone.Read())
                {

                    if (readerClone.TokenType == JsonTokenType.EndArray)
                    {
                        break;
                    }

                    if (readerClone.TokenType == JsonTokenType.StartObject)
                    {
                        var item = JsonSerializer.Deserialize<ChatCompletionRoleRequestMessage>(ref readerClone);
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
                reader = originalReader;
            }

        }

        public override void Write(Utf8JsonWriter writer, List<ChatCompletionRoleRequestMessage> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            if (value != null)
            {
                foreach (ChatCompletionRoleRequestMessage item in value)
                {
                    JsonSerializer.Serialize(writer, item, options);
                }
            }

            writer.WriteEndArray();

        }
    }

}
