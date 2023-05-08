using System.Runtime.CompilerServices;
using System.Text;
using DevextremeAI.Settings;
using DevExtremeToys.JSon;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using System.Text.Json;
using DevExtremeToys.Serialization;
using Newtonsoft.Json;
using DevextremeAI.Communication.DTO;
using DevextremeAI.Communication.APIClient;

namespace DevextremeAILibTest
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
        public async Task FileNOTuningTest()
        {

            using (var scope = _factory.Services.CreateScope())
            {
                var openAiapiClient = scope.ServiceProvider.GetService<IOpenAIAPIClient>();

                var fileDataList = await openAiapiClient.GetFilesDataAsync();

                Assert.False(fileDataList.HasError);

                var testFileData = fileDataList.OpenAIResponse.FileList.FirstOrDefault(f => f.FileName == fileDataTestName);

                ResponseDTO<DeleteFileResponse> deleteResponse = null;
                if (testFileData!=null)
                {
                    deleteResponse = await openAiapiClient.DeleteFileAsync(new DeleteFileRequest() { FileId = testFileData.FileId });
                    Assert.False(deleteResponse.HasError);
                }

                var x = Resources.Resource.TestFileData.ToUTF8ByteArray();
                var y = Encoding.UTF8.GetBytes(Resources.Resource.TestFileData);

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

                //var fileContentResponse = await openAiapiClient.GetFileContentAsync(new RetrieveFileContentRequest()
                //{
                //    FileId = fileDataResponse.OpenAIResponse.FileId
                //});
                //Assert.False(fileContentResponse.HasError);
                //Assert.NotNull(fileContentResponse.OpenAIResponse.FileContent);

                deleteResponse = await openAiapiClient.DeleteFileAsync(new DeleteFileRequest() { FileId = fileDataResponse.OpenAIResponse.FileId });
                Assert.False(deleteResponse.HasError);


            }
        }
    }
}