using Microsoft.Extensions.DependencyInjection;
using DevExtremeAI.OpenAIDTO;
using DevExtremeAI.OpenAIClient;

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
        public async Task RoundTripMessageSerializationTest()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var openAiapiClient = scope.ServiceProvider.GetService<IOpenAIAPIClient>();
                CreateChatCompletionRequest request = new CreateChatCompletionRequest();
                request.AddMessage(new ChatCompletionFunctionMessage());


                //var response = await openAiapiClient.CreateTranscriptionsAsync(request);
                //Assert.NotNull(response);
                //Assert.False(response.HasError,response?.ErrorResponse?.Error?.Message);
                //Assert.NotNull(response?.OpenAIResponse?.Text);
                //Assert.True(response.OpenAIResponse.Text.Contains("prova"));
            }
        }
    }
}