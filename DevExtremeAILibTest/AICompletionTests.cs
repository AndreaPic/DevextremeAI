using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using DevExtremeAI.OpenAIDTO;
using DevExtremeAI.OpenAIClient;


namespace DevExtremeAILibTest
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
        //[InlineData("gpt-3.5-turbo")]
        public async Task CreateCompletionTest(string modelID)
        {

            await Task.Delay(22000);
            using (var scope = _factory.Services.CreateScope())
            {
                var openAiapiClient = scope.ServiceProvider.GetService<IOpenAIAPIClient>();
                CreateCompletionRequest createCompletionRequest = new CreateCompletionRequest();
                createCompletionRequest.Model = modelID;
                createCompletionRequest.MaxTokens = 7;
                createCompletionRequest.Temperature = 0;
                createCompletionRequest.Stream = false;
                createCompletionRequest.LogProbs = null;

                createCompletionRequest.AddCompletionPrompt("Say this is a test");

                var completionResponse = await openAiapiClient.CreateCompletionAsync(createCompletionRequest);
                Assert.False(completionResponse.HasError, completionResponse?.ErrorResponse?.Error?.Message);
                Assert.NotNull(completionResponse?.OpenAIResponse);
                Assert.NotNull(completionResponse?.OpenAIResponse?.Choices);
                Assert.True(completionResponse.OpenAIResponse.Choices.Count > 0);
                Assert.NotNull(completionResponse?.OpenAIResponse?.Usage);

            }
        }

        [Theory]
        [InlineData("text-davinci-003")]
        //[InlineData("gpt-3.5-turbo")]
        public async Task CreateCompletionStreamTest(string modelID)
        {

            await Task.Delay(22000);
            using (var scope = _factory.Services.CreateScope())
            {
                var openAiapiClient = scope.ServiceProvider.GetService<IOpenAIAPIClient>();
                CreateCompletionRequest createCompletionRequest = new CreateCompletionRequest();
                createCompletionRequest.Model = modelID;
                createCompletionRequest.MaxTokens = 50;
                createCompletionRequest.Temperature = 0;
                createCompletionRequest.Stream = false;
                createCompletionRequest.LogProbs = null;

                createCompletionRequest.AddCompletionPrompt("Say this is a test");


                try
                {
                    await foreach (var completionResponse in openAiapiClient.CreateCompletionStreamAsync(
                                       createCompletionRequest))
                    {
                        Assert.False(completionResponse.HasError, completionResponse?.ErrorResponse?.Error?.Message);
                        Assert.NotNull(completionResponse?.OpenAIResponse);
                        Assert.NotNull(completionResponse?.OpenAIResponse?.Choices);
                        Assert.True(completionResponse.OpenAIResponse.Choices.Count > 0);
                        Debug.WriteLine(completionResponse?.OpenAIResponse?.Choices[0]?.Text);

                    }
                }
                catch (Exception ex)
                {
                    Assert.True(false, ex.Message);
                }



            }
        }
    }
}