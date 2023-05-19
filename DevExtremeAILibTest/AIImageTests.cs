using DevExtremeAI.Settings;
using DevExtremeToys.JSon;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using System.Text.Json;
using Newtonsoft.Json;
using DevExtremeAI.OpenAIDTO;
using DevExtremeAI.OpenAIClient;

namespace DevExtremeAILibTest
{
    public class AIImageTests : IClassFixture<TestApplication>
    {
        private readonly TestApplication _factory;

        public AIImageTests(TestApplication factory)
        {
            _factory = factory;
        }

        
        [Fact]
        public async Task CreateImageLinkTest()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var openAiapiClient = scope.ServiceProvider.GetService<IOpenAIAPIClient>();
                CreateImageRequest request = new CreateImageRequest();
                request.Prompt = "draw me a picture of a friendly robot";
                request.N = 1;
                request.Size = CreateImageRequestSizeEnum._256x256;
                request.ResponseFormat = CreateImageRequestResponseFormatEnum.Url;
                

                var response = await openAiapiClient.CreateImageAsync(request);
                Assert.NotNull(response?.OpenAIResponse?.Data);
                Assert.NotNull(response?.OpenAIResponse.Data.FirstOrDefault());
            }
        }

        [Fact]
        public async Task CreateImageFileTest()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var openAiapiClient = scope.ServiceProvider.GetService<IOpenAIAPIClient>();
                CreateImageRequest request = new CreateImageRequest();
                request.Prompt = "friendly robot";
                request.N = 1;
                request.Size = CreateImageRequestSizeEnum._256x256;
                request.ResponseFormat = CreateImageRequestResponseFormatEnum.B64Json;


                var response = await openAiapiClient.CreateImageAsync(request);
                Assert.NotNull(response?.OpenAIResponse?.Data);
                Assert.NotNull(response.OpenAIResponse.Data.FirstOrDefault());
                //File.WriteAllBytes(@"D:\Temp\aifile.png", response.Data.FirstOrDefault().B64Json);
            }
        }


        [Fact]
        public async Task CreateImageEditTest()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var openAiapiClient = scope.ServiceProvider.GetService<IOpenAIAPIClient>();

                CreateImageEditRequest requestEditImage = new CreateImageEditRequest();
                requestEditImage.Image = Resources.Resource.friendly_robot;
                requestEditImage.Prompt = "change robot's color to purple";
                requestEditImage.N = 1;
                requestEditImage.Size = CreateImageRequestSizeEnum._256x256;
                requestEditImage.ResponseFormat = CreateImageRequestResponseFormatEnum.Url;

                var response = await openAiapiClient.CreateImageEditAsync(requestEditImage);
                Assert.NotNull(response);
                //Assert.NotNull(response.OpenAIResponse.Data.FirstOrDefault());
            }
        }

        [Fact]
        public async Task CreateImageVariationsTest()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var openAiapiClient = scope.ServiceProvider.GetService<IOpenAIAPIClient>();

                CreateImageVariationsRequest request = new CreateImageVariationsRequest();
                request.Prompt = "Change robot's color to green";
                request.Image = Resources.Resource.friendly_robot;
                request.N = 1;
                request.Size = CreateImageRequestSizeEnum._256x256;
                request.ResponseFormat = CreateImageRequestResponseFormatEnum.Url;

                var response = await openAiapiClient.CreateImageVariationsAsync(request);
                Assert.NotNull(response);
                //Assert.NotNull(response.OpenAIResponse.Data.FirstOrDefault());
            }
        }

    }
}