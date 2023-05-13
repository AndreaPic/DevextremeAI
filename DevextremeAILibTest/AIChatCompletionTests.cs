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
    public class AIChatCompletionTests : IClassFixture<TestApplication>
    {
        private readonly TestApplication _factory;

        public AIChatCompletionTests(TestApplication factory)
        {
            _factory = factory;
        }

        
        [Theory]
        //[InlineData("text-davinci-003")]
        [InlineData("gpt-3.5-turbo")]
        public async Task CreateChatCompletionTest(string modelID)
        {
            
            using (var scope = _factory.Services.CreateScope())
            {
                var openAiapiClient = scope.ServiceProvider.GetService<IOpenAIAPIClient>();
                CreateChatCompletionRequest createCompletionRequest = new CreateChatCompletionRequest();
                createCompletionRequest.Model = modelID;
                createCompletionRequest.Temperature = 0.9;

                createCompletionRequest.Messages.Add(new ChatCompletionRequestMessage()
                {
                    Role = ChatCompletionRequestMessageRoleEnum.User,
                    Content = "Hi there!"
                }); 

                var response = await openAiapiClient.CreateChatCompletionAsync(createCompletionRequest);
                Assert.NotNull(response?.OpenAIResponse);
                Assert.NotNull(response?.OpenAIResponse.Choices);
                Assert.True(response?.OpenAIResponse.Choices.Count > 0);
                Assert.NotNull(response?.OpenAIResponse.Usage);


                createCompletionRequest = new CreateChatCompletionRequest();
                createCompletionRequest.Model = modelID;
                createCompletionRequest.Temperature = 0.9;

                createCompletionRequest.Messages.Add(new ChatCompletionRequestMessage()
                {
                    Role = ChatCompletionRequestMessageRoleEnum.User,
                    Content = "I'm getting bored, what can you do for me?"
                });

                response = await openAiapiClient.CreateChatCompletionAsync(createCompletionRequest);
                Assert.NotNull(response);
                Assert.NotNull(response?.OpenAIResponse?.Choices);
                Assert.True(response.OpenAIResponse.Choices.Count > 0);
                Assert.NotNull(response?.OpenAIResponse?.Usage);

            }
        }

        [Theory]
        [InlineData("gpt-3.5-turbo")]
        public async Task CreateChatCompletionITATest(string modelID)
        {

            using (var scope = _factory.Services.CreateScope())
            {
                var openAiapiClient = scope.ServiceProvider.GetService<IOpenAIAPIClient>();
                CreateChatCompletionRequest createCompletionRequest = new CreateChatCompletionRequest();
                createCompletionRequest.Model = modelID;
                createCompletionRequest.Temperature = 1.4;

                createCompletionRequest.Messages.Add(new ChatCompletionRequestMessage()
                {
                    Role = ChatCompletionRequestMessageRoleEnum.User,
                    Content = "Ciao, sai parlare Italiano?"
                });

                var response = await openAiapiClient.CreateChatCompletionAsync(createCompletionRequest);
                Assert.NotNull(response?.OpenAIResponse);
                Assert.NotNull(response?.OpenAIResponse?.Choices);
                Assert.True(response.OpenAIResponse.Choices.Count > 0);
                Assert.NotNull(response?.OpenAIResponse?.Usage);


                createCompletionRequest = new CreateChatCompletionRequest();
                createCompletionRequest.Model = modelID;
                createCompletionRequest.Temperature = 1.4;

                createCompletionRequest.Messages.Add(new ChatCompletionRequestMessage()
                {
                    Role = ChatCompletionRequestMessageRoleEnum.User,
                    Content = "Qual'è la città più bella d'Italia?"
                });

                response = await openAiapiClient.CreateChatCompletionAsync(createCompletionRequest);
                Assert.NotNull(response?.OpenAIResponse);
                Assert.NotNull(response?.OpenAIResponse?.Choices);
                Assert.True(response.OpenAIResponse.Choices.Count > 0);
                Assert.NotNull(response?.OpenAIResponse?.Usage);

            }
        }

    }
}