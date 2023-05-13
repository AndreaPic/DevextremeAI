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
    public class AIModerationTests : IClassFixture<TestApplication>
    {
        private readonly TestApplication _factory;

        public AIModerationTests(TestApplication factory)
        {
            _factory = factory;
        }

        
        [Theory]
        [InlineData("text-moderation-stable")]
        [InlineData("text-moderation-latest")]
        public async Task CreateModerationsTest(string model)
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var openAiapiClient = scope.ServiceProvider.GetService<IOpenAIAPIClient>();
                CreateModerationsRequest request = new CreateModerationsRequest();
                request.Model = model;
                request.Input = "I want to kill them.";

                var response = await openAiapiClient.CreateModerationsAsync(request);
                Assert.NotNull(response?.OpenAIResponse);
                Assert.False(response.HasError);
                Assert.NotNull(response.OpenAIResponse.Results);
                Assert.True(response.OpenAIResponse.Results.Count > 0);
                Assert.NotNull(response.OpenAIResponse.Results[0]);
                Assert.NotNull(response.OpenAIResponse.Results[0].Categories);
                Assert.True(response.OpenAIResponse.Results[0].Categories.Violence);

            }
        }

    }
}