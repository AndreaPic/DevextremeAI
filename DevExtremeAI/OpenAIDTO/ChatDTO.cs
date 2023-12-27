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
using DevExtremeAI.Utils;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Options;
using System.IO;
using System.Reflection;
#if NET7_0_OR_GREATER
using static System.Runtime.InteropServices.JavaScript.JSType;
#endif
using System.Threading.Channels;
using System.Net.Mime;

namespace DevExtremeAI.OpenAIDTO
{
    public class CreateChatCompletionRequest
    {
        /// <summary>
        /// ID of the model to use. See the model endpoint compatibility table for details on which models work with the Chat API.
        /// ID of the model to use. Currently, only `gpt-3.5-turbo` and `gpt-3.5-turbo-0301` are supported.
        /// </summary>
        [JsonPropertyName("model")]
        public string Model { get; set; }

        /// <summary>
        /// A list of messages describing the conversation so far.
        /// The messages to generate chat completions for, in the [chat format](/docs/guides/chat/introduction).
        /// </summary>
        [JsonPropertyName("messages")]
        public List<ChatCompletionRoleRequestMessage> Messages { get; private set; } = new List<ChatCompletionRoleRequestMessage>();
        //TODO: manage hierarchy ^^^
        //TODO: add specialized AddMessage


        /// <summary>
        /// Add a message to the completion
        /// </summary>
        /// <param name="message">The message to add</param>
        public void AddMessage(ChatCompletionRequestMessage message)
        {
            Messages.Add(message);
        }

        /// <summary>
        /// Function definitions for ai
        /// </summary>
        [Obsolete("use tool_choice instead")]
        private IList<FunctionDefinition>? functions;

        /// <summary>
        /// A list of functions the model may generate JSON inputs for.
        /// </summary>
        [Obsolete("use tool_choice instead")]
        [JsonPropertyName("functions")]
        public IList<FunctionDefinition>? Functions 
        { 
            get
            {
                if ( (functions == null) || (functions.Count == 0))
                {
                    return null;
                }
                return functions;
            }
            set
            {
                functions = value;
            }
        }

        /// <summary>
        /// Add a function definition to the completion
        /// </summary>
        /// <param name="functionDefinition">The funciton definition</param>
        [Obsolete("use tool_choice instead")]
        public void AddFunction(FunctionDefinition functionDefinition)
        {
            if (functions == null)
            {
                functions = new List<FunctionDefinition>();
            }
            functions.Add(functionDefinition);
        }


        /// <summary>
        /// Controls how the model responds to function calls. 
        /// "none" means the model does not call a function, and responds to the end-user. 
        /// "auto" means the model can pick between an end-user or calling a function. 
        /// Specifying a particular function via {"name":\ "my_function"} 
        /// forces the model to call that function. 
        /// "none" is the default when no functions are present. 
        /// "auto" is the default if functions are present
        /// </summary>
        [Obsolete("use tool_choice instead")]
        [JsonPropertyName("function_call")]
        public string? FunctionCall { get; set; }

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

        /// <summary>
        /// stop list
        /// </summary>
        private List<string> stops { get; set; } = new List<string>();

        /// <summary>
        /// Add a sequence where the API will stop generating further tokens.
        /// </summary>
        /// <param name="stop">sequence where the API will stop generating further tokens</param>
        /// <remarks>
        /// Are allowed up to 4 sequence.
        /// </remarks>
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

        /// <summary>
        /// Whether to return log probabilities of the output tokens or not. 
        /// If true, returns the log probabilities of each output token returned in the content of message. 
        /// This option is currently not available on the gpt-4-vision-preview model.
        /// </summary>
        /// <remarks>Default False</remarks>
        [JsonPropertyName("logprobs")]
        public bool? LogProbs { get; set; }

        /// <summary>
        /// An integer between 0 and 5 specifying the number of most likely tokens to return at each token position, 
        /// each with an associated log probability. 
        /// logprobs must be set to true if this parameter is used.
        /// </summary>
        [JsonPropertyName("top_logprobs")]
        public int? TopLogProbs { get; set; }


        /// <summary>
        /// An object specifying the format that the model must output.Compatible with gpt-4-1106-preview and gpt-3.5-turbo-1106.
        /// Setting to { "type": "json_object" }
        /// enables JSON mode, which guarantees the message the model generates is valid JSON.
        /// Important: when using JSON mode, you must also instruct the model to produce JSON yourself via a system or user message.Without this, the model may generate an unending stream of whitespace until the generation reaches the token limit, resulting in a long-running and seemingly "stuck" request.Also note that the message content may be partially cut off if finish_reason= "length", which indicates the generation exceeded max_tokens or the conversation exceeded the max context length.
        /// </summary>
        [JsonPropertyName("response_format")]
        public ResponseOutputFormat? ResponseFormat { get; set; }

