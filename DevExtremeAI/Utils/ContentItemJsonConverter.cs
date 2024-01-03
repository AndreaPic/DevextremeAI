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
    public class ContentItemJsonConverter : JsonConverter<ContentItem>
    {

        //TODO: https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/converters-how-to?source=recommendations&pivots=dotnet-8-0#support-polymorphic-deserialization

        public override bool CanConvert(Type typeToConvert) =>
            typeof(ContentItem).IsAssignableFrom(typeToConvert);

        public override ContentItem? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            ContentItem? ret = null;
            Utf8JsonReader originalReader = reader;
            Utf8JsonReader specializedReader = reader;
            try
            {
                Utf8JsonReader readerClone = reader;
                var itemType = FindType(ref readerClone, typeToConvert, options);
                if (itemType != null)
                {
                    switch (itemType)
                    {
                        case ContentItemTypes.Text:
                            {
                                ret = JsonSerializer.Deserialize<ContentTextItem>(ref reader);
                            }
                            break;
                        case ContentItemTypes.ImageUrl:
                            {
                                ret = JsonSerializer.Deserialize<ContentImageItem>(ref reader);
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
                //reader = originalReader;
            }
        }

        private ContentItemTypes? FindType(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            ContentItemTypes? ret = null;
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

                    if (cloneReader.TokenType == JsonTokenType.StartObject)
                    {
                        cloneReader.Skip();
                    }

                    if (cloneReader.TokenType == JsonTokenType.PropertyName)
                    {
                        string? propertyName = cloneReader.GetString();
                        if (propertyName == "type")
                        {
                            if (cloneReader.Read())
                            {
                                if (cloneReader.TokenType == JsonTokenType.String)
                                {
                                    var stringValue = cloneReader.GetString();
                                    if (!String.IsNullOrEmpty(stringValue))
                                    {
                                        switch (stringValue)
                                        {
                                            case "text":
                                                return ContentItemTypes.Text;
                                                break;
                                            case "image_url":
                                                return ContentItemTypes.ImageUrl; 
                                                break;
                                        }
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
        public override void Write(Utf8JsonWriter writer, ContentItem value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case ContentTextItem t:
                    {
                        JsonSerializer.Serialize(writer, t, options);
                    }
                    break;
                case ContentImageItem t:
                    {
                        JsonSerializer.Serialize(writer, t, options);
                    }
                    break;
            }
        }
    }

}
