using System.Diagnostics;
using DevExtremeAI.Settings;
using DevExtremeToys.JSon;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using System.Text.Json;
using Newtonsoft.Json;
using DevExtremeAI.OpenAIDTO;
using DevExtremeAI.OpenAIClient;
using DevExtremeToys.Serialization;
using System.Text;
using DevExtremeToys.Strings;

namespace DevextremeAILibTest
{
    public class AIFineTuningTests : IClassFixture<TestApplication>
    {
        private readonly TestApplication _factory;

        public AIFineTuningTests(TestApplication factory)
        {
            _factory = factory;
        }

        private const string andreaFineTuningDataFleName = "AndreaTuningDataCompletion.json";
        [Fact]
        public async Task CreateTuningTest()
        {

            using (var scope = _factory.Services.CreateScope())
            {
                var openAiapiClient = scope.ServiceProvider.GetService<IOpenAIAPIClient>();

                var fileDataList = await openAiapiClient.GetFilesDataAsync();

                Assert.False(fileDataList.HasError);

                var testFileData = fileDataList.OpenAIResponse.FileList.FirstOrDefault(f => f.FileName == andreaFineTuningDataFleName);

                ResponseDTO<DeleteFileResponse> deleteResponse = null;
                if (testFileData != null)
                {
                    deleteResponse = await openAiapiClient.DeleteFileAsync(new DeleteFileRequest() { FileId = testFileData.FileId });
                    Assert.False(deleteResponse.HasError);
                }

                //var fileContent = Resources.Resource.AndreaTuningDataCompletion.Replace("\r\n", "\\n");
                var fileContent = Resources.Resource.AndreaTuningDataCompletion;


                var uploadResponse = await openAiapiClient.UploadFileAsync(new UploadFileRequest()
                {
                    File = Encoding.UTF8.GetBytes(fileContent),
                    FileName = andreaFineTuningDataFleName,
                    Purpose = "fine-tune",
                }
                );
                Assert.False(uploadResponse.HasError);

                //string trainingFIle = uploadResponse.OpenAIResponse.FileId;
                string trainingFIle = "";

                var createFineTuneJobResponse = await openAiapiClient.CreateFineTuneJobAsync(new CreateFineTuneRequest()
                {
                    TrainingFile = trainingFIle,
                    Suffix = "andrea-dev-italy-cv2",
                    ModelId = "davinci"

                });

                Assert.False(createFineTuneJobResponse.HasError);

            }
        }

        [Fact]
        public async Task ListTuningTest()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var openAiapiClient = scope.ServiceProvider.GetService<IOpenAIAPIClient>();

                var fineTuneJobs = await openAiapiClient.GetFineTuneJobListAsync();
                Assert.False(fineTuneJobs.HasError);

                var fineTuneJob = await openAiapiClient.GetFineTuneJobDataAsync(new FineTuneRequest()
                    { FineTuneId = "ft-o6XrTtQowYCBAYzxHnTOF0Hi" });
                Assert.False(fineTuneJob.HasError);

            }
        }

        [Fact]
        public async Task RemoveTuningTest()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var openAiapiClient = scope.ServiceProvider.GetService<IOpenAIAPIClient>();

                var fineTuneJobs = await openAiapiClient.GetFineTuneJobListAsync();
                Assert.False(fineTuneJobs.HasError);

                var fineTuneId = "davinci:ft-devextreme:andrea-dev-italy-cv2-2023-05-11-19-33-23";


                var fineTuneJob = await openAiapiClient.GetFineTuneJobDataAsync(new FineTuneRequest() { FineTuneId = "ft-KL8mNz3uHNb5qb3fojE5oQqy" });
                Assert.False(fineTuneJob.HasError);
                var removed = await openAiapiClient.DeleteFineTuneModelAsync(new FineTuneRequest()
                    { FineTuneId = fineTuneJob.OpenAIResponse.ModelId }); //"davinci:ft-devextreme:andrea-dev-italy-cv1-2023-05-11-19-33-23" });
                Assert.False(removed.HasError);
            }
        }

        [Fact]
        public async Task AskForAndreaTuningTest()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var openAiapiClient = scope.ServiceProvider.GetService<IOpenAIAPIClient>();

                CreateCompletionRequest createCompletionRequest = new CreateCompletionRequest();
                createCompletionRequest.ModelID = "davinci:ft-devextreme:andrea-dev-italy-cv2-2023-05-12-20-06-16";
                string prompt = "Di cosa si è occupato Andrea Piccioni in NTS Informatica tra l'anno 2020 e 2023?";// + "\\n\\n###\\n\\n";
                createCompletionRequest.AddCompletionPrompt(prompt);
                //createCompletionRequest.AddCompletionPrompt("In una breve frase dimmi quali sono le competenze di Andrea Piccioni in N.T.S. Informatica###");
                createCompletionRequest.MaxTokens = 400;
                createCompletionRequest.Temperature = 0;
                //createCompletionRequest.Stop = "###";

                var completionResponse = await openAiapiClient.CreateCompletionAsync(createCompletionRequest);

                Debug.WriteLine(completionResponse.OpenAIResponse.Choices[0].Text);

                //createCompletionRequest = new CreateCompletionRequest();
                //createCompletionRequest.ModelID = "davinci:ft-devextreme:andrea-dev-italy-cv1-2023-05-11-19-33-23";
                //createCompletionRequest.AddCompletionPrompt("che cos'è NTS Cloud Tech?");
                //createCompletionRequest.MaxTokens = 100;
                //createCompletionRequest.Temperature = 0;

                //completionResponse = await openAiapiClient.CreateCompletionAsync(createCompletionRequest);



                //CreateCompletionRequest createCompletionRequest = new CreateCompletionRequest();
                //createCompletionRequest.ModelID = "curie:ft-devextreme:andrea-dev-italy-2023-05-10-20-14-03";
                //createCompletionRequest.AddCompletionPrompt("Cos'è NTS Cloud Tech in poche parole?");
                //createCompletionRequest.MaxTokens = 100;
                //createCompletionRequest.Temperature = 0;

                //var completionResponse = await openAiapiClient.CreateCompletionAsync(createCompletionRequest);

                //Assert.False(completionResponse.HasError);

                //createCompletionRequest = new CreateCompletionRequest();
                //createCompletionRequest.ModelID = "curie:ft-devextreme:andrea-dev-italy-2023-05-10-20-14-03";
                //createCompletionRequest.AddCompletionPrompt("Di cosa si occupa Andrea in N.T.S. Informatica?");
                //createCompletionRequest.MaxTokens = 100;
                //createCompletionRequest.Temperature = 0;

                //completionResponse = await openAiapiClient.CreateCompletionAsync(createCompletionRequest);

                //Assert.False(completionResponse.HasError);

                //createCompletionRequest = new CreateCompletionRequest();
                //createCompletionRequest.ModelID = "curie:ft-devextreme:andrea-dev-italy-2023-05-10-20-14-03";
                //createCompletionRequest.AddCompletionPrompt("Che progetti ha guidato Andrea in N.T.S. Informatica?");
                //createCompletionRequest.MaxTokens = 100;
                ////createCompletionRequest.TopP = 0.1f;
                //createCompletionRequest.Temperature = 0;
                ////createCompletionRequest.Stop = "###";

                //completionResponse = await openAiapiClient.CreateCompletionAsync(createCompletionRequest);

                //Assert.False(completionResponse.HasError);

            }
        }

    }
}

