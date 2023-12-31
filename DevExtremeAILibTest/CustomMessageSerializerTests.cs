using Microsoft.Extensions.DependencyInjection;
using DevExtremeAI.OpenAIDTO;
using DevExtremeAI.OpenAIClient;
using System.Text.Json;

namespace DevExtremeAILibTest
{
    public class CustomMessageSerializerTests : IClassFixture<TestApplication>
    {
        private readonly TestApplication _factory;

        public CustomMessageSerializerTests(TestApplication factory)
        {
            _factory = factory;
        }

        [Fact]
        public void RoundTripMessageSerializationTest()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var openAiapiClient = scope.ServiceProvider.GetService<IOpenAIAPIClient>();
                CreateChatCompletionRequest request = new CreateChatCompletionRequest();
                request.AddMessage(new ChatCompletionFunctionMessage());
                request.AddMessage(new ChatCompletionSystemMessage());
                request.AddMessage(new ChatCompletionAssistantMessage());
                request.AddMessage(new ChatCompletionToolMessage());
                request.AddMessage(new ChatCompletionUserContentMessage());
                request.AddMessage(new ChatCompletionUserContentsMessage());

                var json = JsonSerializer.Serialize(request);

                Assert.NotNull(json);

                var deserializedRequest = JsonSerializer.Deserialize<CreateChatCompletionRequest>(json);
                Assert.NotNull(deserializedRequest);
                Assert.IsType<CreateChatCompletionRequest>(deserializedRequest);
                Assert.True(deserializedRequest.Messages.Count == 6);
                Assert.IsType<ChatCompletionFunctionMessage>(deserializedRequest.Messages[0]);
                Assert.IsType<ChatCompletionSystemMessage>(deserializedRequest.Messages[1]);
                Assert.IsType<ChatCompletionAssistantMessage>(deserializedRequest.Messages[2]);
                Assert.IsType<ChatCompletionToolMessage>(deserializedRequest.Messages[3]);
                Assert.IsType<ChatCompletionUserContentMessage>(deserializedRequest.Messages[4]);
                Assert.IsType<ChatCompletionUserContentsMessage>(deserializedRequest.Messages[5]);



                //var response = await openAiapiClient.CreateTranscriptionsAsync(request);
                //Assert.NotNull(response);
                //Assert.False(response.HasError,response?.ErrorResponse?.Error?.Message);
                //Assert.NotNull(response?.OpenAIResponse?.Text);
                //Assert.True(response.OpenAIResponse.Text.Contains("prova"));
            }
        }
    }
}