using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using DevextremeAI.Settings;
using Microsoft.AspNetCore.Http;

namespace DevextremeAI.Communication
{

    public interface IOpenAIAPIClient
    {
        public Task<ListModelsResponse?> GetModelsAsync();
        public Task<Model?> GetModelAsync(string modelID);
        public Task<CreateCompletionResponse> CreateCompletionAsync(CreateCompletionRequest request);
        public Task<CreateChatCompletionResponse> CreateChatCompletionAsync(CreateChatCompletionRequest request);
        public Task<CreateEditResponse> CreateEditAsync(CreateEditRequest request);
        public Task<ImagesResponse> CreateImageAsync(CreateImageRequest request);
        public Task<ResponseDTO<ImagesResponse>> CreateImageEditAsync(CreateImageEditRequest request);
        public Task<ResponseDTO<ImagesResponse>> CreateImageVariationsAsync(CreateImageVariationsRequest request);
        public Task<CreateEmbeddingResponse> CreateEmbeddingsAsync(CreateEmbeddingsRequest request);
        public Task<ResponseDTO<CreateTranscriptionsResponse>> CreateTranscriptionsAsync(CreateTranscriptionsRequest request);
        public Task<ResponseDTO<CreateTranslationsResponse>> CreateTranslationsAsync(CreateTranslationsRequest request);


    }
    public sealed class OpenAIAPIClient : IOpenAIAPIClient
    {

        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            //PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            //Converters = { new JsonStringEnumConverter() }
        };

        private const string baseAddress = "https://api.openai.com/v1/";
        
        private IHttpClientFactory HttpClientFactory { get; set; }
        private IAIEnvironment CurrentEnvironment { get; set; }
        public OpenAIAPIClient(IHttpClientFactory httpClientFactory, IAIEnvironment environment)
        {
            HttpClientFactory = httpClientFactory;
            CurrentEnvironment = environment;
        }

        public static OpenAIAPIClient CreateDefault() => new();
        private OpenAIAPIClient()
        {

        }

        private const string modelsPath = "models";


        /// <summary>
        /// List and describe the various models available in the API.
        /// You can refer to the Models documentation to understand what models are available and the differences between them.
        /// Lists the currently available models, and provides basic information about each one such as the owner and availability.
        /// </summary>
        /// <returns></returns>
        public async Task<ListModelsResponse?> GetModelsAsync()
        {
            ListModelsResponse? ret = null;
            HttpClient httpClient = HttpClientFactory.CreateClient();
            FillBaseAddress(httpClient);
            FillAuthRequestHeaders(httpClient.DefaultRequestHeaders);
            var httpResponse = await httpClient.GetAsync(modelsPath);
            if (httpResponse.IsSuccessStatusCode) 
            {
                ret = await httpResponse.Content.ReadFromJsonAsync<ListModelsResponse>();
            }
            return ret;
        }

        /// <summary>
        /// Retrieves a model instance, providing basic information about the model such as the owner and permissioning.
        /// </summary>
        /// <param name="modelID">The ID of the model to use for this request</param>
        /// <returns></returns>
        public async Task<Model?> GetModelAsync(string modelID)
        {
            Model? ret = null;
            HttpClient httpClient = HttpClientFactory.CreateClient();
            FillBaseAddress(httpClient);
            FillAuthRequestHeaders(httpClient.DefaultRequestHeaders);
            var httpResponse = await httpClient.GetAsync($"models/{modelID}");
            if (httpResponse.IsSuccessStatusCode)
            {
                ret = await httpResponse.Content.ReadFromJsonAsync<Model>();
            }
            return ret;
        }

        /// <summary>
        /// Creates a completion for the provided prompt and parameters.
        /// Given a prompt, the model will return one or more predicted completions, and can also return the probabilities of alternative tokens at each position.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CreateCompletionResponse> CreateCompletionAsync(CreateCompletionRequest request)
        {
            CreateCompletionResponse? ret = null;
            HttpClient httpClient = HttpClientFactory.CreateClient();
            FillBaseAddress(httpClient);
            FillAuthRequestHeaders(httpClient.DefaultRequestHeaders);

            var jsonContent = CreateJsonStringContent(request);

            var httpResponse = await httpClient.PostAsync($"completions", jsonContent);
            if (httpResponse.IsSuccessStatusCode)
            {
                ret = await httpResponse.Content.ReadFromJsonAsync<CreateCompletionResponse>();
            }
            return ret;
        }

