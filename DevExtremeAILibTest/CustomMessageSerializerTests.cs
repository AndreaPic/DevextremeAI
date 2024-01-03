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
            }
        }

        [Fact]
        public void RoundTripMessageWithDataSerializationTest()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var openAiapiClient = scope.ServiceProvider.GetService<IOpenAIAPIClient>();
                CreateChatCompletionRequest request = new CreateChatCompletionRequest();
                request.AddMessage(new ChatCompletionFunctionMessage());
                request.AddMessage(new ChatCompletionSystemMessage() {  Content = "system content", Name = "system"});
                request.AddMessage(new ChatCompletionAssistantMessage() {  Content = "assistant content", Name = "assistant"} );
                request.AddMessage(new ChatCompletionToolMessage() { Content = "tool content", ToolCallId = Guid.NewGuid().ToString("N")});
                request.AddMessage(new ChatCompletionUserContentMessage() {  Content = "user content", Name = "user"} );
                var userContents = new ChatCompletionUserContentsMessage() { Name = "user contents", };
                userContents.Contents.Add(new ContentTextItem() { Text = "what about this image?" });
                var imageitem = new ContentImageItem();
                imageitem.ImageUrl.Detail = ImageDetailLevel.High;
                imageitem.ImageUrl.ImageURl = Convert.ToBase64String(Resources.Resource.pink_panther);
                userContents.Contents.Add(imageitem);
                request.AddMessage(userContents);

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

                ChatCompletionUserContentsMessage userContentsMessage = deserializedRequest.Messages[5] as ChatCompletionUserContentsMessage;

                Assert.True(userContentsMessage.Contents.Count == 2);
                Assert.IsType<ContentTextItem>(userContentsMessage.Contents[0]);
                Assert.IsType<ContentImageItem>(userContentsMessage.Contents[1]);

                ContentImageItem imageItem = userContentsMessage.Contents[1] as ContentImageItem;

                Assert.True(imageItem.ImageUrl.ImageURl.Length == imageitem.ImageUrl.ImageURl.Length);

            }
        }
    }
}