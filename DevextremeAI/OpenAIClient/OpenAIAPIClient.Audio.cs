using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using DevExtremeAI.OpenAIDTO;


namespace DevExtremeAI.OpenAIClient
{

    partial class OpenAIAPIClient 
    {


        /// <summary>
        /// Transcribes audio into the input language.
        /// </summary>
        /// <param name="request">DTO with request specs.</param>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public async Task<ResponseDTO<CreateTranscriptionsResponse>> CreateTranscriptionsAsync(CreateTranscriptionsRequest request)
        {
            ResponseDTO<CreateTranscriptionsResponse> ret = new ResponseDTO<CreateTranscriptionsResponse>();
            HttpClient httpClient = CreateHttpClient(out bool doDispose);
            try
            {
                FillBaseAddress(httpClient);
                FillAuthRequestHeaders(httpClient.DefaultRequestHeaders);

                using (var content = new MultipartFormDataContent())
                {
                    content.Add(new StringContent(request.Model), "model");

                    content.Add(new ByteArrayContent(request.File), "file", "audio.mp3"); //TODO: add enum for file type

                    if (!string.IsNullOrEmpty(request.Prompt))
                    {
                        content.Add(new StringContent(request.Prompt), "prompt");
                    }

                    if (request.ResponseFormat != null)
                    {
                        switch (request.ResponseFormat.Value)
                        {
                            case CreateTranscriptionsRequestEnum.Json:
                                content.Add(new StringContent("json"), "response_format");
                                break;
                            case CreateTranscriptionsRequestEnum.Text:
                                content.Add(new StringContent("text"), "response_format");
                                break;
                            case CreateTranscriptionsRequestEnum.Srt:
                                content.Add(new StringContent("srt"), "response_format");
                                break;
                            case CreateTranscriptionsRequestEnum.VerboseJson:
                                content.Add(new StringContent("verbose_json"), "response_format");
                                break;
                            case CreateTranscriptionsRequestEnum.Vtt:
                                content.Add(new StringContent("vtt"), "response_format");
                                break;
                        }
                    }

                    if (request.Temperature!=null)
                    {
                        content.Add(new StringContent(request.Temperature.ToString()), "temperature");
                    }

                    if (!string.IsNullOrEmpty(request.Language))
                    {
                        content.Add(new StringContent(request.Language), "language");
                    }

                    var httpResponse = await httpClient.PostAsync("audio/transcriptions", content);
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        ret.OpenAIResponse = await httpResponse.Content.ReadFromJsonAsync<CreateTranscriptionsResponse>();
                    }
                    else
                    {
                        ret.Error = await httpResponse.Content.ReadFromJsonAsync<ErrorResponse>();
                    }
                    return ret;
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
        /// Translates audio into into English.
        /// </summary>
        /// <param name="request">DTO with request specs.</param>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public async Task<ResponseDTO<CreateTranslationsResponse>> CreateTranslationsAsync(CreateTranslationsRequest request)
        {
            ResponseDTO<CreateTranslationsResponse> ret = new ResponseDTO<CreateTranslationsResponse>();
            HttpClient httpClient = CreateHttpClient(out bool doDispose);
            try
            {
                FillBaseAddress(httpClient);
                FillAuthRequestHeaders(httpClient.DefaultRequestHeaders);

                using (var content = new MultipartFormDataContent())
                {
                    content.Add(new StringContent(request.Model), "model");

                    content.Add(new ByteArrayContent(request.File), "file", "audio.mp3"); //TODO: add enum for file type

                    if (!string.IsNullOrEmpty(request.Prompt))
                    {
                        content.Add(new StringContent(request.Prompt), "prompt");
                    }

                    if (request.ResponseFormat != null)
                    {
                        switch (request.ResponseFormat.Value)
                        {
                            case CreateTranscriptionsRequestEnum.Json:
                                content.Add(new StringContent("json"), "response_format");
                                break;
                            case CreateTranscriptionsRequestEnum.Text:
                                content.Add(new StringContent("text"), "response_format");
                                break;
                            case CreateTranscriptionsRequestEnum.Srt:
                                content.Add(new StringContent("srt"), "response_format");
                                break;
                            case CreateTranscriptionsRequestEnum.VerboseJson:
                                content.Add(new StringContent("verbose_json"), "response_format");
                                break;
                            case CreateTranscriptionsRequestEnum.Vtt:
                                content.Add(new StringContent("vtt"), "response_format");
                                break;
                        }
                    }

                    if (request.Temperature != null)
                    {
                        content.Add(new StringContent(request.Temperature.ToString()), "temperature");
                    }


                    var httpResponse = await httpClient.PostAsync("audio/translations", content);
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        ret.OpenAIResponse = await httpResponse.Content.ReadFromJsonAsync<CreateTranslationsResponse>();
                    }
                    else
                    {
                        ret.Error = await httpResponse.Content.ReadFromJsonAsync<ErrorResponse>();
                    }

                    return ret;
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