        /// <summary>
        /// This feature is in Beta. 
        /// If specified, our system will make a best effort to sample deterministically, 
        /// such that repeated requests with the same seed and parameters should return the same result. 
        /// Determinism is not guaranteed, and you should refer to the system_fingerprint response parameter to monitor changes in the backend.
        /// </summary>
        [JsonPropertyName("response_format")]
        public int? Seed {  get; set; }

        /// <summary>
        /// A list of tools the model may call. 
        /// Currently, only functions are supported as a tool. 
        /// Use this to provide a list of functions the model may generate JSON inputs for.
        /// </summary>
        [JsonPropertyName("tools")]
        public List<ToolDefinition> Tools { get; set; }
        //TODO: manage hierarchy ^^^

        /// <summary>
        /// Ad a tool to Tools List
        /// </summary>
        /// <param name="tool">Tool To Add</param>
        public void AddTool(ToolDefinition tool)
        {
            if (Tools == null)
            {
                Tools = new List<ToolDefinition>();
            }
            Tools.Add(tool);
        }


        /// <summary>
        /// Controls which (if any) function is called by the model. none means the model will not call a function and instead generates a message. auto means the model can pick between generating a message or calling a function. Specifying a particular function via {"type": "function", "function": {"name": "my_function"}} forces the model to call that function.
        /// none is the default when no functions are present.auto is the default if functions are present.
        /// </summary>
        [JsonPropertyName("tool_choice")]
        public object? ToolChoice { get; private set;}

        /// <summary>
        /// auto is the default if functions are present.
        /// </summary>
        public void SetAutoToolChoice()
        {
            ToolChoice = "auto";
        }
        /// <summary>
        /// none is the default when no functions are present.
        /// </summary>
        public void SetNoneToolChoice()
        {
            ToolChoice = "none";
        }

        public void SetFunctionNameToolChoice(FunctionNameTool functionNameTool)
        {
            ToolChoice = functionNameTool;
        }


    }

    //TODO: continue form here https://platform.openai.com/docs/api-reference/chat/streaming

    public abstract class ToolDefinition
    {
        /// <summary>
        /// The type of the tool. Currently, only function is supported.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; protected set; }
    }

    public class FunctionTool : ToolDefinition
    {
        public FunctionTool()
        {
            Type = "function";
        }

        [JsonPropertyName("function")]
        public FunctionDefinition Function { get; set; }
    }

    public class FunctionNameTool : ToolDefinition
    {
        public FunctionNameTool()
        {
            Type = "function";
        }

        [JsonPropertyName("function")]
        public FunctionNameDefinition Function { get; set; }
    }


    public abstract class ResponseOutputFormat
    {
        [JsonPropertyName("type")]
        public string Type { get; protected set; }
    }

    public class ResponseOutputTextFormat : ResponseOutputFormat
    {
        public ResponseOutputTextFormat()
        {
            Type = "text";
        }
    }

    public class ResponseOutputJSONFormat : ResponseOutputFormat
    {
        public ResponseOutputJSONFormat()
        {
            Type = "json_object";
        }
    }


    public class ChatCompletionRequestMessage : ChatCompletionStringRequestMessage
    {
        /// <summary>
        /// The role of the author of this message. One of system, user, or assistant.
        /// </summary>
        [JsonPropertyName("role")]
        new public ChatCompletionMessageRoleEnum Role 
        { 
            get
            {
                return base.Role;
            }
            set
            {
                base.Role = value;
            }
        }

        /// <summary>
        /// The name and arguments of a function that should be called, as generated by the model.
        /// </summary>
        [Obsolete("Instead use ToolCalls")]
        [JsonPropertyName("function_call")]
        public FunctionCallDefinition? FunctionCall { get; set; }

    }

    //TODO: review as https://platform.openai.com/docs/api-reference/chat/create

    public abstract class ChatCompletionRoleRequestMessage
    {
        /// <summary>
        /// The role of the author of this message. One of system, user, or assistant.
        /// </summary>
        [JsonPropertyName("role")]
        public ChatCompletionMessageRoleEnum Role { get; internal protected set; }
    }

