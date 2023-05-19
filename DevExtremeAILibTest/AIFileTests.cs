using System.Text;
using Microsoft.Extensions.DependencyInjection;
using DevExtremeAI.OpenAIDTO;
using DevExtremeAI.OpenAIClient;


namespace DevExtremeAILibTest
{
    public class AIFileTests : IClassFixture<TestApplication>
    {
        private readonly TestApplication _factory;

        public AIFileTests(TestApplication factory)
        {
            _factory = factory;
        }

        private const string fileDataTestName = "TestFile.jsonl";

        [Fact]
        public async Task FileTest()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var openAiapiClient = scope.ServiceProvider.GetService<IOpenAIAPIClient>();

                var fileDataList = await openAiapiClient.GetFilesDataAsync();

                Assert.False(fileDataList.HasError,fileDataList?.ErrorResponse?.Error?.Message);

                var testFileData = fileDataList.OpenAIResponse.FileList.FirstOrDefault(f => f.FileName == fileDataTestName);

                ResponseDTO<DeleteFileResponse> deleteResponse = null;
                if (testFileData!=null)
                {
                    deleteResponse = await openAiapiClient.DeleteFileAsync(new DeleteFileRequest() { FileId = testFileData.FileId });
                    Assert.False(deleteResponse.HasError);
                }

                var uploadResponse = await openAiapiClient.UploadFileAsync(new UploadFileRequest()
                    {
                        File = Encoding.UTF8.GetBytes(Resources.Resource.TestFileData),
                        FileName = fileDataTestName, 
                        Purpose = "fine-tune"
                }
                );
                Assert.False(uploadResponse.HasError);

                fileDataList = await openAiapiClient.GetFilesDataAsync();
                Assert.False(fileDataList.HasError);

                testFileData = fileDataList.OpenAIResponse.FileList.FirstOrDefault(f => f.FileName == fileDataTestName);
                Assert.NotNull(testFileData);


                var fileDataResponse = await openAiapiClient.GetFileDataAsync(new RetrieveFileDataRequest() 
                {
                    FileId = testFileData.FileId
                });

                Assert.False(fileDataResponse.HasError);

                var fileContentResponse = await openAiapiClient.GetFileContentAsync(new RetrieveFileContentRequest()
                {
                    FileId = fileDataResponse.OpenAIResponse.FileId
                });
                Assert.True(fileContentResponse.HasError);

                await Task.Delay(22000);

                deleteResponse = await openAiapiClient.DeleteFileAsync(new DeleteFileRequest() { FileId = fileDataResponse.OpenAIResponse.FileId });
                Assert.False(deleteResponse.HasError,deleteResponse?.ErrorResponse?.Error?.Message);

                TestUtility testUtility = new TestUtility(_factory);
                await testUtility.ClearAllTestFiles();

            }
        }
    }
}