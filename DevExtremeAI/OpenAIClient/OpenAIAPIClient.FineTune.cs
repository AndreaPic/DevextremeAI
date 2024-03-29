﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using DevExtremeAI.OpenAIDTO;

namespace DevExtremeAI.OpenAIClient
{
    /// <summary>
    /// Manage Fine Tuning API
    /// </summary>
    /// <remarks>
    /// The Fine-tunes endpoint will be shut off on January 4th, 2024.
    /// </remarks>
    partial class OpenAIAPIClient 
    {
        /// <summary>
        /// Creates a job that fine-tunes a specified model from a given dataset.
        /// Response includes details of the enqueued job including job status and the name of the fine-tuned models once complete.
        /// </summary>
        /// <param name="request">DTO with request specs.</param>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public async Task<ResponseDTO<FineTuneData>> CreateFineTuneJobAsync(CreateFineTuneRequest request)
        {
            ResponseDTO<FineTuneData> ret = new ResponseDTO<FineTuneData>();
            HttpClient httpClient = CreateHttpClient(out bool doDispose);
            try
            {
                FillBaseAddress(httpClient);
                FillAuthRequestHeaders(httpClient.DefaultRequestHeaders);

                var jsonContent = CreateJsonStringContent(request);

                var httpResponse = await httpClient.PostAsync($"fine_tuning/jobs", jsonContent);
                if (httpResponse.IsSuccessStatusCode)
                {
                    ret.OpenAIResponse = await httpResponse.Content.ReadFromJsonAsync<FineTuneData>();
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

        /// <summary>
        /// List your organization's fine-tuning jobs
        /// </summary>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public async Task<ResponseDTO<GetFineTuneListResponse>> GetFineTuneJobListAsync()
        {
            return await GetFineTuneJobListAsync(null, null);
        }

        /// <summary>
        /// List your organization's fine-tuning jobs
        /// </summary>
        /// <param name="after">Identifier for the last job from the previous pagination request.(Optional)</param>
        /// <param name="limit">Number of fine-tuning jobs to retrieve.(Optional Default 20)</param>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public async Task<ResponseDTO<GetFineTuneListResponse>> GetFineTuneJobListAsync(string after, int? limit)
        {
            ResponseDTO<GetFineTuneListResponse> ret = new ResponseDTO<GetFineTuneListResponse>();
            HttpClient httpClient = CreateHttpClient(out bool doDispose);
            try
            {
                FillBaseAddress(httpClient);
                FillAuthRequestHeaders(httpClient.DefaultRequestHeaders);

                string request = "fine_tuning/jobs";
                if (!string.IsNullOrEmpty(after) || limit.HasValue)
                {
                    request += "?" ;
                    if (!string.IsNullOrEmpty(after))
                    {
                        request += $"after={after}" ;
                    }
                    if (!string.IsNullOrEmpty(after) && limit.HasValue)
                    {
                        request += "&";
                    }
                    if (limit.HasValue)
                    {
                        request += $"limit={limit}";
                    }
                }

                var httpResponse = await httpClient.GetAsync(request);
                if (httpResponse.IsSuccessStatusCode)
                {
                    ret.OpenAIResponse = await httpResponse.Content.ReadFromJsonAsync<GetFineTuneListResponse>();
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


        /// <summary>
        /// Gets info about the fine-tune job.
        /// </summary>
        /// <param name="request">DTO with request specs.</param>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public async Task<ResponseDTO<FineTuneData>> GetFineTuneJobDataAsync(FineTuneRequest request)
        {
            ResponseDTO<FineTuneData> ret = new ResponseDTO<FineTuneData>();
            HttpClient httpClient = CreateHttpClient(out bool doDispose);
            try
            {
                FillBaseAddress(httpClient);
                FillAuthRequestHeaders(httpClient.DefaultRequestHeaders);

                var httpResponse = await httpClient.GetAsync($"fine_tuning/jobs/{request.FineTuneId}");
                if (httpResponse.IsSuccessStatusCode)
                {
                    ret.OpenAIResponse = await httpResponse.Content.ReadFromJsonAsync<FineTuneData>();
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

        /// <summary>
        /// Immediately cancel a fine-tune job.
        /// </summary>
        /// <param name="request">DTO with request specs.</param>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public async Task<ResponseDTO<FineTuneData>> CancelFineTuneJobAsync(FineTuneRequest request)
        {
            ResponseDTO<FineTuneData> ret = new ResponseDTO<FineTuneData>();
            HttpClient httpClient = CreateHttpClient(out bool doDispose);
            try
            {
                FillBaseAddress(httpClient);
                FillAuthRequestHeaders(httpClient.DefaultRequestHeaders);

                var httpResponse = await httpClient.PostAsync($"fine_tuning/jobs/{request.FineTuneId}/cancel",null);
                if (httpResponse.IsSuccessStatusCode)
                {
                    ret.OpenAIResponse = await httpResponse.Content.ReadFromJsonAsync<FineTuneData>();
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

        /// <summary>
        /// Get fine-grained status updates for a fine-tune job.
        /// If set to false, only events generated so far will be returned.
        /// </summary>
        /// <param name="request">DTO with request specs.</param>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public async Task<ResponseDTO<GetFineTuneEventListResponse>> GetFineTuneEventListAsync(FineTuneRequest request)
        {
            ResponseDTO<GetFineTuneEventListResponse> ret = new ResponseDTO<GetFineTuneEventListResponse>();
            HttpClient httpClient = CreateHttpClient(out bool doDispose);
            try
            {
                FillBaseAddress(httpClient);
                FillAuthRequestHeaders(httpClient.DefaultRequestHeaders);

                var httpResponse = await httpClient.GetAsync($"fine_tuning/jobs/{request.FineTuneId}/events");
                if (httpResponse.IsSuccessStatusCode)
                {
                    ret.OpenAIResponse = await httpResponse.Content.ReadFromJsonAsync<GetFineTuneEventListResponse>();
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


        /// <summary>
        /// Get fine-grained status updates for a fine-tune job.
        /// Whether to stream events for the fine-tune job.
        /// Events will be sent as data-only server-sent events as they become available.
        /// The stream will terminate with a data: [DONE] message when the job is finished (succeeded, cancelled, or failed).
        /// </summary>
        /// <param name="request">DTO with request specs.</param>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public async IAsyncEnumerable<Event> GetFineTuneEventStreamAsync(FineTuneRequest request)
        {
            HttpClient httpClient = CreateHttpClient(out bool doDispose);
            try
            {
                FillBaseAddress(httpClient);
                FillAuthRequestHeaders(httpClient.DefaultRequestHeaders);

                var httpResponse = await httpClient.GetAsync($"fine-tunes/{request.FineTuneId}/events?stream=true");
                if (httpResponse.IsSuccessStatusCode)
                {
                    await using var stream = await httpResponse.Content.ReadAsStreamAsync();
                    using var reader = new StreamReader(stream);

                    bool stop = false;
                    do
                    {
                        var line = await reader.ReadLineAsync();
                        if (string.IsNullOrWhiteSpace(line))
                        {
                            stop = false;
                        }
                        else
                        {
                            if (line.StartsWith(streamLineBegin))
                            {
                                line = line.Substring(streamLineBegin.Length);
                                if (line != streamDoneLine)
                                {
                                    yield return JsonSerializer.Deserialize<Event>(line);
                                    stop = false;
                                }
                                else
                                {
                                    stop = true;
                                }
                            }
                            else
                            {
                                //TODO: what is this?
                                stop = false;
                            }
                        }
                    } while (!stop);
                }
                else
                {
                    ErrorResponse error = await httpResponse.Content.ReadFromJsonAsync<ErrorResponse>() ?? ErrorResponse.CreateDefaultErrorResponse();
                    //TODO: notify the error in safe way
                }
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
        /// Delete a fine-tuned model. You must have the Owner role in your organization.
        /// </summary>
        /// <param name="request">DTO with request specs.</param>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public async Task<ResponseDTO<DeleteFineTuneModelResponse>> DeleteFineTuneModelAsync(FineTuneRequest request)
        {
            ResponseDTO<DeleteFineTuneModelResponse> ret = new ResponseDTO<DeleteFineTuneModelResponse>();
            HttpClient httpClient = CreateHttpClient(out bool doDispose);
            try
            {
                FillBaseAddress(httpClient);
                FillAuthRequestHeaders(httpClient.DefaultRequestHeaders);

                var httpResponse = await httpClient.DeleteAsync($"models/{request.FineTuneId}");
                if (httpResponse.IsSuccessStatusCode)
                {
                    ret.OpenAIResponse = await httpResponse.Content.ReadFromJsonAsync<DeleteFineTuneModelResponse>();
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
