using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DevextremeAI.Communication
{
    public class CreateEditRequest
    {
        /// <summary>
        /// ID of the model to use.
        /// You can use the `text-davinci-edit-001` or `code-davinci-edit-001` model with this endpoint.
        /// </summary>
        [JsonPropertyName("model")]
        public string ModelID { get; set; }

        /// <summary>
        /// The input text to use as a starting point for the edit.
        /// </summary>
        [JsonPropertyName("input")]
        public string? Input { get; set; }

        /// <summary>
        /// The instruction that tells the model how to edit the prompt.
        /// </summary>
        [JsonPropertyName("instruction")]
        public string Instruction { get; set; }

        /// <summary>
        /// How many edits to generate for the input and instruction.
        /// </summary>
        [JsonPropertyName("n")]
        public int? N { get; set; }

        /// <summary>
        /// What sampling temperature to use, between 0 and 2.
        /// Higher values like 0.8 will make the output more random, while lower values like 0.2 will make it more focused and deterministic.
        /// We generally recommend altering this or `top_p` but not both. 
        /// </summary>
        [JsonPropertyName("temperature")]
        public double? Temperature { get; set; }

        /// <summary>
        /// An alternative to sampling with temperature, called nucleus sampling, where the model considers the results of the tokens with top_p probability mass.
        /// So 0.1 means only the tokens comprising the top 10% probability mass are considered.
        /// We generally recommend altering this or `temperature` but not both. 
        /// </summary>
        [JsonPropertyName("top_p")]
        public double? TopP { get; set; }

    }

    public class CreateEditResponse
    {
        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("created")]
        public double Created { get; set; }

        [JsonPropertyName("choices")]
        public List<CreateCompletionResponseChoicesInner>? Choices { get; set; }

        [JsonPropertyName("usage")]
        public CreateCompletionResponseUsage? Usage { get; set; }
    }
}
