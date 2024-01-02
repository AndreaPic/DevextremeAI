using DevExtremeAI.OpenAIClient;
using DevExtremeAI.OpenAIDTO;
using Microsoft.Extensions.DependencyInjection;

namespace DevExtremeAILibTest
{
    internal class TestUtility
    {
        private readonly TestApplication _factory;
        public TestUtility(TestApplication factory)
        {
            _factory = factory;
        }

        internal async Task ClearAllTestFiles()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var openAiapiClient = scope.ServiceProvider.GetService<IOpenAIAPIClient>();
                var files = await openAiapiClient.GetFilesDataAsync();
                foreach (var file in files.OpenAIResponse.FileList.Where(f => f.FileName.StartsWith("Test-")))
                {
                    var deleted = await openAiapiClient.DeleteFileAsync(new DeleteFileRequest() { FileId = file.FileId });
                    Assert.False(deleted.HasError);
                }
            }
        }

        internal async Task ClearAllTestJobsAndModels()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var openAiapiClient = scope.ServiceProvider.GetService<IOpenAIAPIClient>();

                var fineTuneList = await openAiapiClient.GetFineTuneJobListAsync();
                Assert.False(fineTuneList.HasError);

                foreach (var fineTuneData in fineTuneList.OpenAIResponse.Data.Where(ft => !string.IsNullOrEmpty(ft?.FineTunedModel) && (ft.FineTunedModel.StartsWith("Test") || ft.FineTunedModel.Contains("trivia")) ))
                {

                    if (fineTuneData.ResultFiles != null)
                    {
                        foreach (var resultFile in fineTuneData.ResultFiles)
                        {
                            var deletedFile = await openAiapiClient.DeleteFileAsync(new DeleteFileRequest()
                            { FileId = resultFile });
                        }
                    }

                    if (fineTuneData.ValidationFile != null)
                    {
                        var deletedFile = await openAiapiClient.DeleteFileAsync(new DeleteFileRequest()
                        { FileId = fineTuneData.ValidationFile});
                    }

                    if (fineTuneData.TrainingFile != null)
                    {
                        var deletedFile = await openAiapiClient.DeleteFileAsync(new DeleteFileRequest()
                        { FileId = fineTuneData.TrainingFile });
                    }

                    if (!string.IsNullOrEmpty(fineTuneData.FineTunedModel))
                    {
                        var deletedJob = await openAiapiClient.DeleteFineTuneModelAsync(new FineTuneRequest()
                        { FineTuneId = fineTuneData.FineTunedModel });
                    }
                    else
                    {
                        if ((fineTuneData.Status != "cancelled") && (fineTuneData.Status != "failed"))
                        {
                            var canceled = await openAiapiClient.CancelFineTuneJobAsync(new FineTuneRequest()
                            { FineTuneId = fineTuneData.Id });
                        }
                    }
                }
            }
        }

    }
}
