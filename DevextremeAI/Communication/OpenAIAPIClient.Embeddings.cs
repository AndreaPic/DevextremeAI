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

    }
}