    public abstract class ChatCompletionNameRequestMessage : ChatCompletionRoleRequestMessage
    {
        /// </summary>
        /// The name of the user in a multi-user chat
        /// The name of the author of this message. May contain a-z, A-Z, 0-9, and underscores, with a maximum length of 64 characters.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }
    public abstract class ChatCompletionStringRequestMessage : ChatCompletionNameRequestMessage
    {
        /// <summary>
        /// The contents of the message.
        /// </summary>
        [JsonPropertyName("content")]
        public string Content { get; set; }
    }

    /// <summary>
    /// System Message
    /// </summary>
    public class ChatCompletionSystemMessage : ChatCompletionStringRequestMessage
    {

        public ChatCompletionSystemMessage()
        {
            Role = ChatCompletionMessageRoleEnum.System;
        }
    }

    /// <summary>
    /// User Message with text contents of the message
    /// </summary>
    public class ChatCompletionUserContentMessage : ChatCompletionStringRequestMessage
    {
        public ChatCompletionUserContentMessage()
        {
            Role = ChatCompletionMessageRoleEnum.User;
        }
    }

    /// <summary>
    /// User Message with Array of content parts
    /// </summary>
    public class ChatCompletionUserContentsMessage : ChatCompletionNameRequestMessage
    {
        public ChatCompletionUserContentsMessage()
        {
            Role = ChatCompletionMessageRoleEnum.User;
        }

        /// <summary>
        /// An array of content parts with a defined type, each can be of type text or image_url when passing in images. 
        /// You can pass multiple images by adding multiple image_url content parts. 
        /// Image input is only supported when using the gpt-4-visual-preview model.
        /// </summary>
        [JsonPropertyName("content")]
        public List<ContentItem> Contents { get; private set; } = new List<ContentItem>();
    }

    /// <summary>
    /// Assistant message
    /// </summary>
    public class ChatCompletionAssistantMessage : ChatCompletionStringRequestMessage
    {
        public ChatCompletionAssistantMessage()
        {
            Role = ChatCompletionMessageRoleEnum.Assistant;
        }

        /// <summary>
        /// The tool calls generated by the model, such as function calls.
        /// </summary>
        [JsonPropertyName("tool_calls")]        
        public List<ToolCall> ToolCalls { get; private set; } = new List<ToolCall>();
    }

    /// <summary>
    /// Tool message
    /// </summary>
    public class ChatCompletionToolMessage : ChatCompletionRoleRequestMessage
    {
        public ChatCompletionToolMessage()
        {
            Role = ChatCompletionMessageRoleEnum.Tool;
        }

        /// <summary>
        /// The contents of the tool message.
        /// </summary>
        [JsonPropertyName("content")]
        public string Content { get; set; }

        /// <summary>
        /// Tool call that this message is responding to.
        /// </summary>
        [JsonPropertyName("tool_call_id")]
        public string ToolCallId { get; set; }
    }


    public class ToolCall
    {
        /// <summary>
        /// The ID of the tool call.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// The type of the tool. Currently, only function is supported.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get => "function"; /*set;*/ }

        /// <summary>
        /// The function that the model called.
        /// </summary>
        [JsonPropertyName("function")]
        public FunctionDefinition FunctionCall { get; set; }
    }


    /// <summary>
    /// Base class for content items used in Contents of ChatCompletionUserContentsMessage
    /// </summary>
    public abstract class ContentItem
    {
        /// <summary>
        /// The type of the content part.
        /// </summary>
        [JsonPropertyName("type")]
        public ContentItemTypes ContentType { get; internal protected set; }
    }

    public class ContentTextItem : ContentItem
    {
        public ContentTextItem()
        {
            ContentType = ContentItemTypes.Text;
        }

        /// <summary>
        /// The text content.
        /// </summary>
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }

    public class ContentImageItem : ContentItem
    {
        public ContentImageItem()
        {
            ContentType = ContentItemTypes.ImageUrl;
        }

        [JsonPropertyName("image_url")]
        public ImageUrl ImageUrl { get; private set; } = new ImageUrl();
    }

    public class ImageUrl
    {
        /// <summary>
        /// Either a URL of the image or the base64 encoded image data.
        /// </summary>
        [JsonPropertyName("url")]
        public string ImageURl { get; set; }

        /// <summary>
        /// Specifies the detail level of the image.
        /// </summary>
        [JsonPropertyName("detail")]
        public ImageDetailLevel? Detail { get; set; }
    }


    [JsonConverter(typeof(JsonStringEnumConverterEx<ContentItemTypes>))]
    public enum ContentItemTypes
    {
        [EnumMember(Value = "Text")]
        Text = 0,
        [EnumMember(Value = "image_url")]
        ImageUrl = 1,
    }

