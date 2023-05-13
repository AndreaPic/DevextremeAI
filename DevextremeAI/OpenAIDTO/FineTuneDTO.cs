using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Formats.Asn1.AsnWriter;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DevExtremeAI.OpenAIDTO
{
    public class CreateFineTuneRequest
    {
        /// <summary>
        /// The ID of an uploaded file that contains training data.
        /// See upload file for how to upload a file.
        /// Your dataset must be formatted as a JSONL file, where each training example is a JSON object with the keys "prompt" and "completion".
        /// Additionally, you must upload your file with the purpose fine-tune.
        /// See the fine-tuning guide for more details.
        /// </summary>
        [JsonPropertyName("training_file")]
        public string TrainingFile { get; set; }

        /// <summary>
        /// The ID of an uploaded file that contains validation data.
        /// If you provide this file, the data is used to generate validation metrics periodically during fine-tuning.
        /// These metrics can be viewed in the fine-tuning results file.
        /// Your train and validation data should be mutually exclusive.
        /// Your dataset must be formatted as a JSONL file, where each validation example is a JSON object with the keys "prompt" and "completion".
        /// Additionally, you must upload your file with the purpose fine-tune.
        /// See the fine-tuning guide for more details.
        /// </summary>
        [JsonPropertyName("validation_file")]
        public string? ValidationFile { get; set; }

        /// <summary>
        /// The name of the base model to fine-tune.
        /// You can select one of "ada", "babbage", "curie", "davinci", or a fine-tuned model created after 2022-04-21.
        /// To learn more about these models, see the Models documentation.
        /// The default is "curie"
        /// </summary>
        [JsonPropertyName("model")]
        public string? ModelId { get; set; }

        /// <summary>
        /// The number of epochs to train the model for.
        /// An epoch refers to one full cycle through the training dataset.
        /// The default is 4
        /// </summary>
        [JsonPropertyName("n_epochs")]
        public int? NEpochs { get; set; }

        /// <summary>
        /// The batch size to use for training.
        /// The batch size is the number of training examples used to train a single forward and backward pass.
        /// y default, the batch size will be dynamically configured to be ~0.2% of the number of examples in the training set,
        /// capped at 256 - in general, we've found that larger batch sizes tend to work better for larger datasets
        /// </summary>
        [JsonPropertyName("batch_size")]
        public int? BatchSize { get; set; }

        /// <summary>
        /// The learning rate multiplier to use for training.
        /// The fine-tuning learning rate is the original learning rate used for pretraining multiplied by this value.
        /// By default, the learning rate multiplier is the 0.05, 0.1, or 0.2 depending on final batch_size
        /// (larger learning rates tend to perform better with larger batch sizes). We recommend experimenting with values in the range 0.02 to 0.2 to see what produces the best results.
        /// </summary>
        [JsonPropertyName("learning_rate_multiplier")]
        public double? LearningRateMultiplier { get; set; }

        /// <summary>
        /// The weight to use for loss on the prompt tokens.
        /// This controls how much the model tries to learn to generate the prompt (as compared to the completion which always has a weight of 1.0), and can add a stabilizing effect to training when completions are short.
        /// If prompts are extremely long (relative to completions), it may make sense to reduce this weight so as to avoid over-prioritizing learning the prompt.
        /// </summary>
        [JsonPropertyName("PromptLossWeight")]
        public double? prompt_loss_weight { get; set; }

        /// <summary>
        /// If set, we calculate classification-specific metrics such as accuracy and F-1 score using the validation set at the end of every epoch.
        /// These metrics can be viewed in the results file.
        /// In order to compute classification metrics, you must provide a validation_file.
        /// Additionally, you must specify
        /// classification_n_classes for multiclass classification or
        /// classification_positive_class for binary classification.
        /// </summary>
        [JsonPropertyName("compute_classification_metrics")]
        public bool? ComputeClassificationMetrics { get; set; }

        /// <summary>
        /// The number of classes in a classification task.
        /// This parameter is required for multiclass classification.
        /// </summary>
        [JsonPropertyName("classification_n_classes")]
        public int? ClassificationNClasses { get; set; }

        /// <summary>
        /// The positive class in binary classification.
        /// This parameter is needed to generate precision, recall, and F1 metrics when doing binary classification.
        /// </summary>
        [JsonPropertyName("classification_positive_class")]
        public string? ClassificationPositiveClass { get; set; }

        /// <summary>
        /// If this is provided, we calculate F-beta scores at the specified beta values.
        /// The F-beta score is a generalization of F-1 score.
        /// This is only used for binary classification.
        /// With a beta of 1 (i.e.the F-1 score), precision and recall are given the same weight.
        /// A larger beta score puts more weight on recall and less on precision.
        /// A smaller beta score puts more weight on precision and less on recall.
        /// </summary>
        [JsonPropertyName("classification_betas")]
        public List<double>? ClassificationBetas { get; set; }

        /// <summary>
        /// A string of up to 40 characters that will be added to your fine-tuned model name.
        /// For example, a suffix of "custom-model-name" would produce a model name like ada:ft-your-org:custom-model-name-2022-02-15-04-21-04.
        /// </summary>
        [JsonPropertyName("suffix")]
        public string? Suffix { get; set; }
    }

    public class FineTuneData
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("model")]
        public string ModelId { get; set; }

        [JsonPropertyName("created_at")]
        public long CreatedAt { get; set; }

        [JsonPropertyName("events")]
        public List<Event> Events { get; set; }

        [JsonPropertyName("fine_tuned_model")]
        public string FineTunedModel { get; set; }

        [JsonPropertyName("hyperparams")]
        public HyperParam? HyperParams { get; set; }

        [JsonPropertyName("organization_id")]
        public string OrganizationId { get; set; }

        [JsonPropertyName("result_files")]
        public List<FileData>? ResultFiles { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("validation_files")]
        public List<FileData>? ValidationFiles { get; set; }

        [JsonPropertyName("training_files")]
        public List<FileData>? TrainingFiles { get; set; }

        [JsonPropertyName("updated_at")]
        public long UpdatedAt { get; set; }
    }

    public class Event
    {
        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("created_at")]
        public long CreatedAt { get; set; }

        [JsonPropertyName("level")]
        public string Level { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

    }

    public class HyperParam
    {
        [JsonPropertyName("batch_size")]
        public int? BatchSize { get; set; }

        [JsonPropertyName("learning_rate_multiplier")]
        public double? LearningRateMultiplier { get; set; }

        [JsonPropertyName("n_epochs")]
        public int? NEpochs { get; set; }

        [JsonPropertyName("prompt_loss_weight")]
        public double? PromptLossWeight { get; set; }

    }

    public class GetFineTuneListResponse
    {
        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("data")]
        public List<FineTuneData> Data { get; set; }
    }

    public class FineTuneRequest
    {
        [JsonPropertyName("fine_tune_id")]
        public string FineTuneId { get; set; }
    }

    public class GetFineTuneEventListResponse
    {
        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("data")]
        public List<Event> Data { get; set; }
    }

    public class DeleteFineTuneModelResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("deleted")]
        public bool Deleted { get; set; }

    }

}
