using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using DevExtremeAI.OpenAIDTO;

namespace DevExtremeAI.OpenAIClient
{

    partial class OpenAIAPIClient 
    {
        private const string modelsPath = "models";

        /// <summary>
        /// List and describe the various models available in the API.
        /// You can refer to the Models documentation to understand what models are available and the differences between them.
        /// Lists the currently available models, and provides basic information about each one such as the owner and availability.
        /// </summary>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public async Task<ResponseDTO<ListModelsResponse>> GetModelsAsync()
        {
            ResponseDTO<ListModelsResponse> ret = new ResponseDTO<ListModelsResponse>();
            HttpClient httpClient = CreateHttpClient(out bool doDispose);
            try
            {
                FillBaseAddress(httpClient);
                FillAuthRequestHeaders(httpClient.DefaultRequestHeaders);
                var httpResponse = await httpClient.GetAsync(modelsPath);
                if (httpResponse.IsSuccessStatusCode)
                {
                    ret.OpenAIResponse = await httpResponse.Content.ReadFromJsonAsync<ListModelsResponse>();
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

        /// <summary>
        /// Retrieves a model instance, providing basic information about the model such as the owner and permissioning.
        /// </summary>
        /// <param name="modelID">The ID of the model to use for this request</param>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public async Task<ResponseDTO<Model>> GetModelAsync(string modelID)
        {
            ResponseDTO<Model> ret = new ResponseDTO<Model>();
            HttpClient httpClient = CreateHttpClient(out bool doDispose);
            try
            {
                FillBaseAddress(httpClient);
                FillAuthRequestHeaders(httpClient.DefaultRequestHeaders);
                var httpResponse = await httpClient.GetAsync($"models/{modelID}");
                if (httpResponse.IsSuccessStatusCode)
                {
                    ret.OpenAIResponse = await httpResponse.Content.ReadFromJsonAsync<Model>();
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
