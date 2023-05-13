using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using DevExtremeAI.OpenAIDTO;
using DevExtremeAI.Settings;

namespace DevExtremeAI.OpenAIClient
{

    public sealed partial class OpenAIAPIClient : IOpenAIAPIClient
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
