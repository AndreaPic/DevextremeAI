using System.Diagnostics;
using System.Linq.Expressions;
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

        private const string fineTuningDataFleName = "Test-Trivia-Tune.json";

        [Fact]
        public async Task FileTuningWithCancelTest()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var openAiapiClient = scope.ServiceProvider.GetService<IOpenAIAPIClient>();

                var fileDataList = await openAiapiClient.GetFilesDataAsync();

                Assert.False(fileDataList.HasError);

                var testFileData = fileDataList.OpenAIResponse.FileList.FirstOrDefault(f => f.FileName == fineTuningDataFleName);

                ResponseDTO<DeleteFileResponse> deleteResponse = null;
                if (testFileData != null)
                {
                    deleteResponse = await openAiapiClient.DeleteFileAsync(new DeleteFileRequest() { FileId = testFileData.FileId });
                    Assert.False(deleteResponse.HasError);
                }

                var fileContent = Resources.Resource.Trivia_Tune;


                var uploadResponse = await openAiapiClient.UploadFineTuningFileAsync(new UploadFineTuningFileRequest()
                {
                    File = Encoding.UTF8.GetBytes(fileContent),
                    FileName = fineTuningDataFleName,
                }
                );
                Assert.False(uploadResponse.HasError);

                var createFineTuneJobResponse = await openAiapiClient.CreateFineTuneJobAsync(new CreateFineTuneRequest()
                {
                    TrainingFile = uploadResponse.OpenAIResponse.FileId,
                    Suffix = "andrea-dev-italy-trivia-tune",
                    Model = "davinci"

                });
                Assert.False(createFineTuneJobResponse.HasError, createFineTuneJobResponse?.ErrorResponse?.Error?.Message);

                var fineTuneList = await openAiapiClient.GetFineTuneJobListAsync();
                Assert.False(fineTuneList.HasError);

                try
                {
                    //with free subscription sometimes connection stream breaks
                    await foreach (var eventData in openAiapiClient.GetFineTuneEventStreamAsync(new FineTuneRequest() { FineTuneId = createFineTuneJobResponse.OpenAIResponse.Id }))
                    {
                        Assert.NotNull(eventData.Object);
                        Debug.WriteLine(eventData.Message);
                        break;
                    }
                }
                catch{}

                var cancelJob = await openAiapiClient.CancelFineTuneJobAsync(new FineTuneRequest() { FineTuneId = createFineTuneJobResponse.OpenAIResponse.Id });
                Assert.False(cancelJob.HasError);

                TestUtility testUtility = new TestUtility(_factory);
                await testUtility.ClearAllTestJobsAndModels();
            }
        }

        //[Fact]
        public async Task FileTuningWithDeleteTest()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var openAiapiClient = scope.ServiceProvider.GetService<IOpenAIAPIClient>();

                var fileDataList = await openAiapiClient.GetFilesDataAsync();

                Assert.False(fileDataList.HasError);

                var testFileData = fileDataList.OpenAIResponse.FileList.FirstOrDefault(f => f.FileName == fineTuningDataFleName);

                ResponseDTO<DeleteFileResponse> deleteResponse = null;
                if (testFileData != null)
                {
                    deleteResponse = await openAiapiClient.DeleteFileAsync(new DeleteFileRequest() { FileId = testFileData.FileId });
                    Assert.False(deleteResponse.HasError);
                    await Task.Delay(22000);
                }

                var fileContent = Resources.Resource.Trivia_Tune;


                var uploadResponse = await openAiapiClient.UploadFineTuningFileAsync(new UploadFineTuningFileRequest()
                {
                    File = Encoding.UTF8.GetBytes(fileContent),
                    FileName = fineTuningDataFleName,
                }
                );
                Assert.False(uploadResponse.HasError);
                await Task.Delay(22000);

                var createFineTuneJobResponse = await openAiapiClient.CreateFineTuneJobAsync(new CreateFineTuneRequest()
                {
                    TrainingFile = uploadResponse.OpenAIResponse.FileId,
                    Suffix = "andrea-dev-italy-trivia-tune",
                });
                Assert.False(createFineTuneJobResponse.HasError,createFineTuneJobResponse?.ErrorResponse?.Error?.Message);

                var fineTuneList = await openAiapiClient.GetFineTuneJobListAsync();
                Assert.False(fineTuneList.HasError);


                try
                {
                    //with free subscription sometimes connection stream breaks
                    await foreach (var eventData in openAiapiClient.GetFineTuneEventStreamAsync(new FineTuneRequest()
                                       { FineTuneId = createFineTuneJobResponse.OpenAIResponse.Id }))
                    {
                        Assert.NotNull(eventData.Object);
                        Debug.WriteLine(eventData.Message);
                    }
                }catch{}

                var eventsData = await openAiapiClient.GetFineTuneEventListAsync(new FineTuneRequest() { FineTuneId = createFineTuneJobResponse.OpenAIResponse.Id });
                Assert.False(eventsData.HasError);

                var fineTuneData = await openAiapiClient.GetFineTuneJobDataAsync(new FineTuneRequest() { FineTuneId = createFineTuneJobResponse.OpenAIResponse.Id });
                Assert.False(fineTuneData.HasError);

                await Task.Delay(30000);

                if (!string.IsNullOrEmpty(fineTuneData?.OpenAIResponse?.FineTunedModel))
                {
                    var removed = await openAiapiClient.DeleteFineTuneModelAsync(new FineTuneRequest()
                        { FineTuneId = fineTuneData.OpenAIResponse.FineTunedModel });
                    Assert.False(removed.HasError, removed?.ErrorResponse?.Error?.Message);
                }

                TestUtility testUtility = new TestUtility(_factory);
                await testUtility.ClearAllTestJobsAndModels();
            }
        }

    }
}

