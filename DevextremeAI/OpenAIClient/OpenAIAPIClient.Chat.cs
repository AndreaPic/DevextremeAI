using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using DevExtremeAI.OpenAIDTO;

namespace DevExtremeAI.OpenAIClient
{

    partial class OpenAIAPIClient 
    {

        /// <summary>
        /// Creates a model response for the given chat conversation.
        /// </summary>
        /// <param name="request">DTO with request specs.</param>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public async Task<ResponseDTO<CreateChatCompletionResponse>> CreateChatCompletionAsync(CreateChatCompletionRequest request)
        {
            ResponseDTO<CreateChatCompletionResponse> ret = new ResponseDTO<CreateChatCompletionResponse>();
            HttpClient httpClient = CreateHttpClient(out bool doDispose);
            try
            {
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
            finally
            {
                if (doDispose)
                {
                    httpClient.Dispose();
                }
            }
        }

    }
}
