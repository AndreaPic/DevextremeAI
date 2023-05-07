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
        public Task<ImagesResponse> CreateImageAsync(CreateImageRequest request);
        public Task<ResponseDTO<ImagesResponse>> CreateImageEditAsync(CreateImageEditRequest request);
        public Task<ResponseDTO<ImagesResponse>> CreateImageVariationsAsync(CreateImageVariationsRequest request);
        public Task<CreateEmbeddingResponse> CreateEmbeddingsAsync(CreateEmbeddingsRequest request);
        public Task<ResponseDTO<CreateTranscriptionsResponse>> CreateTranscriptionsAsync(CreateTranscriptionsRequest request);
        public Task<ResponseDTO<CreateTranslationsResponse>> CreateTranslationsAsync(CreateTranslationsRequest request);


    }
}
