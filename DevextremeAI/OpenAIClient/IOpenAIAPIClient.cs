using System;
using System.Collections.Generic;
using System.Linq;
using DevExtremeAI.OpenAIDTO;

namespace DevExtremeAI.OpenAIClient
{
    public interface IOpenAIAPIClient
    {
        /// <summary>
        /// List and describe the various models available in the API.
        /// You can refer to the Models documentation to understand what models are available and the differences between them.
        /// Lists the currently available models, and provides basic information about each one such as the owner and availability.
        /// </summary>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public Task<ResponseDTO<ListModelsResponse>> GetModelsAsync();
        /// <summary>
        /// Retrieves a model instance, providing basic information about the model such as the owner and permissioning.
        /// </summary>
        /// <param name="modelID">The ID of the model to use for this request</param>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public Task<ResponseDTO<Model>> GetModelAsync(string modelID);
        /// <summary>
        /// Creates a completion for the provided prompt and parameters.
        /// Given a prompt, the model will return one or more predicted completions, and can also return the probabilities of alternative tokens at each position.
        /// </summary>
        /// <param name="request">DTO with request specs.</param>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public Task<ResponseDTO<CreateCompletionResponse>> CreateCompletionAsync(CreateCompletionRequest request);
        /// <summary>
        /// Creates a model response for the given chat conversation.
        /// </summary>
        /// <param name="request">DTO with request specs.</param>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public Task<ResponseDTO<CreateChatCompletionResponse>> CreateChatCompletionAsync(CreateChatCompletionRequest request);
        /// <summary>
        /// Given a prompt and an instruction, the model will return an edited version of the prompt.
        /// Creates a new edit for the provided input, instruction, and parameters.
        /// </summary>
        /// <param name="request">DTO with request specs.</param>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public Task<ResponseDTO<CreateEditResponse>> CreateEditAsync(CreateEditRequest request);
        /// <summary>
        /// Creates an image given a prompt.
        /// </summary>
        /// <param name="request">DTO with request specs.</param>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public Task<ResponseDTO<ImagesResponse>> CreateImageAsync(CreateImageRequest request);
        /// <summary>
        /// Creates an edited or extended image given an original image and a prompt.
        /// </summary>
        /// <param name="request">DTO with request specs.</param>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public Task<ResponseDTO<ImagesResponse>> CreateImageEditAsync(CreateImageEditRequest request);
        /// <summary>
        /// Creates a variation of a given image. (BETA)
        /// </summary>
        /// <param name="request">DTO with request specs.</param>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public Task<ResponseDTO<ImagesResponse>> CreateImageVariationsAsync(CreateImageVariationsRequest request);
        /// <summary>
        /// Creates an embedding vector representing the input text.
        /// </summary>
        /// <param name="request">DTO with request specs.</param>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public Task<ResponseDTO<CreateEmbeddingResponse>> CreateEmbeddingsAsync(CreateEmbeddingsRequest request);
        /// <summary>
        /// Transcribes audio into the input language.
        /// </summary>
        /// <param name="request">DTO with request specs.</param>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public Task<ResponseDTO<CreateTranscriptionsResponse>> CreateTranscriptionsAsync(CreateTranscriptionsRequest request);
        /// <summary>
        /// Translates audio into into English.
        /// </summary>
        /// <param name="request">DTO with request specs.</param>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public Task<ResponseDTO<CreateTranslationsResponse>> CreateTranslationsAsync(CreateTranslationsRequest request);
        /// <summary>
        /// Returns a list of files that belong to the user's organization.
        /// </summary>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public Task<ResponseDTO<FileDataListResponse>> GetFilesDataAsync();
        /// <summary>
        /// Upload a file that contains document(s) to be used across various endpoints/features.
        /// Currently, the size of all the files uploaded by one organization can be up to 1 GB.
        /// Please contact us if you need to increase the storage limit.
        /// </summary>
        /// <param name="request">DTO with request specs.</param>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public Task<ResponseDTO<FileData>> UploadFileAsync(UploadFileRequest request);
        /// <summary>
        /// Upload a file that contains document(s) to be used across various endpoints/features.
        /// Currently, the size of all the files uploaded by one organization can be up to 1 GB.
        /// Please contact us if you need to increase the storage limit.
        /// </summary>
        /// <param name="request">DTO with request specs.</param>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public Task<ResponseDTO<FileData>> UploadFineTuningFileAsync(UploadFineTuningFileRequest request);
        /// <summary>
        /// Delete a file.
        /// </summary>
        /// <param name="request">DTO with request specs.</param>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public Task<ResponseDTO<DeleteFileResponse>> DeleteFileAsync(DeleteFileRequest request);
        /// <summary>
        /// Returns information about a specific file.
        /// </summary>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public Task<ResponseDTO<FileData>> GetFileDataAsync(RetrieveFileDataRequest request);
        /// <summary>
        /// Returns the contents of the specified file
        /// </summary>
        /// <param name="request">DTO with request specs.</param>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public Task<ResponseDTO<RetrieveFileContentResponse>> GetFileContentAsync(RetrieveFileContentRequest request);
        /// <summary>
        /// Creates a job that fine-tunes a specified model from a given dataset.
        /// Response includes details of the enqueued job including job status and the name of the fine-tuned models once complete.
        /// </summary>
        /// <param name="request">DTO with request specs.</param>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public Task<ResponseDTO<FineTuneData>> CreateFineTuneJobAsync(CreateFineTuneRequest request);
        /// <summary>
        /// List your organization's fine-tuning jobs
        /// </summary>
        /// <param name="request">DTO with request specs.</param>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public Task<ResponseDTO<GetFineTuneListResponse>> GetFineTuneJobListAsync();
        /// <summary>
        /// Gets info about the fine-tune job.
        /// </summary>
        /// <param name="request">DTO with request specs.</param>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public Task<ResponseDTO<FineTuneData>> GetFineTuneJobDataAsync(FineTuneRequest request);
        /// <summary>
        /// Immediately cancel a fine-tune job.
        /// </summary>
        /// <param name="request">DTO with request specs.</param>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public Task<ResponseDTO<FineTuneData>> CancelFineTuneJobAsync(FineTuneRequest request);
        /// <summary>
        /// Get fine-grained status updates for a fine-tune job.
        /// If set to false, only events generated so far will be returned.
        /// </summary>
        /// <param name="request">DTO with request specs.</param>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public Task<ResponseDTO<GetFineTuneEventListResponse>> GetFineTuneEventListAsync(FineTuneRequest request);
        /// <summary>
        /// Get fine-grained status updates for a fine-tune job.
        /// Whether to stream events for the fine-tune job.
        /// Events will be sent as data-only server-sent events as they become available.
        /// The stream will terminate with a data: [DONE] message when the job is finished (succeeded, cancelled, or failed).
        /// </summary>
        /// <param name="request">DTO with request specs.</param>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public IAsyncEnumerable<Event> GetFineTuneEventStreamAsync(FineTuneRequest request);
        /// <summary>
        /// Delete a fine-tuned model. You must have the Owner role in your organization.
        /// </summary>
        /// <param name="request">DTO with request specs.</param>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public Task<ResponseDTO<DeleteFineTuneModelResponse>> DeleteFineTuneModelAsync(FineTuneRequest request);
        /// <summary>
        /// Creates an embedding vector representing the input text.
        /// </summary>
        /// <param name="request">DTO with request specs.</param>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public Task<ResponseDTO<ModerationsResponse>> CreateModerationsAsync(CreateModerationsRequest request);
    }
}
