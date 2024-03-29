using DevExtremeAI.OpenAIClient;
using Microsoft.Extensions.DependencyInjection;

namespace DevExtremeAILibTest
{
    public class AIModelTests : IClassFixture<TestApplication>
    {
        private readonly TestApplication _factory;

        public AIModelTests(TestApplication factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetModelTest()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var openAiapiClient = scope.ServiceProvider.GetService<IOpenAIAPIClient>();
                var models = await openAiapiClient.GetModelsAsync();
                Assert.NotNull(models?.OpenAIResponse?.Data);
                Assert.True(models?.OpenAIResponse.Data.Count > 0);
                int modelIndex = 0;
                var model = await openAiapiClient.GetModelAsync(models.OpenAIResponse.Data[modelIndex].Id);
                Assert.NotNull(model?.OpenAIResponse);
                Assert.NotNull(model?.OpenAIResponse?.Permissions);
                Assert.True(model.OpenAIResponse.Permissions.Count == 0);
            }
        }
    }
}