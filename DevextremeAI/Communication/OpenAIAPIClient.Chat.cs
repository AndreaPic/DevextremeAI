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

    partial class OpenAIAPIClient 
    {

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

    }
}
