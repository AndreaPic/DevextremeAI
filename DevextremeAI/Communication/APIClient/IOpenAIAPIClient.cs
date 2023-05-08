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

namespace DevextremeAI.Communication.APIClient
{

    public interface IOpenAIAPIClient
    {
        public Task<ResponseDTO<ListModelsResponse>> GetModelsAsync();
        public Task<ResponseDTO<Model>> GetModelAsync(string modelID);
        public Task<ResponseDTO<CreateCompletionResponse>> CreateCompletionAsync(CreateCompletionRequest request);
        public Task<ResponseDTO<CreateChatCompletionResponse>> CreateChatCompletionAsync(CreateChatCompletionRequest request);
        public Task<ResponseDTO<CreateEditResponse>> CreateEditAsync(CreateEditRequest request);
        public Task<ResponseDTO<ImagesResponse>> CreateImageAsync(CreateImageRequest request);
        public Task<ResponseDTO<ImagesResponse>> CreateImageEditAsync(CreateImageEditRequest request);
        public Task<ResponseDTO<ImagesResponse>> CreateImageVariationsAsync(CreateImageVariationsRequest request);
        public Task<CreateEmbeddingResponse> CreateEmbeddingsAsync(CreateEmbeddingsRequest request);
        public Task<ResponseDTO<CreateTranscriptionsResponse>> CreateTranscriptionsAsync(CreateTranscriptionsRequest request);
        public Task<ResponseDTO<CreateTranslationsResponse>> CreateTranslationsAsync(CreateTranslationsRequest request);
        public Task<ResponseDTO<FileDataListResponse>> GetFilesDataAsync();
        public Task<ResponseDTO<FileData>> UploadFileAsync(UploadFileRequest request);
        public Task<ResponseDTO<FileData>> UploadFineTuningFileAsync(UploadFineTuningFileRequest request);
        public Task<ResponseDTO<DeleteFileResponse>> DeleteFileAsync(DeleteFileRequest request);
        public Task<ResponseDTO<FileData>> GetFileDataAsync(RetrieveFileDataRequest request);
        public Task<ResponseDTO<RetrieveFileContentResponse>> GetFileContentAsync(RetrieveFileContentRequest request);
        public Task<ResponseDTO<FineTuneData>> CreateFineTuneJobAsync(CreateFineTuneRequest request);
        public Task<ResponseDTO<GetFineTuneListResponse>> GetFineTuneJobListAsync(CreateFineTuneRequest request);
        public Task<ResponseDTO<FineTuneData>> GetFineTuneJobDataAsync(FineTuneRequest request);
        public Task<ResponseDTO<FineTuneData>> CancelFineTuneJobAsync(FineTuneRequest request);
        public Task<ResponseDTO<GetFineTuneEventListResponse>> GetFineTuneEventListAsync(FineTuneRequest request);
        public IAsyncEnumerable<Event> GetFineTuneEventStreamAsync(FineTuneRequest request);
        public Task<ResponseDTO<DeleteFineTuneModelResponse>> DeleteFineTuneModelAsync(FineTuneRequest request);

    }
}
