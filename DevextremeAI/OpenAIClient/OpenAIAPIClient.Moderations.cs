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
        /// Creates an embedding vector representing the input text.
        /// </summary>
        /// <param name="request">DTO with request specs.</param>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public async Task<ResponseDTO<ModerationsResponse>> CreateModerationsAsync(CreateModerationsRequest request)
        {
            ResponseDTO<ModerationsResponse> ret = new ResponseDTO<ModerationsResponse>();
            HttpClient httpClient = CreateHttpClient(out bool doDispose);
            try
            {
                FillBaseAddress(httpClient);
                FillAuthRequestHeaders(httpClient.DefaultRequestHeaders);

                var jsonContent = CreateJsonStringContent(request);

                var httpResponse = await httpClient.PostAsync($"moderations ", jsonContent);
                if (httpResponse.IsSuccessStatusCode)
                {
                    ret.OpenAIResponse = await httpResponse.Content.ReadFromJsonAsync<ModerationsResponse>();
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
