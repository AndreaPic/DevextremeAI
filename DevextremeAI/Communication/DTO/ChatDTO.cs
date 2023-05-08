using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using DevextremeAI.Utils;

namespace DevextremeAI.Communication.DTO
{
    public class CreateChatCompletionRequest
    {
        /// <summary>
        /// ID of the model to use. See the model endpoint compatibility table for details on which models work with the Chat API.
        /// ID of the model to use. Currently, only `gpt-3.5-turbo` and `gpt-3.5-turbo-0301` are supported.
        /// </summary>
        [JsonPropertyName("model")]
        public string ModelID { get; set; }

        /// <summary>
        /// A list of messages describing the conversation so far.
        /// The messages to generate chat completions for, in the [chat format](/docs/guides/chat/introduction).
        /// </summary>
        [JsonPropertyName("messages")]
        public List<ChatCompletionRequestMessage> Messages { get; private set; } = new List<ChatCompletionRequestMessage>();

        /// <summary>
        /// What sampling temperature to use, between 0 and 2.
        /// Higher values like 0.8 will make the output more random, while lower values like 0.2 will make it more focused and deterministic.  We generally recommend altering this or `top_p` but not both.
        /// We generally recommend altering this or top_p but not both.
        /// </summary>
        [JsonPropertyName("temperature")]
        public double? Temperature { get; set; }

        /// <summary>
        /// An alternative to sampling with temperature, called nucleus sampling, where the model considers the results of the tokens with top_p probability mass.
        /// So 0.1 means only the tokens comprising the top 10% probability mass are considered.
        /// We generally recommend altering this or `temperature` but not both.
        /// </summary>
        [JsonPropertyName("top_p")]
        public float? TopP { get; set; }

        /// <summary>
        /// How many chat completion choices to generate for each input message.
        /// </summary>
        [JsonPropertyName("n")]
        public int? N { get; set; }

        /// <summary>
        /// If set, partial message deltas will be sent, like in ChatGPT. Tokens will be sent as data-only [server-sent events](https://developer.mozilla.org/en-US/docs/Web/API/Server-sent_events/Using_server-sent_events#Event_stream_format) as they become available, with the stream terminated by a `data: [DONE]` message. 
        /// </summary>
        [JsonPropertyName("stream")]
        public bool? Stream { get; set; }

        /// <summary>
        /// Up to 4 sequences where the API will stop generating further tokens.
        /// </summary>
        [JsonPropertyName("stop")]
        public object? Stop => stops.Count switch { 0 => null, 1 => stops[0], > 1 => stops, _ => null };

        private List<string> stops { get; set; } = new List<string>();

        public void AddStop(string stop)
        {
            stops.Add(stop);
        }

        /// <summary>
        /// The maximum number of tokens allowed for the generated answer.
        /// By default, the number of tokens the model can return will be (4096 - prompt tokens).
        /// The maximum number of tokens to generate in the chat completion.
        /// The total length of input tokens and generated tokens is limited by the model's context length.
        /// </summary>
        [JsonPropertyName("max_tokens")]
        public int? MaxTokens { get; set; }

        /// <summary>
        /// Number between -2.0 and 2.0.
        /// Positive values penalize new tokens based on whether they appear in the text so far, increasing the model\'s likelihood to talk about new topics.  [See more information about frequency and presence penalties.](/docs/api-reference/parameter-details)
        /// </summary>
        [JsonPropertyName("presence_penalty")]
        public float? PresencePenalty { get; set; }

        /// <summary>
        /// Number between -2.0 and 2.0. Positive values penalize new tokens based on their existing frequency in the text so far, decreasing the model\'s likelihood to repeat the same line verbatim.  [See more information about frequency and presence penalties.](/docs/api-reference/parameter-details)
        /// </summary>
        [JsonPropertyName("frequency_penalty")]
        public float? FrequencyPenalty { get; set; }

        /// <summary>
        /// Modify the likelihood of specified tokens appearing in the completion.
        /// Accepts a json object that maps tokens (specified by their token ID in the GPT tokenizer) to an associated bias value from -100 to 100.
        /// You can use this [tokenizer tool](/tokenizer?view=bpe) (which works for both GPT-2 and GPT-3) to convert text to token IDs.
        /// Mathematically, the bias is added to the logits generated by the model prior to sampling.
        /// The exact effect will vary per model, but values between -1 and 1 should decrease or increase likelihood of selection;
        /// values like -100 or 100 should result in a ban or exclusive selection of the relevant token.
        /// As an example, you can pass `{"50256": -100}` to prevent the <|endoftext|> token from being generated. 
        /// </summary>
        [JsonPropertyName("logit_bias")]
        public object? LogitBias { get; set; }

        /// <summary>
        /// A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.
        /// [Learn more](/docs/guides/safety-best-practices/end-user-ids).
        /// </summary>
        [JsonPropertyName("user")]
        public string? User { get; set; }

    }

    public class ChatCompletionRequestMessage
    {
        /// <summary>
        /// The role of the author of this message. One of system, user, or assistant.
        /// </summary>
        [JsonPropertyName("role")]

        public ChatCompletionRequestMessageRoleEnum Role { get; set; }

        /// <summary>
        /// The contents of the message.
        /// </summary>
        [JsonPropertyName("content")]
        public string Content { get; set; }

        /// <summary>
        /// The name of the user in a multi-user chat
        /// The name of the author of this message. May contain a-z, A-Z, 0-9, and underscores, with a maximum length of 64 characters.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

    }

    [JsonConverter(typeof(JsonStringEnumConverterEx<ChatCompletionRequestMessageRoleEnum>))]
    public enum ChatCompletionRequestMessageRoleEnum
    {
        [EnumMember(Value = "system")]
        System = 0,
        [EnumMember(Value = "user")]
        User = 1,
        [EnumMember(Value = "assistant")]
        Assistant = 2
    }


    public class CreateChatCompletionResponse
    {
        [JsonPropertyName("id")]
        public string ID { get; set; }

        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("created")]
        public double Created { get; set; }

        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("choices")]
        public List<CreateChatCompletionResponseChoicesInner> Choices { get; set; }

        [JsonPropertyName("usage")]
        public CreateCompletionResponseUsage? Usage { get; set; }
    }

    public class CreateChatCompletionResponseChoicesInner
    {

        [JsonPropertyName("index")]
        public double? Index { get; set; }

        public ChatCompletionResponseMessage? Message { get; set; }


        [JsonPropertyName("finish_reason")]
        public string? FinishReason { get; set; }
    }

    public class ChatCompletionResponseMessage
    {
        /// <summary>
        /// The role of the author of this message.
        /// </summary>
        [JsonPropertyName("role")]
        public ChatCompletionResponseMessageRoleEnum Role { get; set; }

        /// <summary>
        /// The contents of the message
        /// </summary>
        [JsonPropertyName("content")]
        public string Content { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverterEx<ChatCompletionResponseMessageRoleEnum>))]

    public enum ChatCompletionResponseMessageRoleEnum
    {
        [EnumMember(Value = "system")]
        System = 0,
        [EnumMember(Value = "user")]
        User = 1,
        [EnumMember(Value = "assistant")]
        Assistant = 2
    }

}

