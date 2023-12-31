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
    public class MessageJsonConverter : JsonConverter<ChatCompletionRoleRequestMessage>
    {

        //TODO: https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/converters-how-to?source=recommendations&pivots=dotnet-8-0#support-polymorphic-deserialization


        public override bool CanConvert(Type typeToConvert) =>
            typeof(ChatCompletionRoleRequestMessage).IsAssignableFrom(typeToConvert);

        public override ChatCompletionRoleRequestMessage? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            ChatCompletionRoleRequestMessage? ret = null;
            Utf8JsonReader originalReader = reader;
            Utf8JsonReader specializedReaderClone = reader;
            try
            {
                Utf8JsonReader readerClone = reader;
                var baseMessage = JsonSerializer.Deserialize<ChatCompletionRoleRequestMessage>(ref readerClone);
                if (baseMessage != null)
                {
                    switch (baseMessage.Role)
                    {
                        case ChatCompletionMessageRoleEnum.Assistant:
                            {
                                ret = JsonSerializer.Deserialize<ChatCompletionAssistantMessage>(ref specializedReaderClone);
                            }
                            break;
                        case ChatCompletionMessageRoleEnum.Function:
                            {
                                ret = JsonSerializer.Deserialize<ChatCompletionFunctionMessage>(ref specializedReaderClone);
                                
                            }
                            break;
                        case ChatCompletionMessageRoleEnum.System:
                            {
                                ret = JsonSerializer.Deserialize<ChatCompletionSystemMessage>(ref specializedReaderClone);
                            }
                            break;
                        case ChatCompletionMessageRoleEnum.Tool:
                            {
                                ret = JsonSerializer.Deserialize<ChatCompletionToolMessage>(ref specializedReaderClone);
                            }
                            break;
                        case ChatCompletionMessageRoleEnum.User:
                            {
                                ret = DeserializeUserRequestMessage(ref specializedReaderClone, typeToConvert, options);
                            }
                            break;
                    }
                }

                throw new JsonException();
            }
            finally
            {
                reader = originalReader;
            }

        }

        private ChatCompletionRoleRequestMessage? DeserializeUserRequestMessage(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Utf8JsonReader originalReader = reader;
            Utf8JsonReader cloneReader = reader;
            ChatCompletionRoleRequestMessage? ret = null;
            try
            {
                if (reader.TokenType != JsonTokenType.StartObject)
                {
                    throw new JsonException();
                }
                cloneReader.Read();

                while (cloneReader.Read())
                {
                    if (cloneReader.TokenType == JsonTokenType.EndObject)
                    {
                        return ret;
                    }

                    if (cloneReader.TokenType == JsonTokenType.PropertyName)
                    {
                        string? propertyName = cloneReader.GetString();
                        if (propertyName == nameof(ChatCompletionUserContentMessage.Content))
                        {
                            if (cloneReader.TokenType == JsonTokenType.String)
                            {
                                ret = JsonSerializer.Deserialize<ChatCompletionUserContentMessage>(ref cloneReader);
                            }
                            else
                            {
                                ret = JsonSerializer.Deserialize<ChatCompletionUserContentsMessage>(ref cloneReader);
                            }
                        }
                    }
                }
            }
            finally
            {
                reader = originalReader;
            }

            return ret;

        }

        public override void Write(Utf8JsonWriter writer, ChatCompletionRoleRequestMessage value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);            
        }
    }

}
