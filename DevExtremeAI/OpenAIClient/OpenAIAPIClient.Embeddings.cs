﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using DevExtremeAI.OpenAIDTO;
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
        public async Task<ResponseDTO<CreateEmbeddingResponse>> CreateEmbeddingsAsync(CreateEmbeddingsRequest request)
        {
            ResponseDTO<CreateEmbeddingResponse> ret = new ResponseDTO<CreateEmbeddingResponse>();

            HttpClient httpClient = CreateHttpClient(out bool doDispose);
            try
            {
                FillBaseAddress(httpClient);
                FillAuthRequestHeaders(httpClient.DefaultRequestHeaders);

                var jsonContent = CreateJsonStringContent(request);

                var httpResponse = await httpClient.PostAsync($"embeddings", jsonContent);
                if (httpResponse.IsSuccessStatusCode)
                {
                    ret.OpenAIResponse = await httpResponse.Content.ReadFromJsonAsync<CreateEmbeddingResponse>();
                }
                else
                {
                    ret.ErrorResponse = await httpResponse.Content.ReadFromJsonAsync<ErrorResponse>() ?? ErrorResponse.CreateDefaultErrorResponse();
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
