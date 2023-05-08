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
    public class AICompletionTests : IClassFixture<TestApplication>
    {
        private readonly TestApplication _factory;

        public AICompletionTests(TestApplication factory)
        {
            _factory = factory;
        }

        
        [Theory]
        [InlineData("text-davinci-003")]
        [InlineData("gpt-3.5-turbo")]
        public async Task CreateCompletionTest(string modelID)
        {
            
            using (var scope = _factory.Services.CreateScope())
            {
                var openAiapiClient = scope.ServiceProvider.GetService<IOpenAIAPIClient>();
                CreateCompletionRequest createCompletionRequest = new CreateCompletionRequest();
                createCompletionRequest.ModelID = modelID;
                createCompletionRequest.MaxTokens = 7;
                createCompletionRequest.Temperature = 0;
                //createCompletionRequest.TopP = 1;
                //createCompletionRequest.N = 1;
                createCompletionRequest.Stream = false;
                createCompletionRequest.LogProbs = null;
                //createCompletionRequest.Stop = "\n";


                createCompletionRequest.AddCompletionPrompt("Say this is a test");

                var json = createCompletionRequest.ToJSon(new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                var completionResponse = await openAiapiClient.CreateCompletionAsync(createCompletionRequest);
                Assert.NotNull(completionResponse?.OpenAIResponse);
                Assert.NotNull(completionResponse?.OpenAIResponse?.Choices);
                Assert.True(completionResponse.OpenAIResponse.Choices.Count > 0);
                Assert.NotNull(completionResponse?.OpenAIResponse?.Usage);

            }
        }
    }
}