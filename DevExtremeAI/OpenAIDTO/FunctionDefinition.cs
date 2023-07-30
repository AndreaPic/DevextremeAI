using DevExtremeAI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DevExtremeAI.OpenAIDTO
{


    /// <summary>
    /// This object define a schema for the function's description used by OpenAI
    /// </summary>
    public class FunctionDefinition
    {
        /// <summary>
        /// The name of the funciont
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// The description of the function behavior
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// The function parameters
        /// </summary>
        [JsonPropertyName("parameters")]
        public ParametersDefinition Parameters {get;set;} = new ();

    }

    /// <summary>
    /// Definition of the function parameters
    /// </summary>
    public class ParametersDefinition
    {
        /// <summary>
        /// parameters are in dto object so in openai example the value is "object"
        /// </summary>
        [JsonPropertyName("type")]
        public string TypeName { get; set; } = "object";

        /// <summary>
        /// List of the properties used as argument for the function (key is the property name, value is the property definition)
        /// </summary>
        [JsonPropertyName("properties")]
        public IDictionary<string, PropertyDefinition> Properties { get; set; } = new Dictionary<string, PropertyDefinition>();

        /// <summary>
        /// List of the required properties
        /// </summary>
        [JsonPropertyName("required")]
        public IList<string>? Required { get; set; }

        /// <summary>
        /// Add a property to required property list
        /// </summary>
        /// <param name="propertyName"></param>
        public void AddRequiredProperty(string propertyName)
        {
            if (Required == null) 
            { 
                Required = new List<string>(); 
            }
            Required.Add(propertyName);
        }
    }

    /// <summary>
    /// Property definition
    /// </summary>
    public class PropertyDefinition
    {
        /// <summary>
        /// property type (like string,integer)
        /// </summary>
        /// <remarks>
        /// In case of enum you can use string as property type
        /// </remarks>
        [JsonPropertyName("type")]
        public string TypeName { get; set; }

        /// <summary>
        /// property discription
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// If the property is a enum value, here there are valid values
        /// </summary>
        [JsonPropertyName("enum")]
        public IList<string>? EnumValues { get; set; }

        /// <summary>
        /// Add a valid value to EnumValues
        /// </summary>
        /// <param name="enumValue">
        /// A value for the enum list EnumValues
        /// </param>
        public void AddEnumValue(string enumValue)
        {
            if (EnumValues == null)
            {
                EnumValues = new List<string>();
            }
            EnumValues.Add(enumValue);
        }
    }

    /// <summary>
    /// The definition of a function call
    /// </summary>
    public class FunctionCallDefinition
    {
        /// <summary>
        /// Function name
        /// </summary>
        [JsonPropertyName("name")]
        public string FunctionName { get; set; }

        /// <summary>
        /// The arguments of the function, the key is the property name of the object to pass to the function and the value is the value of the property
        /// </summary>
        [JsonPropertyName("arguments")]
        [JsonConverter(typeof(JsonStringArgumentsDictionaryConverter))]
        public IDictionary<string, object> Arguments { get; set; } = new Dictionary<string, object>();
    }
}
