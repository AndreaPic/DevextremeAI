using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using DevExtremeAI.OpenAIDTO;
using DevExtremeAI.OpenAIClient;

namespace DevExtremeAILibTest
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
                    Role = ChatCompletionMessageRoleEnum.User,
                    Content = "Hi there!"
                });
                await Task.Delay(22000);

                var response = await openAiapiClient.CreateChatCompletionAsync(createCompletionRequest);
                Assert.False(response.HasError,response?.ErrorResponse?.Error?.Message);
                Assert.NotNull(response?.OpenAIResponse);
                Assert.NotNull(response?.OpenAIResponse.Choices);
                Assert.True(response?.OpenAIResponse.Choices.Count > 0);
                Assert.NotNull(response?.OpenAIResponse.Usage);


                createCompletionRequest = new CreateChatCompletionRequest();
                createCompletionRequest.Model = modelID;
                createCompletionRequest.Temperature = 0.9;

                createCompletionRequest.Messages.Add(new ChatCompletionRequestMessage()
                {
                    Role = ChatCompletionMessageRoleEnum.User,
                    Content = "I'm getting bored, what can you do for me?"
                });

                await Task.Delay(22000);

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
                    Role = ChatCompletionMessageRoleEnum.User,
                    Content = "Ciao, sai parlare Italiano?"
                });

                var response = await openAiapiClient.CreateChatCompletionAsync(createCompletionRequest);
                Assert.False(response.HasError,response?.ErrorResponse?.Error?.Message);
                Assert.NotNull(response?.OpenAIResponse);
                Assert.NotNull(response?.OpenAIResponse?.Choices);
                Assert.True(response.OpenAIResponse.Choices.Count > 0);
                Assert.NotNull(response?.OpenAIResponse?.Usage);
                
                Debug.WriteLine(response.OpenAIResponse.Choices[0].Message.Content);

                createCompletionRequest.Messages.Add(new ChatCompletionRequestMessage()
                {
                    Role = response.OpenAIResponse.Choices[0].Message.Role,
                    Content = response.OpenAIResponse.Choices[0].Message.Content
                });


                createCompletionRequest.Messages.Add(new ChatCompletionRequestMessage()
                {
                    Role = ChatCompletionMessageRoleEnum.User,
                    Content = "Qual'è la capitale d'Italia?"
                });

                await Task.Delay(22000);
                response = await openAiapiClient.CreateChatCompletionAsync(createCompletionRequest);
                Assert.NotNull(response?.OpenAIResponse);
                Assert.NotNull(response?.OpenAIResponse?.Choices);
                Assert.True(response.OpenAIResponse.Choices.Count > 0);
                Assert.NotNull(response?.OpenAIResponse?.Usage);

                Debug.WriteLine(response.OpenAIResponse.Choices[0].Message.Content);

                createCompletionRequest.Messages.Add(new ChatCompletionRequestMessage()
                {
                    Role = response.OpenAIResponse.Choices[0].Message.Role,
                    Content = response.OpenAIResponse.Choices[0].Message.Content
                });

                createCompletionRequest.Messages.Add(new ChatCompletionRequestMessage()
                {
                    Role = ChatCompletionMessageRoleEnum.User,
                    Content = "Quali cose potrei visitare li?"
                });

                await Task.Delay(22000);
                response = await openAiapiClient.CreateChatCompletionAsync(createCompletionRequest);
                Assert.NotNull(response?.OpenAIResponse);
                Assert.NotNull(response?.OpenAIResponse?.Choices);
                Assert.True(response.OpenAIResponse.Choices.Count > 0);
                Assert.NotNull(response?.OpenAIResponse?.Usage);
                Debug.WriteLine(response.OpenAIResponse.Choices[0].Message.Content);


            }
        }

    }
}