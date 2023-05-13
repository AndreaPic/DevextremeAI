using DevExtremeAI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DevExtremeAI.OpenAIDTO
{
    public class CreateTranscriptionsRequest
    {
        /// <summary>
        /// The audio file to transcribe, in one of these formats: mp3, mp4, mpeg, mpga, m4a, wav, or webm.
        /// </summary>
        [JsonPropertyName("file")]
        public byte[] File { get; set; }

        /// <summary>
        /// ID of the model to use. Only whisper-1 is currently available.
        /// </summary>
        [JsonPropertyName("model")]
        public string Model { get; set; }

        /// <summary>
        /// An optional text to guide the model's style or continue a previous audio segment. The prompt should match the audio language.
        /// </summary>
        [JsonPropertyName("prompt")]
        public string? Prompt { get; set; }

        /// <summary>
        /// The format of the transcript output, in one of these options: json, text, srt, verbose_json, or vtt.
        /// </summary>
        [JsonPropertyName("response_format")]
        public CreateTranscriptionsRequestEnum? ResponseFormat { get; set; }

        /// <summary>
        /// The sampling temperature, between 0 and 1.
        /// Higher values like 0.8 will make the output more random, while lower values like 0.2 will make it more focused and deterministic.
        /// If set to 0, the model will use log probability to automatically increase the temperature until certain thresholds are hit.
        /// </summary>
        [JsonPropertyName("temperature")]
        public double? Temperature { get; set; }

        /// <summary>
        /// The language of the input audio.
        /// Supplying the input language in ISO-639-1 format will improve accuracy and latency.
        /// </summary>
        [JsonPropertyName("language")]
        public string? Language { get; set; }

    }

    public class CreateTranscriptionsResponse
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }


    [JsonConverter(typeof(JsonStringEnumConverterEx<CreateTranscriptionsRequestEnum>))]
    public enum CreateTranscriptionsRequestEnum
    {
        [EnumMember(Value = "json")]
        Json = 1,
        [EnumMember(Value = "text")]
        Text = 2,
        [EnumMember(Value = "srt")]
        Srt = 3,
        [EnumMember(Value = "verbose_json")]
        VerboseJson = 4,
        [EnumMember(Value = "vtt")]
        Vtt = 5
    }


    public class CreateTranslationsRequest
    {
        /// <summary>
        /// The audio file to translate, in one of these formats: mp3, mp4, mpeg, mpga, m4a, wav, or webm.
        /// </summary>
        [JsonPropertyName("file")]
        public byte[] File { get; set; }

        /// <summary>
        /// ID of the model to use. Only whisper-1 is currently available.
        /// </summary>
        [JsonPropertyName("model")]
        public string Model { get; set; }

        /// <summary>
        /// An optional text to guide the model's style or continue a previous audio segment. The prompt should be in English.
        /// </summary>
        [JsonPropertyName("prompt")]
        public string? Prompt { get; set; }

        /// <summary>
        /// The format of the transcript output, in one of these options: json, text, srt, verbose_json, or vtt.
        /// </summary>
        [JsonPropertyName("response_format")]
        public CreateTranscriptionsRequestEnum? ResponseFormat { get; set; }

        /// <summary>
        /// The sampling temperature, between 0 and 1.
        /// Higher values like 0.8 will make the output more random, while lower values like 0.2 will make it more focused and deterministic.
        /// If set to 0, the model will use log probability to automatically increase the temperature until certain thresholds are hit.
        /// </summary>
        [JsonPropertyName("temperature")]
        public double? Temperature { get; set; }

    }

    public class CreateTranslationsResponse
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }

    }


}
