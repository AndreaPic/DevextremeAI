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
                

                var completionResponse = await openAiapiClient.CreateImageAsync(request);
                Assert.NotNull(completionResponse);
                Assert.NotNull(completionResponse.Data.FirstOrDefault());
            }
        }
    }
}