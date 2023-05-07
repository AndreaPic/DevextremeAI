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
        private const string filesPath = "files";

        /// <summary>
        /// Returns a list of files that belong to the user's organization.
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDTO<FileDataListResponse>> GetFilesDataAsync()
        {
            ResponseDTO<FileDataListResponse> ret = new ResponseDTO<FileDataListResponse>();
            HttpClient httpClient = HttpClientFactory.CreateClient();
            FillBaseAddress(httpClient);
            FillAuthRequestHeaders(httpClient.DefaultRequestHeaders);
            var httpResponse = await httpClient.GetAsync(filesPath);
            if (httpResponse.IsSuccessStatusCode)
            {
                ret.OpenAIResponse = await httpResponse.Content.ReadFromJsonAsync<FileDataListResponse>();
            }
            else
            {
                ret.Error = await httpResponse.Content.ReadFromJsonAsync<ErrorResponse>();
            }
            return ret;
        }

        /// <summary>
        /// Upload a file that contains document(s) to be used across various endpoints/features.
        /// Currently, the size of all the files uploaded by one organization can be up to 1 GB.
        /// Please contact us if you need to increase the storage limit.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ResponseDTO<FileData>> UploadFileAsync(UploadFileRequest request)
        {
            ResponseDTO<FileData> ret = new ResponseDTO<FileData>();
            HttpClient httpClient = HttpClientFactory.CreateClient();
            FillBaseAddress(httpClient);
            FillAuthRequestHeaders(httpClient.DefaultRequestHeaders);

            using (var content = new MultipartFormDataContent())
            {

                content.Add(new ByteArrayContent(request.File), "file", request.FileName); //TODO: add enum for file type

                if (!string.IsNullOrEmpty(request.Purpose))
                {
                    content.Add(new StringContent(request.Purpose), "purpose");
                }


                var httpResponse = await httpClient.PostAsync(filesPath, content);
                if (httpResponse.IsSuccessStatusCode)
                {
                    ret.OpenAIResponse = await httpResponse.Content.ReadFromJsonAsync<FileData>();
                }
                else
                {
                    ret.Error = await httpResponse.Content.ReadFromJsonAsync<ErrorResponse>();
                }

                return ret;
            }
        }

        /// <summary>
        /// Upload a file that contains document(s) to be used across various endpoints/features.
        /// Currently, the size of all the files uploaded by one organization can be up to 1 GB.
        /// Please contact us if you need to increase the storage limit.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ResponseDTO<FileData>> UploadFineTuningFileAsync(UploadFineTuningFileRequest request)
        {
            ResponseDTO<FileData> ret = new ResponseDTO<FileData>();
            HttpClient httpClient = HttpClientFactory.CreateClient();
            FillBaseAddress(httpClient);
            FillAuthRequestHeaders(httpClient.DefaultRequestHeaders);

            using (var content = new MultipartFormDataContent())
            {

                content.Add(new ByteArrayContent(request.File), "file", request.FileName); //TODO: add enum for file type

                content.Add(new StringContent(request.Purpose), "purpose");


                var httpResponse = await httpClient.PostAsync(filesPath, content);
                if (httpResponse.IsSuccessStatusCode)
                {
                    ret.OpenAIResponse = await httpResponse.Content.ReadFromJsonAsync<FileData>();
                }
                else
                {
                    ret.Error = await httpResponse.Content.ReadFromJsonAsync<ErrorResponse>();
                }

                return ret;
            }
        }

        /// <summary>
        /// Delete a file.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ResponseDTO<DeleteFileResponse>> DeleteFileAsync(DeleteFileRequest request)
        {
            ResponseDTO<DeleteFileResponse> ret = new ResponseDTO<DeleteFileResponse>();
            HttpClient httpClient = HttpClientFactory.CreateClient();
            FillBaseAddress(httpClient);
            FillAuthRequestHeaders(httpClient.DefaultRequestHeaders);

            var httpResponse = await httpClient.DeleteAsync($"{filesPath}/{request.FileId}");


            if (httpResponse.IsSuccessStatusCode)
            {
                ret.OpenAIResponse = await httpResponse.Content.ReadFromJsonAsync<DeleteFileResponse>();
            }
            else
            {
                ret.Error = await httpResponse.Content.ReadFromJsonAsync<ErrorResponse>();
            }

            return ret;
        }

        /// <summary>
        /// Returns information about a specific file.
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDTO<FileData>> GetFileDataAsync(RetrieveFileDataRequest request)
        {
            ResponseDTO<FileData> ret = new ResponseDTO<FileData>();
            HttpClient httpClient = HttpClientFactory.CreateClient();
            FillBaseAddress(httpClient);
            FillAuthRequestHeaders(httpClient.DefaultRequestHeaders);
            var httpResponse = await httpClient.GetAsync($"{filesPath}/{request.FileId}");
            if (httpResponse.IsSuccessStatusCode)
            {
                ret.OpenAIResponse = await httpResponse.Content.ReadFromJsonAsync<FileData>();
            }
            else
            {
                ret.Error = await httpResponse.Content.ReadFromJsonAsync<ErrorResponse>();
            }
            return ret;
        }

        /// <summary>
        /// Returns the contents of the specified file
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ResponseDTO<RetrieveFileContentResponse>> GetFileContentAsync(RetrieveFileContentRequest request)
        {
            ResponseDTO<RetrieveFileContentResponse> ret = new ResponseDTO<RetrieveFileContentResponse>();
            HttpClient httpClient = HttpClientFactory.CreateClient();
            FillBaseAddress(httpClient);
            FillAuthRequestHeaders(httpClient.DefaultRequestHeaders);
            var httpResponse = await httpClient.GetAsync($"{filesPath}/{request.FileId}/content");
            if (httpResponse.IsSuccessStatusCode)
            {
                ret.OpenAIResponse = new RetrieveFileContentResponse();
                ret.OpenAIResponse.FileContent = await httpResponse.Content.ReadAsByteArrayAsync();
            }
            else
            {
                ret.Error = await httpResponse.Content.ReadFromJsonAsync<ErrorResponse>();
            }
            return ret;
        }

    }
}