        /// <summary>
        /// Creates a model response for the given chat conversation.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CreateChatCompletionResponse> CreateChatCompletionAsync(CreateChatCompletionRequest request)
        {
            CreateChatCompletionResponse? ret = null;
            HttpClient httpClient = HttpClientFactory.CreateClient();
            FillBaseAddress(httpClient);
            FillAuthRequestHeaders(httpClient.DefaultRequestHeaders);

            var jsonContent = CreateJsonStringContent(request);

            var httpResponse = await httpClient.PostAsync($"chat/completions", jsonContent);
            if (httpResponse.IsSuccessStatusCode)
            {
                ret = await httpResponse.Content.ReadFromJsonAsync<CreateChatCompletionResponse>();
            }
            return ret;
        }

        /// <summary>
        /// Given a prompt and an instruction, the model will return an edited version of the prompt.
        /// Creates a new edit for the provided input, instruction, and parameters.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CreateEditResponse> CreateEditAsync(CreateEditRequest request)
        {
            CreateEditResponse? ret = null;
            HttpClient httpClient = HttpClientFactory.CreateClient();
            FillBaseAddress(httpClient);
            FillAuthRequestHeaders(httpClient.DefaultRequestHeaders);

            var jsonContent = CreateJsonStringContent(request);

            var httpResponse = await httpClient.PostAsync($"edits", jsonContent);
            if (httpResponse.IsSuccessStatusCode)
            {
                ret = await httpResponse.Content.ReadFromJsonAsync<CreateEditResponse>();
            }
            return ret;
        }

        /// <summary>
        /// Creates an image given a prompt.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ImagesResponse> CreateImageAsync(CreateImageRequest request)
        {
            ImagesResponse? ret = null;
            HttpClient httpClient = HttpClientFactory.CreateClient();
            FillBaseAddress(httpClient);
            FillAuthRequestHeaders(httpClient.DefaultRequestHeaders);

            var jsonContent = CreateJsonStringContent(request);

            var httpResponse = await httpClient.PostAsync($"images/generations", jsonContent);
            if (httpResponse.IsSuccessStatusCode)
            {
                ret = await httpResponse.Content.ReadFromJsonAsync<ImagesResponse>();
            }
            return ret;
        }

