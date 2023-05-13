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
    public class AIAudioTests : IClassFixture<TestApplication>
    {
        private readonly TestApplication _factory;

        public AIAudioTests(TestApplication factory)
        {
            _factory = factory;
        }

        
        [Theory]
        [InlineData("whisper-1")]
        public async Task CreateTranscriptionsTest(string modelID)
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var openAiapiClient = scope.ServiceProvider.GetService<IOpenAIAPIClient>();
                CreateTranscriptionsRequest request = new CreateTranscriptionsRequest();
                request.ModelID = modelID;
                request.File = Resources.Resource.Test;
                request.Language = "it";

                var response = await openAiapiClient.CreateTranscriptionsAsync(request);
                Assert.NotNull(response);
                Assert.False(response.HasError);
                Assert.NotNull(response.OpenAIResponse.Text);
                Assert.Equal(response.OpenAIResponse.Text,"1 2 3 prova");
            }
        }

        [Theory]
        [InlineData("whisper-1")]
        public async Task CreateTranslationsTest(string modelID)
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var openAiapiClient = scope.ServiceProvider.GetService<IOpenAIAPIClient>();
                CreateTranslationsRequest request = new CreateTranslationsRequest();
                request.ModelID = modelID;
                request.File = Resources.Resource.Test;

                var response = await openAiapiClient.CreateTranslationsAsync(request);
                Assert.NotNull(response);
                Assert.False(response.HasError);
                Assert.NotNull(response.OpenAIResponse.Text);
                Assert.Equal(response.OpenAIResponse.Text, "1, 2, 3, try!");
            }
        }

    }
}