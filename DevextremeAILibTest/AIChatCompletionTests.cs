using DevextremeAI.Communication;
using DevextremeAI.Settings;
using DevExtremeToys.JSon;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using System.Text.Json;
using Newtonsoft.Json;

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
        [InlineData("text-davinci-003")]
        [InlineData("gpt-3.5-turbo")]
        public async Task CreateCompletionTest(string modelID)
        {
            
            using (var scope = _factory.Services.CreateScope())
            {
                var openAiapiClient = scope.ServiceProvider.GetService<IOpenAIAPIClient>();
                CreateChatCompletionRequest createCompletionRequest = new CreateChatCompletionRequest();
                createCompletionRequest.ModelID = modelID;
                createCompletionRequest.Temperature = 0.9;

                createCompletionRequest.Messages.Add(new ChatCompletionRequestMessage()
                {
                    Role = ChatCompletionRequestMessageRoleEnum.User,
                    Content = "Hi there!"
                }); 

                var response = await openAiapiClient.CreateChatCompletionAsync(createCompletionRequest);
                Assert.NotNull(response);
                Assert.NotNull(response.Choices);
                Assert.True(response.Choices.Count > 0);
                Assert.NotNull(response.Usage);


                createCompletionRequest = new CreateChatCompletionRequest();
                createCompletionRequest.ModelID = modelID;
                createCompletionRequest.Temperature = 0.9;

                createCompletionRequest.Messages.Add(new ChatCompletionRequestMessage()
                {
                    Role = ChatCompletionRequestMessageRoleEnum.User,
                    Content = "I'm getting bored, what can you do for me?"
                });

                response = await openAiapiClient.CreateChatCompletionAsync(createCompletionRequest);
                Assert.NotNull(response);
                Assert.NotNull(response.Choices);
                Assert.True(response.Choices.Count > 0);
                Assert.NotNull(response.Usage);

            }
        }
    }
}