        /// <summary>
        /// Creates an edited or extended image given an original image and a prompt.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ResponseDTO<ImagesResponse>> CreateImageEditAsync(CreateImageEditRequest request)
        {
            ResponseDTO<ImagesResponse> ret = new ResponseDTO<ImagesResponse>();
            HttpClient httpClient = HttpClientFactory.CreateClient();
            FillBaseAddress(httpClient);
            FillAuthRequestHeaders(httpClient.DefaultRequestHeaders);

            using (var content = new MultipartFormDataContent())
            {
                content.Add(new ByteArrayContent(request.Image), "image", "image.png");

                if (request.Mask != null)
                {
                    content.Add(new ByteArrayContent(request.Mask), "mask", "mask.png");
                }

                content.Add(new StringContent(request.Prompt), "prompt");

                if (request.N != null)
                {
                    content.Add(new StringContent(request.N.ToString()), "n");
                }

                if (request.Size != null)
                {
                    switch (request.Size.Value)
                    {
                        case CreateImageRequestSizeEnum._1024x1024:
                            content.Add(new StringContent("1024x1024"), "size");
                            break;
                        case CreateImageRequestSizeEnum._512x512:
                            content.Add(new StringContent("512x512"), "size");
                            break;
                        case CreateImageRequestSizeEnum._256x256:
                            content.Add(new StringContent("256x256"), "size");
                            break;
                    }
                }

                if (request.ResponseFormat != null)
                {
                    switch (request.ResponseFormat.Value)
                    {
                        case CreateImageRequestResponseFormatEnum.B64Json:
                            content.Add(new StringContent("b64_json"), "response_format");
                            break;
                        case CreateImageRequestResponseFormatEnum.Url:
                            content.Add(new StringContent("url"), "response_format");
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(request.User))
                {
                    content.Add(new StringContent(request.User), "user");
                }

                

                var httpResponse = await httpClient.PostAsync("images/edits", content);
                if (httpResponse.IsSuccessStatusCode)
                {
                    ret.OpenAIResponse = await httpResponse.Content.ReadFromJsonAsync<ImagesResponse>();
                }
                else
                {
                    ret.Error = await httpResponse.Content.ReadFromJsonAsync<ErrorResponse>() ?? ErrorResponse.CreateDefaultErrorResponse();
                }

                return ret;
            }
        }

        /// <summary>
        /// Creates a variation of a given image. (BETA)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ResponseDTO<ImagesResponse>> CreateImageVariationsAsync(CreateImageVariationsRequest request)
        {
            ResponseDTO<ImagesResponse> ret = new ResponseDTO<ImagesResponse>();
            HttpClient httpClient = HttpClientFactory.CreateClient();
            FillBaseAddress(httpClient);
            FillAuthRequestHeaders(httpClient.DefaultRequestHeaders);

            using (var content = new MultipartFormDataContent())
            {
                content.Add(new ByteArrayContent(request.Image), "image", "image.png");

                content.Add(new StringContent(request.Prompt), "prompt");

                if (request.N != null)
                {
                    content.Add(new StringContent(request.N.ToString()), "n");
                }

                if (request.Size != null)
                {
                    switch (request.Size.Value)
                    {
                        case CreateImageRequestSizeEnum._1024x1024:
                            content.Add(new StringContent("1024x1024"), "size");
                            break;
                        case CreateImageRequestSizeEnum._512x512:
                            content.Add(new StringContent("512x512"), "size");
                            break;
                        case CreateImageRequestSizeEnum._256x256:
                            content.Add(new StringContent("256x256"), "size");
                            break;
                    }
                }

                if (request.ResponseFormat != null)
                {
                    switch (request.ResponseFormat.Value)
                    {
                        case CreateImageRequestResponseFormatEnum.B64Json:
                            content.Add(new StringContent("b64_json"), "response_format");
                            break;
                        case CreateImageRequestResponseFormatEnum.Url:
                            content.Add(new StringContent("url"), "response_format");
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(request.User))
                {
                    content.Add(new StringContent(request.User), "user");
                }

                var httpResponse = await httpClient.PostAsync("images/edits", content);
                if (httpResponse.IsSuccessStatusCode)
                {
                    ret.OpenAIResponse = await httpResponse.Content.ReadFromJsonAsync<ImagesResponse>();
                }
                else
                {
                    ret.Error = await httpResponse.Content.ReadFromJsonAsync<ErrorResponse>() ?? ErrorResponse.CreateDefaultErrorResponse();
                }

                return ret;
            }
        }

        /// <summary>
        /// Creates an embedding vector representing the input text.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CreateEmbeddingResponse> CreateEmbeddingsAsync(CreateEmbeddingsRequest request)
        {
            CreateEmbeddingResponse? ret = null;
            HttpClient httpClient = HttpClientFactory.CreateClient();
            FillBaseAddress(httpClient);
            FillAuthRequestHeaders(httpClient.DefaultRequestHeaders);

            var jsonContent = CreateJsonStringContent(request);

            var httpResponse = await httpClient.PostAsync($"embeddings", jsonContent);
            if (httpResponse.IsSuccessStatusCode)
            {
                ret = await httpResponse.Content.ReadFromJsonAsync<CreateEmbeddingResponse>();
            }
            return ret;

        }

        /// <summary>
        /// Transcribes audio into the input language.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ResponseDTO<CreateTranscriptionsResponse>> CreateTranscriptionsAsync(CreateTranscriptionsRequest request)
        {
            ResponseDTO<CreateTranscriptionsResponse> ret = new ResponseDTO<CreateTranscriptionsResponse>();
            HttpClient httpClient = HttpClientFactory.CreateClient();
            FillBaseAddress(httpClient);
            FillAuthRequestHeaders(httpClient.DefaultRequestHeaders);

            using (var content = new MultipartFormDataContent())
            {
                content.Add(new StringContent(request.ModelID), "model");

                content.Add(new ByteArrayContent(request.File), "file", "audio.mp3"); //TODO: add enum for file type

                if (!string.IsNullOrEmpty(request.Prompt))
                {
                    content.Add(new StringContent(request.Prompt), "prompt");
                }

                if (request.ResponseFormat != null)
                {
                    switch (request.ResponseFormat.Value)
                    {
                        case CreateTranscriptionsRequestEnum.Json:
                            content.Add(new StringContent("json"), "response_format");
                            break;
                        case CreateTranscriptionsRequestEnum.Text:
                            content.Add(new StringContent("text"), "response_format");
                            break;
                        case CreateTranscriptionsRequestEnum.Srt:
                            content.Add(new StringContent("srt"), "response_format");
                            break;
                        case CreateTranscriptionsRequestEnum.VerboseJson:
                            content.Add(new StringContent("verbose_json"), "response_format");
                            break;
                        case CreateTranscriptionsRequestEnum.Vtt:
                            content.Add(new StringContent("vtt"), "response_format");
                            break;
                    }
                }

                if (request.Temperature!=null)
                {
                    content.Add(new StringContent(request.Temperature.ToString()), "temperature");
                }

                if (!string.IsNullOrEmpty(request.Language))
                {
                    content.Add(new StringContent(request.Language), "language");
                }

                var httpResponse = await httpClient.PostAsync("audio/transcriptions", content);
                if (httpResponse.IsSuccessStatusCode)
                {
                    ret.OpenAIResponse = await httpResponse.Content.ReadFromJsonAsync<CreateTranscriptionsResponse>();
                }
                else
                {
                    ret.Error = await httpResponse.Content.ReadFromJsonAsync<ErrorResponse>();
                }

                return ret;
            }
        }

        /// <summary>
        /// Translates audio into into English.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ResponseDTO<CreateTranslationsResponse>> CreateTranslationsAsync(CreateTranslationsRequest request)
        {
            ResponseDTO<CreateTranslationsResponse> ret = new ResponseDTO<CreateTranslationsResponse>();
            HttpClient httpClient = HttpClientFactory.CreateClient();
            FillBaseAddress(httpClient);
            FillAuthRequestHeaders(httpClient.DefaultRequestHeaders);

            using (var content = new MultipartFormDataContent())
            {
                content.Add(new StringContent(request.ModelID), "model");

                content.Add(new ByteArrayContent(request.File), "file", "audio.mp3"); //TODO: add enum for file type

                if (!string.IsNullOrEmpty(request.Prompt))
                {
                    content.Add(new StringContent(request.Prompt), "prompt");
                }

                if (request.ResponseFormat != null)
                {
                    switch (request.ResponseFormat.Value)
                    {
                        case CreateTranscriptionsRequestEnum.Json:
                            content.Add(new StringContent("json"), "response_format");
                            break;
                        case CreateTranscriptionsRequestEnum.Text:
                            content.Add(new StringContent("text"), "response_format");
                            break;
                        case CreateTranscriptionsRequestEnum.Srt:
                            content.Add(new StringContent("srt"), "response_format");
                            break;
                        case CreateTranscriptionsRequestEnum.VerboseJson:
                            content.Add(new StringContent("verbose_json"), "response_format");
                            break;
                        case CreateTranscriptionsRequestEnum.Vtt:
                            content.Add(new StringContent("vtt"), "response_format");
                            break;
                    }
                }

                if (request.Temperature != null)
                {
                    content.Add(new StringContent(request.Temperature.ToString()), "temperature");
                }


                var httpResponse = await httpClient.PostAsync("audio/translations", content);
                if (httpResponse.IsSuccessStatusCode)
                {
                    ret.OpenAIResponse = await httpResponse.Content.ReadFromJsonAsync<CreateTranslationsResponse>();
                }
                else
                {
                    ret.Error = await httpResponse.Content.ReadFromJsonAsync<ErrorResponse>();
                }

                return ret;
            }
        }

        private StringContent CreateJsonStringContent(object request)
        {
            var jsonString = JsonSerializer.Serialize(request, _jsonSerializerOptions);
            var jsonContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            return jsonContent;
        }

        private static void FillBaseAddress(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri(baseAddress);
        }
        private string ComposeAuthHeader()
        {
            return CurrentEnvironment.GetApiKey();
        }
        private string? GetOrganizationHeader()
        {
            return CurrentEnvironment.GetOrganization();
        }
        private void FillAuthRequestHeaders(HttpRequestHeaders headers)
        {
            //JwtBearerDefaults.AuthenticationScheme
            headers.Authorization = new AuthenticationHeaderValue("Bearer", ComposeAuthHeader());
            var orgHeader = GetOrganizationHeader();
            if (!string.IsNullOrEmpty(orgHeader))
            {
                headers.Add("OpenAI-Organization", orgHeader);
            }
        }
    }
}
