using DevextremeAI.Communication.APIClient;
using DevextremeAI.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevextremeAILibTest
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
                var model = await openAiapiClient.GetModelAsync(models.OpenAIResponse.Data[0].Id);
                Assert.NotNull(model?.OpenAIResponse);
                Assert.NotNull(model?.OpenAIResponse?.Permissions);
                Assert.True(model.OpenAIResponse.Permissions.Count > 0);
            }
        }
    }
}