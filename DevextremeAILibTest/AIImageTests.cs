using DevextremeAI.Communication;
using DevextremeAI.Settings;
using DevExtremeToys.JSon;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using System.Text.Json;
using Newtonsoft.Json;

namespace DevextremeAILibTest
{
    public class AIImageTests : IClassFixture<TestApplication>
    {
        private readonly TestApplication _factory;

        public AIImageTests(TestApplication factory)
        {
            _factory = factory;
        }

        
        [Fact]
        public async Task CreateImageTest()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var openAiapiClient = scope.ServiceProvider.GetService<IOpenAIAPIClient>();
                CreateImageRequest request = new CreateImageRequest();
                request.Prompt = "friendly robot";
                request.N = 1;
                request.Size = CreateImageRequestSizeEnum._256x256;
                request.ResponseFormat = CreateImageRequestResponseFormatEnum.Url;
                

                var response = await openAiapiClient.CreateImageAsync(request);
                Assert.NotNull(response);
                Assert.NotNull(response.Data.FirstOrDefault());
            }
        }

        [Fact]
        public async Task CreateImageEditTest()
        {
            using (var scope = _factory.Services.CreateScope())
            {

                //var openAiapiClient = scope.ServiceProvider.GetService<IOpenAIAPIClient>();
                //CreateImageRequest requestCreateImage = new CreateImageRequest();
                //requestCreateImage.Prompt = "friendly robot";
                //requestCreateImage.N = 1;
                //requestCreateImage.Size = CreateImageRequestSizeEnum._256x256;
                //requestCreateImage.ResponseFormat = CreateImageRequestResponseFormatEnum.B64Json;

                //var responseCreateImage = await openAiapiClient.CreateImageAsync(requestCreateImage);
                //Assert.NotNull(responseCreateImage);
                //Assert.NotNull(responseCreateImage.Data.FirstOrDefault());


                var openAiapiClient = scope.ServiceProvider.GetService<IOpenAIAPIClient>();
                CreateImageEditRequest requestEditImage = new CreateImageEditRequest();
                requestEditImage.Image = Resources.Resource.pink_panther;
                requestEditImage.Prompt = "change ping to black";
                //requestEditImage.N = 1;
                requestEditImage.Size = CreateImageRequestSizeEnum._256x256;
                requestEditImage.ResponseFormat = CreateImageRequestResponseFormatEnum.Url;


                var response = await openAiapiClient.CreateImageEditAsync(requestEditImage);
                Assert.NotNull(response);
                Assert.NotNull(response.Data.FirstOrDefault());
            }
        }

    }
}