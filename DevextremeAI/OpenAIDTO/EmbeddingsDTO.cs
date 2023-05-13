using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DevExtremeAI.OpenAIDTO
{
    public class CreateEmbeddingsRequest
    {
        /// <summary>
        /// ID of the model to use. You can use the List models API to see all of your available models, or see our Model overview for descriptions of them.
        /// </summary>
        [JsonPropertyName("model")]
        public string ModelID { get; set; }

        /// <summary>
        /// Input text to get embeddings for, encoded as a string or array of tokens.
        /// To get embeddings for multiple inputs in a single request, pass an array of strings or array of token arrays.
        /// Each input must not exceed 8192 tokens in length.
        /// </summary>
        [JsonPropertyName("input")]
        public object? Input => Inputs.Count switch { 0 => null, 1 => Inputs[0], > 1 => Inputs, _ => null };
        private List<string> Inputs { get; set; } = new List<string>();
        public void AddInput(string input)
        {
            Inputs.Add(input);
        }

        /// <summary>
        /// A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse
        /// </summary>
        [JsonPropertyName("user")]
        public string? User { get; set; }
    }

    public class CreateEmbeddingResponse
    {
        [JsonPropertyName("object")]
        public string? Object { get; set; }

        [JsonPropertyName("model")]
        public string? ModelID { get; set; }

        [JsonPropertyName("data")]
        public List<CreateEmbeddingResponseDataInner>? Data { get; set; }

        [JsonPropertyName("usage")]
        public CreateEmbeddingResponseUsage? Usage { get; set; }
    }

    public class CreateEmbeddingResponseDataInner
    {
        [JsonPropertyName("index")]
        public double? Index { get; set; }

        [JsonPropertyName("object")]
        public string? Object { get; set; }

        [JsonPropertyName("embedding")]
        public List<double>? Embedding { get; set; }
    }

    public class CreateEmbeddingResponseUsage
    {
        [JsonPropertyName("prompt_tokens")]
        public double? PromptTokens { get; set; }

        [JsonPropertyName("total_tokens")]
        public double? TotalTokens { get; set; }
    }

}
