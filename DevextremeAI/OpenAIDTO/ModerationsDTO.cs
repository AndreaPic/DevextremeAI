using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DevExtremeAI.OpenAIDTO
{
    public class CreateModerationsRequest
    {
        /// <summary>
        /// The input text to classify
        /// </summary>
        [JsonPropertyName("input")]
        public string Input { get; set; }

        /// <summary>
        /// Two content moderations models are available: text-moderation-stable and text-moderation-latest.
        /// The default is text-moderation-latest which will be automatically upgraded over time.
        /// This ensures you are always using our most accurate model.
        /// If you use text-moderation-stable, we will provide advanced notice before updating the model.
        /// Accuracy of text-moderation-stable may be slightly lower than for text-moderation-latest.
        /// </summary>
        [JsonPropertyName("model")]
        public string? Model { get; set; }
    }
    public class ModerationsResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("model")]
        public string Model { get; set; }
        [JsonPropertyName("results")]
        public List<ModerationCategories> Results { get; set; }
    }
    public class ModerationCategories
    {
        [JsonPropertyName("categories")]
        public Categories Categories { get; set; }

        [JsonPropertyName("category_scores")]
        public CategoryScores CategoryScores { get; set; }

        [JsonPropertyName("flagged")]
        public bool? Flagged { get; set; }
    }
    public class Categories
    {
        [JsonPropertyName("hate")]
        public bool? Hate { get; set; }
        [JsonPropertyName("hate/threatening")]
        public bool? HateThreatening { get; set; }
        [JsonPropertyName("self-harm")]
        public bool? SelfHarm { get; set; }
        [JsonPropertyName("sexual")]
        public bool? Sexual { get; set; }

        [JsonPropertyName("sexual/minors")]
        public bool? SexualMinors { get; set; }

        [JsonPropertyName("violence")]
        public bool? Violence { get; set; }

        [JsonPropertyName("violence/graphic")]
        public bool? ViolenceGraphic { get; set; }
    }

    public class CategoryScores
    {
        [JsonPropertyName("hate")]
        public double? Hate { get; set; }

        [JsonPropertyName("hate/threatening")]
        public double? HateThreatening { get; set; }

        [JsonPropertyName("self-harm")]
        public double? SelfHarm { get; set;}

        [JsonPropertyName("sexual")]
        public double? Sexual { get; set;}

        [JsonPropertyName("sexual/minors")]
        public double? SexualMinors { get; set; }

        [JsonPropertyName("violence")]
        public double? Violence { get; set; }

        [JsonPropertyName("violence/graphic")]
        public double? ViolenceGraphic { get; set; }
    }
}
