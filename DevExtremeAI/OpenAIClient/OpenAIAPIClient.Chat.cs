using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
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
        /// <remarks>With this method the Stream property of CreateChatCompletionRequest is forced false</remarks>
        public async Task<ResponseDTO<CreateChatCompletionResponse>> CreateChatCompletionAsync(CreateChatCompletionRequest request)
        {
            ResponseDTO<CreateChatCompletionResponse> ret = new ResponseDTO<CreateChatCompletionResponse>();
            HttpClient httpClient = CreateHttpClient(out bool doDispose);
            try
            {
                request.Stream = false;
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
        /// Creates a model response for the given chat conversation in stream way.
        /// </summary>
        /// <param name="request">DTO with request specs.</param>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        /// <remarks>With this method the Stream property of CreateChatCompletionRequest is forced true</remarks>
        public async IAsyncEnumerable<ResponseDTO<CreateChatCompletionResponse>> CreateChatCompletionStreamAsync(CreateChatCompletionRequest request)
        {
            request.Stream = true;
            HttpClient httpClient = CreateHttpClient(out bool doDispose);
            try
            {

                request.Stream = true;
                FillBaseAddress(httpClient);
                FillAuthRequestHeaders(httpClient.DefaultRequestHeaders);

                var jsonContent = CreateJsonStringContent(request);

                var httpResponse = await httpClient.PostAsync($"chat/completions", jsonContent);
                ResponseDTO<CreateChatCompletionResponse> ret = new ResponseDTO<CreateChatCompletionResponse>();
                if (httpResponse.IsSuccessStatusCode)
                {
                    await using var stream = await httpResponse.Content.ReadAsStreamAsync();
                    using var reader = new StreamReader(stream);

                    bool stop = false;
                    do
                    {
                        ret = new ResponseDTO<CreateChatCompletionResponse>();
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
                                    ret.OpenAIResponse = JsonSerializer.Deserialize<CreateChatCompletionResponse>(line);
                                    yield return ret;
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
                    ErrorResponse error = await httpResponse.Content.ReadFromJsonAsync<ErrorResponse>() ??
                                          ErrorResponse.CreateDefaultErrorResponse();
                    ret.ErrorResponse = error;
                    yield return ret;
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

    }
}
