using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DevExtremeAI.OpenAIDTO
{
    public class FileData
    {
        [JsonPropertyName("id")]
        public string FileId { get; set; }

        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("bytes")]
        public int Bytes { get; set; }

        [JsonPropertyName("created_at")]
        public long CreatedAt { get; set; }

        [JsonPropertyName("filename")]
        public string FileName { get; set; }

        [JsonPropertyName("purpose")]
        public string Purpose { get; set; }
    }

    public class FileDataListResponse
    {
        [JsonPropertyName("data")]
        public List<FileData> FileList { get; set; }

        [JsonPropertyName("object")]
        public string Object { get; set; }
    }

    public class BaseUploadFileRequest
    {
        /// <summary>
        /// Name of the JSON Lines file to be uploaded.
        /// </summary>
        [JsonPropertyName("file")]
        public byte[] File { get; set; }

        [JsonPropertyName("filename")]
        public string FileName { get; set; }

    }

    public class UploadFineTuningFileRequest : BaseUploadFileRequest
    {
        /// <summary>
        /// Each line must be a JSON record with "prompt" and "completion" fields representing your training examples.
        /// Allows us to validate the format of the uploaded file.
        /// </summary>
        [JsonPropertyName("purpose")]
        public string Purpose => "fine-tune";
    }

    public class UploadFileRequest : BaseUploadFileRequest
    {
        /// <summary>
        /// The intended purpose of the uploaded documents.
        /// </summary>
        [JsonPropertyName("purpose")]
        public string Purpose { get; set; }
    }

    public class DeleteFileRequest
    {
        /// <summary>
        /// The ID of the file to use for this request
        /// </summary>
        [JsonPropertyName("file_id")]
        public string FileId { get; set; }
    }

    public class DeleteFileResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("deleted")]
        public bool IsDeleted { get; set; }

    }

    public class RetrieveFileDataRequest
    {
        /// <summary>
        /// The ID of the file to use for this request
        /// </summary>
        [JsonPropertyName("file_id")]
        public string FileId { get; set; }
    }

    public class RetrieveFileContentRequest : RetrieveFileDataRequest
    {

    }

    public class RetrieveFileContentResponse
    {
        [JsonPropertyName("file")]
        public byte[] FileContent { get; set; }
    }

}
