using Microsoft.Extensions.DependencyInjection;
using DevExtremeAI.OpenAIDTO;
using DevExtremeAI.OpenAIClient;

namespace DevExtremeAILibTest
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
                request.Model = modelID;
                request.AddInput("The food was delicious and the waiter very kind");

                var completionResponse = await openAiapiClient.CreateEmbeddingsAsync(request);
                Assert.False(completionResponse.HasError,completionResponse?.ErrorResponse?.Error?.Message);
                Assert.NotNull(completionResponse.OpenAIResponse.Data);
                Assert.NotNull(completionResponse.OpenAIResponse.Object);
                Assert.NotNull(completionResponse.OpenAIResponse.Usage);
            }
        }
    }
}