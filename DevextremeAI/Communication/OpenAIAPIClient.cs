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