    [JsonConverter(typeof(JsonStringEnumConverterEx<ImageDetailLevel>))]
    public enum ImageDetailLevel
    {
        [EnumMember(Value = "auto")]
        Auto = 0,
        [EnumMember(Value = "low")]
        Low = 1,
        [EnumMember(Value = "high")]
        High = 2,
    }


    public class ChatCompletionfunction
    {
        /// <summary>
        /// The name of the function to be called. Must be a-z, A-Z, 0-9, or contain underscores and dashes, with a maximum length of 64.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// A description of what the function does, used by the model to choose when and how to call the function.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// The parameters the functions accepts, described as a JSON Schema object. 
        /// See the guide (https://platform.openai.com/docs/guides/gpt/function-calling) for examples, 
        /// and the JSON Schema reference (https://json-schema.org/understanding-json-schema/) for documentation about the format.
        /// To describe a function that accepts no parameters, provide the value {"type": "object", "properties": { } }.
        /// </summary>
        [JsonPropertyName("parameters")]
        public string JSONSchemaParameters { get; set; }

    }

    [JsonConverter(typeof(JsonStringEnumConverterEx<ChatCompletionMessageRoleEnum>))]
    public enum ChatCompletionMessageRoleEnum
    {
        [EnumMember(Value = "system")]
        System = 0,
        [EnumMember(Value = "user")]
        User = 1,
        [EnumMember(Value = "assistant")]
        Assistant = 2,
        [EnumMember(Value = "function")]
        Function = 3,
        [EnumMember(Value = "tool")]
        Tool = 4
    }

    /// <summary>
    /// Represents a chat completion response returned by model, based on the provided input.
    /// </summary>
    public class CreateChatCompletionResponse
    {
        /// <summary>
        /// A unique identifier for the chat completion.
        /// </summary>
        [JsonPropertyName("id")]
        public string ID { get; set; }

        /// <summary>
        /// The object type, which is always chat.completion.
        /// </summary>
        [JsonPropertyName("object")]
        public string Object { get; set; }

        /// <summary>
        /// The Unix timestamp (in seconds) of when the chat completion was created.
        /// </summary>
        [JsonPropertyName("created")]
        public double Created { get; set; }

        /// <summary>
        /// The model used for the chat completion.
        /// </summary>
        [JsonPropertyName("model")]
        public string Model { get; set; }

        /// <summary>
        /// A list of chat completion choices. Can be more than one if n is greater than 1.
        /// </summary>
        [JsonPropertyName("choices")]
        public List<CreateChatCompletionResponseChoicesInner> Choices { get; set; }

        /// <summary>
        /// Usage statistics for the completion request.
        /// </summary>
        [JsonPropertyName("usage")]
        public CreateCompletionResponseUsage? Usage { get; set; }

        /// <summary>
        /// This fingerprint represents the backend configuration that the model runs with.
        /// Can be used in conjunction with the seed request parameter to understand when backend changes have been made that might impact determinism.
        /// </summary>
        [JsonPropertyName("system_fingerprint")]
        public string SystemFingerprint { get; set; }
    }

    public class CreateChatCompletionResponseChoicesInner
    {
        /// <summary>
        /// The index of the choice in the list of choices.
        /// </summary>
        [JsonPropertyName("index")]
        public double? Index { get; set; }

        /// <summary>
        /// A chat completion message generated by the model.
        /// </summary>
        [JsonPropertyName("message")]
        public ChatCompletionResponseMessage? Message { get; set; }

        [JsonPropertyName("delta")]
        public ChatCompletionResponseMessage? Delta { get; set; }

        /// <summary>
        /// The reason the model stopped generating tokens. This will be stop if the model hit a natural stop point or a provided stop sequence, length if the maximum number of tokens specified in the request was reached, content_filter if content was omitted due to a flag from our content filters, tool_calls if the model called a tool, or function_call (deprecated) if the model called a function.
        /// </summary>
        [JsonPropertyName("finish_reason")]
        public string? FinishReason { get; set; }
    }

    public class ChatCompletionResponseMessage
    {
        /// <summary>
        /// The role of the author of this message.
        /// </summary>
        [JsonPropertyName("role")]
        public ChatCompletionMessageRoleEnum Role { get; set; }

        /// <summary>
        /// The contents of the message
        /// </summary>
        [JsonPropertyName("content")]
        public string Content { get; set; }

        /// <summary>
        /// The function to call
        /// </summary>
        [JsonPropertyName("function_call")]
        public FunctionCallDefinition? FunctionCall { get; set; }
    }

}

