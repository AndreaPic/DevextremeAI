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

        //public override ChatCompletionRoleRequestMessage ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        //{
        //    return base.ReadAsPropertyName(ref reader, typeToConvert, options);
        //}

        //public override bool Equals(object? obj)
        //{
        //    return base.Equals(obj);
        //}

        public override bool CanConvert(Type typeToConvert) =>
            typeof(ChatCompletionRoleRequestMessage).IsAssignableFrom(typeToConvert);

        public override ChatCompletionRoleRequestMessage? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            ChatCompletionRoleRequestMessage? ret = null;
            Utf8JsonReader originalReader = reader;
            Utf8JsonReader specializedReader = reader;
            try
            {
                Utf8JsonReader readerClone = reader;
                var role = FindRole(ref readerClone, typeToConvert, options);
                if (role != null)
                {
                    switch (role)
                    {
                        case ChatCompletionMessageRoleEnum.Assistant:
                            {
                                ret = JsonSerializer.Deserialize<ChatCompletionAssistantMessage>(ref specializedReader);
                            }
                            break;
                        case ChatCompletionMessageRoleEnum.Function:
                            {
                                ret = JsonSerializer.Deserialize<ChatCompletionFunctionMessage>(ref specializedReader);
                                
                            }
                            break;
                        case ChatCompletionMessageRoleEnum.System:
                            {
                                ret = JsonSerializer.Deserialize<ChatCompletionSystemMessage>(ref specializedReader);
                            }
                            break;
                        case ChatCompletionMessageRoleEnum.Tool:
                            {
                                ret = JsonSerializer.Deserialize<ChatCompletionToolMessage>(ref specializedReader);
                            }
                            break;
                        case ChatCompletionMessageRoleEnum.User:
                            {
                                ret = DeserializeUserRequestMessage(ref specializedReader, typeToConvert, options);
                            }
                            break;
                        default:
                            {
                                throw new JsonException();
                            }
                    }
                    return ret;
                }
                throw new JsonException();
            }
            finally
            {
                reader = originalReader;
            }
        }

        private ChatCompletionMessageRoleEnum? FindRole(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            ChatCompletionMessageRoleEnum? ret = null;
            Utf8JsonReader originalReader = reader;
            Utf8JsonReader cloneReader = reader;

            try
            {
                if (reader.TokenType != JsonTokenType.StartObject)
                {
                    throw new JsonException();
                }

                while (cloneReader.Read())
                {
                    if (cloneReader.TokenType == JsonTokenType.EndObject)
                    {
                        return ret;
                    }

                    if (cloneReader.TokenType == JsonTokenType.PropertyName)
                    {
                        string? propertyName = cloneReader.GetString();
                        if (propertyName == nameof(ChatCompletionRoleRequestMessage.Role).ToLower())
                        {
                            if (cloneReader.Read())
                            {
                                if (cloneReader.TokenType == JsonTokenType.String)
                                {
                                    var stringValue = cloneReader.GetString();
                                    if (!String.IsNullOrEmpty(stringValue))
                                    {
                                        ret = (ChatCompletionMessageRoleEnum)Enum.Parse(typeof(ChatCompletionMessageRoleEnum), stringValue,true);
                                        return ret;
                                    }
                                }
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

        private ChatCompletionRoleRequestMessage? DeserializeUserRequestMessage(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Utf8JsonReader originalReader = reader;
            Utf8JsonReader cloneReader = reader;
            Utf8JsonReader specializedReader = reader;
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
                        ret = JsonSerializer.Deserialize<ChatCompletionUserContentMessage>(ref specializedReader);
                        return ret;
                    }

                    if (cloneReader.TokenType == JsonTokenType.PropertyName)
                    {
                        string? propertyName = cloneReader.GetString();
                        if (propertyName == nameof(ChatCompletionUserContentMessage.Content).ToLower())
                        {
                            if (cloneReader.TokenType == JsonTokenType.String)
                            {
                                ret = JsonSerializer.Deserialize<ChatCompletionUserContentMessage>(ref specializedReader);
                            }
                            else
                            {
                                ret = JsonSerializer.Deserialize<ChatCompletionUserContentsMessage>(ref specializedReader);
                            }
                            break;
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
            switch (value)
            {
                case ChatCompletionAssistantMessage t:
                    {
                        JsonSerializer.Serialize(writer, t, options);
                    }
                    break;
                case ChatCompletionFunctionMessage t:
                    {
                        JsonSerializer.Serialize(writer, t, options);
                    }
                    break;
                case ChatCompletionSystemMessage t:
                    {
                        JsonSerializer.Serialize(writer, t, options);
                    }
                    break;
                case ChatCompletionToolMessage t:
                    {
                        JsonSerializer.Serialize(writer, t, options);
                    }
                    break;
                case ChatCompletionUserContentMessage t:
                    {
                        JsonSerializer.Serialize(writer, t, options);
                    }
                    break;
                case ChatCompletionUserContentsMessage t:
                    {
                        JsonSerializer.Serialize(writer, t, options);
                    }
                    break;
            }
        }
    }

}
