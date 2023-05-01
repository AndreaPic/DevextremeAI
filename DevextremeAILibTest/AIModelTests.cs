using DevextremeAI.Communication;
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
                Assert.NotNull(models);
                Assert.True(models.Data.Count > 0);
                var model = await openAiapiClient.GetModelAsync(models.Data[0].Id);
                Assert.NotNull(model);
                Assert.NotNull(model.Permissions);
                Assert.True(model.Permissions.Count > 0);
            }
        }
    }
}