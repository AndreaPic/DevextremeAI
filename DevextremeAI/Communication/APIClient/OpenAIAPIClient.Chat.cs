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
using DevextremeAI.Communication.DTO;
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
        public async Task<ResponseDTO<CreateChatCompletionResponse>> CreateChatCompletionAsync(CreateChatCompletionRequest request)
        {
            ResponseDTO<CreateChatCompletionResponse> ret = new ResponseDTO<CreateChatCompletionResponse>();
            HttpClient httpClient = HttpClientFactory.CreateClient();
            FillBaseAddress(httpClient);
            FillAuthRequestHeaders(httpClient.DefaultRequestHeaders);

            var jsonContent = CreateJsonStringContent(request);

            var httpResponse = await httpClient.PostAsync($"chat/completions", jsonContent);
            if (httpResponse.IsSuccessStatusCode)
            {
                ret.OpenAIResponse = await httpResponse.Content.ReadFromJsonAsync<CreateChatCompletionResponse>();
            }
            else
            {
                ret.Error = await httpResponse.Content.ReadFromJsonAsync<ErrorResponse>() ?? ErrorResponse.CreateDefaultErrorResponse();
            }

            return ret;
        }

    }
}
