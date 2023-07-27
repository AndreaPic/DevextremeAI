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
        public async Task CreateChatCompletionStreamTest(string modelID)
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
                    Content = "I'm getting bored, what can you do for me?"
                });
                await Task.Delay(22000);

                try
                {
                    await foreach (var response in openAiapiClient.CreateChatCompletionStreamAsync(
                                       createCompletionRequest))
                    {
                        Assert.False(response.HasError, response?.ErrorResponse?.Error?.Message);
                        Assert.NotNull(response?.OpenAIResponse);
                        Assert.NotNull(response?.OpenAIResponse.Choices);
                        Assert.True(response?.OpenAIResponse.Choices.Count > 0);
                        Debug.WriteLine(response?.OpenAIResponse?.Choices[0]?.Delta?.Content);

                    }
                }
                catch (Exception ex)
                {
                    Assert.True(false,ex.Message);
                }

            }
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

        [Theory]
        [InlineData("gpt-3.5-turbo-0613")]
        public async Task CreateChatCompletionFunctionTest(string modelID)
        {

            using (var scope = _factory.Services.CreateScope())
            {
                var openAiapiClient = scope.ServiceProvider.GetService<IOpenAIAPIClient>();
                CreateChatCompletionRequest createCompletionRequest = new CreateChatCompletionRequest();
                createCompletionRequest.Model = modelID;
                createCompletionRequest.Temperature = 1.4;
                var function = new ChatCompletionfunction();
                //function.Name = "get_current_weather";
                //function.Description = "Get the current weather in a given location";
                //function.JSONSchemaParameters = "{\r\n    \"type\": \"object\",\r\n    \"properties\": {\r\n        \"location\": {\r\n            \"type\": \"string\",\r\n            \"description\": \"The city and state, e.g. San Francisco, CA\"\r\n        },\r\n        \"unit\": {\"type\": \"string\", \"enum\": [\"celsius\", \"fahrenheit\"]},\r\n    },\r\n    \"required\": [\"location\"]\r\n}";
                ////createCompletionRequest.Functions = "[{\"name\": \"get_current_weather\",\"description\": \"Get the current weather\",\"parameters\": {\"type\": \"object\",\"properties\": {\"location\": {\"type\": \"string\",\"description\": \"The city and state, e.g. San Francisco , CA\"},\"format\": {\"type\": \"string\",\"enum\": [\"celsius\", \"fahrenheit\"],\"description\": \"The temperature unit to use. Infer this from the users location.\"}},\"required\": [\"location\", \"format\"]}}]";
                var func = new FunctionDefinition()
                {
                    Name = "get_current_weather",
                    Description = "Get the current weather in a given location"
                };
                func.Parameters.Properties.Add("location", new PropertyDefinition()
                {
                     TypeName = "string",
                     Description = "The city and state, e.g. San Francisco, CA"
                });
                func.Parameters.Properties.Add("format", new PropertyDefinition()
                {
                    TypeName = "string",
                    Description = "The temperature unit to use. Infer this from the users location.",
                    EnumValues = new []{ "celsius", "fahrenheit" }                    
                });
                func.Parameters.AddRequiredProperty("location");
                func.Parameters.AddRequiredProperty("format");

                createCompletionRequest.AddFunction(func);

                createCompletionRequest.Messages.Add(new ChatCompletionRequestMessage()
                {
                    Role = ChatCompletionMessageRoleEnum.User,
                    Content = "Come sarà il tempo a Venezia, Italia oggi?"
                });

                var response = await openAiapiClient.CreateChatCompletionAsync(createCompletionRequest);
                Assert.False(response.HasError, response?.ErrorResponse?.Error?.Message);
                Assert.NotNull(response?.OpenAIResponse);
                Assert.NotNull(response?.OpenAIResponse?.Choices);
                Assert.True(response.OpenAIResponse.Choices.Count > 0);
                Assert.NotNull(response?.OpenAIResponse?.Usage);


            }
        }


    }
}