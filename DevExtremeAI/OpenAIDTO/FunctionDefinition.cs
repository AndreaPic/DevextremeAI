using DevExtremeAI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DevExtremeAI.OpenAIDTO
{

    //https://github.com/openai/openai-cookbook/blob/main/examples/How_to_call_functions_with_chat_models.ipynb

    public class FunctionDefinition
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("parameters")]
        public ParametersDefinition Parameters {get;set;} = new ();

    }

    public class ParametersDefinition
    {
        [JsonPropertyName("type")]
        public string TypeName { get; set; } = "object";

        [JsonPropertyName("properties")]
        public IDictionary<string, PropertyDefinition> Properties { get; set; } = new Dictionary<string, PropertyDefinition>();

        [JsonPropertyName("required")]
        public IList<string>? Required { get; set; }

        public void AddRequiredProperty(string propertyName)
        {
            if (Required == null) 
            { 
                Required = new List<string>(); 
            }
            Required.Add(propertyName);
        }
    }

    public class PropertyDefinition
    {
        [JsonPropertyName("type")]
        public string TypeName { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("enum")]
        public IList<string>? EnumValues { get; set; }

        public void AddEnumValue(string enumValue)
        {
            if (EnumValues == null)
            {
                EnumValues = new List<string>();
            }
            EnumValues.Add(enumValue);
        }
    }

    public class FunctionCallDefinition
    {
        [JsonPropertyName("name")]
        public string FunctionName { get; set; }

        [JsonPropertyName("arguments")]
        [JsonConverter(typeof(JsonStringArgumentsDictionaryConverter))]
        public IDictionary<string, object> Arguments { get; set; } = new Dictionary<string, object>();
    }
}
