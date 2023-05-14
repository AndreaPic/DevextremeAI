using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using DevExtremeAI.OpenAIDTO;

namespace DevExtremeAI.OpenAIClient
{
    partial class OpenAIAPIClient 
    {
        /// <summary>
        /// Creates an image given a prompt.
        /// </summary>
        /// <param name="request">DTO with request specs.</param>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public async Task<ResponseDTO<ImagesResponse>> CreateImageAsync(CreateImageRequest request)
        {
            ResponseDTO<ImagesResponse> ret = new ResponseDTO<ImagesResponse>();
            HttpClient httpClient = CreateHttpClient(out bool doDispose);
            try
            {
                FillBaseAddress(httpClient);
                FillAuthRequestHeaders(httpClient.DefaultRequestHeaders);

                var jsonContent = CreateJsonStringContent(request);

                var httpResponse = await httpClient.PostAsync($"images/generations", jsonContent);
                if (httpResponse.IsSuccessStatusCode)
                {
                    ret.OpenAIResponse = await httpResponse.Content.ReadFromJsonAsync<ImagesResponse>();
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
        /// Creates an edited or extended image given an original image and a prompt.
        /// </summary>
        /// <param name="request">DTO with request specs.</param>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public async Task<ResponseDTO<ImagesResponse>> CreateImageEditAsync(CreateImageEditRequest request)
        {
            ResponseDTO<ImagesResponse> ret = new ResponseDTO<ImagesResponse>();
            HttpClient httpClient = CreateHttpClient(out bool doDispose);
            try
            {
                FillBaseAddress(httpClient);
                FillAuthRequestHeaders(httpClient.DefaultRequestHeaders);

                using (var content = new MultipartFormDataContent())
                {
                    content.Add(new ByteArrayContent(request.Image), "image", "image.png");

                    if (request.Mask != null)
                    {
                        content.Add(new ByteArrayContent(request.Mask), "mask", "mask.png");
                    }

                    content.Add(new StringContent(request.Prompt), "prompt");

                    if (request.N != null)
                    {
                        content.Add(new StringContent(request.N.ToString()), "n");
                    }

                    if (request.Size != null)
                    {
                        switch (request.Size.Value)
                        {
                            case CreateImageRequestSizeEnum._1024x1024:
                                content.Add(new StringContent("1024x1024"), "size");
                                break;
                            case CreateImageRequestSizeEnum._512x512:
                                content.Add(new StringContent("512x512"), "size");
                                break;
                            case CreateImageRequestSizeEnum._256x256:
                                content.Add(new StringContent("256x256"), "size");
                                break;
                        }
                    }

                    if (request.ResponseFormat != null)
                    {
                        switch (request.ResponseFormat.Value)
                        {
                            case CreateImageRequestResponseFormatEnum.B64Json:
                                content.Add(new StringContent("b64_json"), "response_format");
                                break;
                            case CreateImageRequestResponseFormatEnum.Url:
                                content.Add(new StringContent("url"), "response_format");
                                break;
                        }
                    }

                    if (!string.IsNullOrEmpty(request.User))
                    {
                        content.Add(new StringContent(request.User), "user");
                    }



                    var httpResponse = await httpClient.PostAsync("images/edits", content);
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        ret.OpenAIResponse = await httpResponse.Content.ReadFromJsonAsync<ImagesResponse>();
                    }
                    else
                    {
                        ret.ErrorResponse = await httpResponse.Content.ReadFromJsonAsync<ErrorResponse>() ?? ErrorResponse.CreateDefaultErrorResponse();
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
        /// Creates a variation of a given image. (BETA)
        /// </summary>
        /// <param name="request">DTO with request specs.</param>
        /// <returns>OpenAIResponse property contains the AI response, if an error occurs HasError is true and the Error property contains the complete error details.</returns>
        public async Task<ResponseDTO<ImagesResponse>> CreateImageVariationsAsync(CreateImageVariationsRequest request)
        {
            ResponseDTO<ImagesResponse> ret = new ResponseDTO<ImagesResponse>();
            HttpClient httpClient = CreateHttpClient(out bool doDispose);
            try
            {
                FillBaseAddress(httpClient);
                FillAuthRequestHeaders(httpClient.DefaultRequestHeaders);

                using (var content = new MultipartFormDataContent())
                {
                    content.Add(new ByteArrayContent(request.Image), "image", "image.png");

                    content.Add(new StringContent(request.Prompt), "prompt");

                    if (request.N != null)
                    {
                        content.Add(new StringContent(request.N.ToString()), "n");
                    }

                    if (request.Size != null)
                    {
                        switch (request.Size.Value)
                        {
                            case CreateImageRequestSizeEnum._1024x1024:
                                content.Add(new StringContent("1024x1024"), "size");
                                break;
                            case CreateImageRequestSizeEnum._512x512:
                                content.Add(new StringContent("512x512"), "size");
                                break;
                            case CreateImageRequestSizeEnum._256x256:
                                content.Add(new StringContent("256x256"), "size");
                                break;
                        }
                    }

                    if (request.ResponseFormat != null)
                    {
                        switch (request.ResponseFormat.Value)
                        {
                            case CreateImageRequestResponseFormatEnum.B64Json:
                                content.Add(new StringContent("b64_json"), "response_format");
                                break;
                            case CreateImageRequestResponseFormatEnum.Url:
                                content.Add(new StringContent("url"), "response_format");
                                break;
                        }
                    }

                    if (!string.IsNullOrEmpty(request.User))
                    {
                        content.Add(new StringContent(request.User), "user");
                    }

                    var httpResponse = await httpClient.PostAsync("images/edits", content);
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        ret.OpenAIResponse = await httpResponse.Content.ReadFromJsonAsync<ImagesResponse>();
                    }
                    else
                    {
                        ret.ErrorResponse = await httpResponse.Content.ReadFromJsonAsync<ErrorResponse>() ?? ErrorResponse.CreateDefaultErrorResponse();
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
