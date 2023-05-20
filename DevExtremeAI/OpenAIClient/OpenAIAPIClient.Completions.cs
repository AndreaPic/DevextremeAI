using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using DevExtremeAI.OpenAIDTO;

namespace DevExtremeAI.OpenAIClient
{

    partial class OpenAIAPIClient 
    {
        /// <summary>
        /// Creates a completion for the provided prompt and parameters.
        /// Given a prompt, the model will return one or more predicted completions, and can also return the probabilities of alternative tokens at each position.
        /// </summary>
        /// <param name="request">DTO with request specs.</param>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        /// <remarks>With this method the Stream property of CreateChatCompletionRequest is forced false</remarks>
        public async Task<ResponseDTO<CreateCompletionResponse>> CreateCompletionAsync(CreateCompletionRequest request)
        {
            ResponseDTO<CreateCompletionResponse> ret = new ResponseDTO<CreateCompletionResponse>();
            HttpClient httpClient = CreateHttpClient(out bool doDispose);
            try
            {
                request.Stream = false;
                FillBaseAddress(httpClient);
                FillAuthRequestHeaders(httpClient.DefaultRequestHeaders);

                var jsonContent = CreateJsonStringContent(request);

                var httpResponse = await httpClient.PostAsync($"completions", jsonContent);
                if (httpResponse.IsSuccessStatusCode)
                {
                    ret.OpenAIResponse = await httpResponse.Content.ReadFromJsonAsync<CreateCompletionResponse>();
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
        /// Creates a completion for the provided prompt and parameters in stream way.
        /// Given a prompt, the model will return one or more predicted completions, and can also return the probabilities of alternative tokens at each position.
        /// </summary>
        /// <param name="request">DTO with request specs.</param>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        /// <remarks>With this method the Stream property of CreateChatCompletionRequest is forced true</remarks>
        public async IAsyncEnumerable<ResponseDTO<CreateCompletionResponse>> CreateCompletionStreamAsync(CreateCompletionRequest request)
        {
            HttpClient httpClient = CreateHttpClient(out bool doDispose);
            try
            {
                request.Stream = true;
                FillBaseAddress(httpClient);
                FillAuthRequestHeaders(httpClient.DefaultRequestHeaders);

                var jsonContent = CreateJsonStringContent(request);

                var httpResponse = await httpClient.PostAsync($"completions", jsonContent);
                ResponseDTO<CreateCompletionResponse> ret = new ResponseDTO<CreateCompletionResponse>();
                if (httpResponse.IsSuccessStatusCode)
                {
                    await using var stream = await httpResponse.Content.ReadAsStreamAsync();
                    using var reader = new StreamReader(stream);

                    bool stop = false;
                    do
                    {
                        ret = new ResponseDTO<CreateCompletionResponse>();
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
                                    ret.OpenAIResponse = JsonSerializer.Deserialize<CreateCompletionResponse>(line);
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
