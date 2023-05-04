using DevextremeAI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DevextremeAI.Communication
{
    public class CreateImageRequest
    {
        /// <summary>
        /// A text description of the desired image(s). The maximum length is 1000 characters.
        /// </summary>
        [JsonPropertyName("prompt")]
        public string Prompt { get; set; }

        /// <summary>
        /// The number of images to generate. Must be between 1 and 10.
        /// </summary>
        [JsonPropertyName("n")]
        public int? N { get; set; }

        /// <summary>
        /// The size of the generated images. Must be one of `256x256`, `512x512`, or `1024x1024`.
        /// </summary>
        [JsonPropertyName("size")]
        public CreateImageRequestSizeEnum? Size { get; set; }

        /// <summary>
        /// The format in which the generated images are returned. Must be one of `url` or `b64_json`.
        /// </summary>
        [JsonPropertyName("response_format")]
        public CreateImageRequestResponseFormatEnum? ResponseFormat { get; set; }

        /// <summary>
        /// A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.
        /// [Learn more](/docs/guides/safety-best-practices/end-user-ids).
        /// </summary>
        [JsonPropertyName("user")]
        public string? User { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverterEx<CreateImageRequestSizeEnum>))]
    public enum CreateImageRequestSizeEnum 
    {
        [EnumMember(Value = "256x256")]
        _256x256 = 0,
        [EnumMember(Value = "512x512")]
        _512x512 = 1,
        [EnumMember(Value = "1024x1024")]
        _1024x1024 = 2
    }

    [JsonConverter(typeof(JsonStringEnumConverterEx<CreateImageRequestResponseFormatEnum>))]
    public enum CreateImageRequestResponseFormatEnum
    {
        [EnumMember(Value = "url")]
        Url = 0,
        [EnumMember(Value = "b64_json")]
        B64Json = 1
    }

    public class ImagesResponse
    {
        [JsonPropertyName("created")]
        public double Created { get; set; }

        [JsonPropertyName("data")]
        public List<ImagesResponseDataInner> Data { get; set; }
    }

    public class ImagesResponseDataInner
    {
        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("b64_json")]
        public byte[]? B64Json { get; set; }
    }

    public class CreateImageEditRequest
    {
        /// <summary>
        /// The image to edit.
        /// Must be a valid PNG file, less than 4MB, and square.
        /// If mask is not provided, image must have transparency, which will be used as the mask.
        /// </summary>
        [JsonPropertyName("image")]
        public byte[] Image { get; set; }

        /// <summary>
        /// An additional image whose fully transparent areas (e.g. where alpha is zero) indicate where image should be edited.
        /// Must be a valid PNG file, less than 4MB, and have the same dimensions as image.
        /// </summary>
        [JsonPropertyName("mask")]
        public byte[]? Mask { get; set; }

        /// <summary>
        /// A text description of the desired image(s). The maximum length is 1000 characters.
        /// </summary>
        [JsonPropertyName("prompt")]
        public string Prompt { get; set; }

        /// <summary>
        /// The number of images to generate. Must be between 1 and 10.
        /// </summary>
        [JsonPropertyName("n")]
        public int? N { get; set; }

        /// <summary>
        /// The size of the generated images. Must be one of 256x256, 512x512, or 1024x1024
        /// </summary>
        [JsonPropertyName("size")]
        public CreateImageRequestSizeEnum? Size { get; set; }

        /// <summary>
        /// The format in which the generated images are returned. Must be one of url or b64_json
        /// </summary>
        [JsonPropertyName("response_format")]
        public CreateImageRequestResponseFormatEnum? ResponseFormat { get; set; }

        /// <summary>
        /// A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse. 
        /// </summary>
        [JsonPropertyName("user")]
        public string? User { get; set; }
    }

    public class CreateImageVariationsRequest
    {

        [JsonPropertyName("prompt")]
        public string Prompt { get; set; }

        /// <summary>
        /// The image to use as the basis for the variation(s). Must be a valid PNG file, less than 4MB, and square.
        /// </summary>
        [JsonPropertyName("image")]
        public byte[] Image { get; set; }

        /// <summary>
        /// The number of images to generate. Must be between 1 and 10.
        /// </summary>
        [JsonPropertyName("n")]
        public int? N { get; set; }

        /// <summary>
        /// The size of the generated images. Must be one of 256x256, 512x512, or 1024x1024
        /// </summary>
        [JsonPropertyName("size")]
        public CreateImageRequestSizeEnum? Size { get; set; }

        /// <summary>
        /// The format in which the generated images are returned. Must be one of url or b64_json
        /// </summary>
        [JsonPropertyName("response_format")]
        public CreateImageRequestResponseFormatEnum? ResponseFormat { get; set; }

        /// <summary>
        /// A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse. 
        /// </summary>
        [JsonPropertyName("user")]
        public string? User { get; set; }

    }

}
