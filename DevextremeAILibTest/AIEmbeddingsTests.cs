using DevExtremeAI.Settings;
using DevExtremeToys.JSon;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using System.Text.Json;
using Newtonsoft.Json;
using DevExtremeAI.OpenAIDTO;
using DevExtremeAI.OpenAIClient;

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
                Assert.False(completionResponse.HasError);
                Assert.NotNull(completionResponse.OpenAIResponse.Data);
                Assert.NotNull(completionResponse.OpenAIResponse.Object);
                Assert.NotNull(completionResponse.OpenAIResponse.Usage);
            }
        }
    }
}