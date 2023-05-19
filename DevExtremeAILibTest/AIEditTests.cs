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
    public class AIEditTests : IClassFixture<TestApplication>
    {
        private readonly TestApplication _factory;

        public AIEditTests(TestApplication factory)
        {
            _factory = factory;
        }

        
        [Theory]
        [InlineData("text-davinci-edit-001")]
        public async Task CreateEditTest(string modelID)
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var openAiapiClient = scope.ServiceProvider.GetService<IOpenAIAPIClient>();
                CreateEditRequest request = new CreateEditRequest();
                request.Model = modelID;
                request.Input = "What day of the wek is it?";
                request.Instruction = "Fix the spelling mistakes";

                await Task.Delay(22000);

                var completionResponse = await openAiapiClient.CreateEditAsync(request);
                Assert.False(completionResponse.HasError,completionResponse?.ErrorResponse?.Error?.Message);
                Assert.NotNull(completionResponse?.OpenAIResponse);
                Assert.NotNull(completionResponse?.OpenAIResponse?.Choices);
                Assert.True(completionResponse?.OpenAIResponse.Choices.Count > 0);
                Assert.NotNull(completionResponse.OpenAIResponse.Usage);
            }
        }
        [Theory]
        [InlineData("text-davinci-edit-001")]
        public async Task CreateEditMathTest(string modelID)
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var openAiapiClient = scope.ServiceProvider.GetService<IOpenAIAPIClient>();
                CreateEditRequest request = new CreateEditRequest();
                request.Model = modelID;
                request.Input = "2 + 2 = 5";
                request.Instruction = "Fix the wrong result";

                await Task.Delay(22000);
                var completionResponse = await openAiapiClient.CreateEditAsync(request);
                Assert.False(completionResponse.HasError, completionResponse?.ErrorResponse?.Error?.Message);
                Assert.NotNull(completionResponse?.OpenAIResponse);
                Assert.NotNull(completionResponse?.OpenAIResponse.Choices);
                Assert.True(completionResponse?.OpenAIResponse.Choices.Count > 0);
                Assert.NotNull(completionResponse?.OpenAIResponse.Usage);
            }
        }
    }
}