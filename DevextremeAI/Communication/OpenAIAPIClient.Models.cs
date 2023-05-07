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


    }
}
