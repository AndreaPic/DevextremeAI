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
        [InlineData("gpt-3.5-turbo-instruct")]
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
        [InlineData("gpt-3.5-turbo-instruct")]
        public async Task CreateCompletionArrayTest(string modelID)
        {

            //await Task.Delay(22000);
            using (var scope = _factory.Services.CreateScope())
            {
                var openAiapiClient = scope.ServiceProvider.GetService<IOpenAIAPIClient>();
                CreateCompletionRequest createCompletionRequest = new CreateCompletionRequest();
                createCompletionRequest.Model = modelID;
                createCompletionRequest.MaxTokens = 1024;
                createCompletionRequest.Temperature = 0.3F;
                createCompletionRequest.Stream = false;
                createCompletionRequest.LogProbs = null;

                string prompt = "\nQUESTION: What is the Italian's capital?";

                createCompletionRequest.AddCompletionPrompt(prompt);

                var completionResponse = await openAiapiClient.CreateCompletionAsync(createCompletionRequest);
                Assert.False(completionResponse.HasError, completionResponse?.ErrorResponse?.Error?.Message);
                Assert.NotNull(completionResponse?.OpenAIResponse);
                Assert.NotNull(completionResponse?.OpenAIResponse?.Choices);
                Assert.True(completionResponse.OpenAIResponse.Choices.Count > 0);
                Assert.NotNull(completionResponse?.OpenAIResponse?.Usage);

                prompt += completionResponse.OpenAIResponse.Choices[0].Text;
                prompt += "\nQUESTION: " + "what can i see there?";

                createCompletionRequest = new CreateCompletionRequest();
                createCompletionRequest.Model = modelID;
                createCompletionRequest.MaxTokens = 1024;
                createCompletionRequest.Temperature = 0.3F;
                createCompletionRequest.Stream = false;
                createCompletionRequest.LogProbs = null;
                createCompletionRequest.AddCompletionPrompt(prompt);

                completionResponse = await openAiapiClient.CreateCompletionAsync(createCompletionRequest);
                Assert.False(completionResponse.HasError, completionResponse?.ErrorResponse?.Error?.Message);
                Assert.NotNull(completionResponse?.OpenAIResponse);
                Assert.NotNull(completionResponse?.OpenAIResponse?.Choices);
                Assert.True(completionResponse.OpenAIResponse.Choices.Count > 0);
                Assert.NotNull(completionResponse?.OpenAIResponse?.Usage);

                prompt += completionResponse.OpenAIResponse.Choices[0].Text;
                prompt += "\nQUESTION: " + "what is the best?";
                createCompletionRequest = new CreateCompletionRequest();
                createCompletionRequest.Model = modelID;
                createCompletionRequest.MaxTokens = 1024;
                createCompletionRequest.Temperature = 0.3F;
                createCompletionRequest.Stream = false;
                createCompletionRequest.LogProbs = null;
                createCompletionRequest.AddCompletionPrompt(prompt);

                completionResponse = await openAiapiClient.CreateCompletionAsync(createCompletionRequest);
                Assert.False(completionResponse.HasError, completionResponse?.ErrorResponse?.Error?.Message);
                Assert.NotNull(completionResponse?.OpenAIResponse);
                Assert.NotNull(completionResponse?.OpenAIResponse?.Choices);
                Assert.True(completionResponse.OpenAIResponse.Choices.Count > 0);
                Assert.NotNull(completionResponse?.OpenAIResponse?.Usage);

            }
        }


        [Theory]
        [InlineData("gpt-3.5-turbo-instruct")]
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