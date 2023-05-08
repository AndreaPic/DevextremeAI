using DevextremeAI.Settings;
using DevExtremeToys.JSon;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using System.Text.Json;
using Newtonsoft.Json;
using DevextremeAI.Communication.DTO;
using DevextremeAI.Communication.APIClient;

namespace DevextremeAILibTest
{
    public class AIEmbeddingsTests : IClassFixture<TestApplication>
    {
        private readonly TestApplication _factory;

        public AIEmbeddingsTests(TestApplication factory)
        {
            _factory = factory;
        }


        [Theory]
        [InlineData("text-embedding-ada-002")]
        public async Task CreateEmbeddingsTest(string modelID)
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var openAiapiClient = scope.ServiceProvider.GetService<IOpenAIAPIClient>();
                CreateEmbeddingsRequest request = new CreateEmbeddingsRequest();
                request.ModelID = modelID;
                request.AddInput("The food was delicious and the waiter very kind");

                var completionResponse = await openAiapiClient.CreateEmbeddingsAsync(request);
                Assert.NotNull(completionResponse);
                Assert.NotNull(completionResponse.Data);
                Assert.NotNull(completionResponse.Object);
                Assert.NotNull(completionResponse.Usage);
            }
        }
    }
}