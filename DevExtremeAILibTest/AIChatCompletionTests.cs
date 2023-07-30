using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using DevExtremeAI.OpenAIDTO;
using DevExtremeAI.OpenAIClient;
using System.Text.Json;

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






        /// <summary>
        /// This test is looks like the official documentation at https://github.com/openai/openai-cookbook/blob/main/examples/How_to_call_functions_with_chat_models.ipynb
        /// </summary>
        /// <param name="modelID"></param>
        /// <returns></returns>
        [Theory]
        [InlineData("gpt-3.5-turbo-0613")]
        public async Task CreateChatCompletionOneFunctionTest(string modelID)
        {

            using (var scope = _factory.Services.CreateScope())
            {
                var openAiapiClient = scope.ServiceProvider.GetService<IOpenAIAPIClient>();
                CreateChatCompletionRequest createCompletionRequest = new CreateChatCompletionRequest();
                createCompletionRequest.Model = modelID;
                createCompletionRequest.Temperature = 1.4;
                var function = new ChatCompletionfunction();
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
                Assert.True(response.OpenAIResponse.Choices[0].FinishReason == "function_call");
                Assert.NotNull(response.OpenAIResponse.Choices[0].Message.FunctionCall);
                Assert.True(response.OpenAIResponse.Choices[0].Message.FunctionCall.FunctionName == "get_current_weather");
                Assert.True(response.OpenAIResponse.Choices[0].Message.FunctionCall.Arguments.Keys.Count == 2);
                //Assert.True(response.OpenAIResponse.Choices[0].Message.FunctionCall.Arguments.Keys.ElementAt(0) == "location");
                //Assert.True(response.OpenAIResponse.Choices[0].Message.FunctionCall.Arguments["location"].ToString().Contains("Venezia"));
                //Assert.True(response.OpenAIResponse.Choices[0].Message.FunctionCall.Arguments.Keys.ElementAt(1) == "format");
                //Assert.True(response.OpenAIResponse.Choices[0].Message.FunctionCall.Arguments.Values.ElementAt(1).ToString().Contains("celsius"));
                //Assert.NotNull(response?.OpenAIResponse?.Usage);


                //"{\n\"location\": \"Venezia, IT\",\n\"format\": \"celsius\"\n}");

                var jsonFunctionArguments = JsonSerializer.Serialize(response.OpenAIResponse.Choices[0].Message.FunctionCall.Arguments, response.OpenAIResponse.Choices[0].Message.FunctionCall.Arguments.GetType());
                var args = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonFunctionArguments);
                Assert.NotNull(args);
                Assert.True(args.Count == response.OpenAIResponse.Choices[0].Message.FunctionCall.Arguments.Count);
                Assert.True(args["location"].ToString() == response.OpenAIResponse.Choices[0].Message.FunctionCall.Arguments["location"].ToString());
                Assert.True(args["format"].ToString() == response.OpenAIResponse.Choices[0].Message.FunctionCall.Arguments["format"].ToString());


                var jsonFunctionCall = JsonSerializer.Serialize(response.OpenAIResponse.Choices[0].Message.FunctionCall, response.OpenAIResponse.Choices[0].Message.FunctionCall.GetType());
                FunctionCallDefinition fc = JsonSerializer.Deserialize<FunctionCallDefinition>(jsonFunctionCall);
                Assert.NotNull(fc);
                Assert.True(fc.FunctionName == response.OpenAIResponse.Choices[0].Message.FunctionCall.FunctionName);
                Assert.True(fc.Arguments.Count == response.OpenAIResponse.Choices[0].Message.FunctionCall.Arguments.Count);
                Assert.True(fc.Arguments["location"].ToString() == response.OpenAIResponse.Choices[0].Message.FunctionCall.Arguments["location"].ToString());
                Assert.True(fc.Arguments["format"].ToString() == response.OpenAIResponse.Choices[0].Message.FunctionCall.Arguments["format"].ToString());
            }
        }

        /// <summary>
        /// This test is looks like the official documentation at https://github.com/openai/openai-cookbook/blob/main/examples/How_to_call_functions_with_chat_models.ipynb
        /// </summary>
        /// <param name="modelID"></param>
        /// <returns></returns>
        [Theory]
        [InlineData("gpt-3.5-turbo-0613")]
        public async Task CreateChatCompletionMultipleFunctionTest(string modelID)
        {

            using (var scope = _factory.Services.CreateScope())
            {
                var openAiapiClient = scope.ServiceProvider.GetService<IOpenAIAPIClient>();
                CreateChatCompletionRequest createCompletionRequest = new CreateChatCompletionRequest();
                createCompletionRequest.Model = modelID;
                createCompletionRequest.Temperature = 1.4;
                var function = new ChatCompletionfunction();
                var func1 = new FunctionDefinition()
                {
                    Name = "get_current_weather",
                    Description = "Get the current weather in a given location"
                };
                func1.Parameters.Properties.Add("location", new PropertyDefinition()
                {
                    TypeName = "string",
                    Description = "The city and state, e.g. San Francisco, CA"
                });
                func1.Parameters.Properties.Add("format", new PropertyDefinition()
                {
                    TypeName = "string",
                    Description = "The temperature unit to use. Infer this from the users location.",
                    EnumValues = new[] { "celsius", "fahrenheit" }
                });
                func1.Parameters.AddRequiredProperty("location");
                func1.Parameters.AddRequiredProperty("format");

                createCompletionRequest.AddFunction(func1);


                var func2 = new FunctionDefinition()
                {
                    Name = "get_n_day_weather_forecast",
                    Description = "Get an N-day weather forecast"
                };
                func2.Parameters.Properties.Add("location", new PropertyDefinition()
                {
                    TypeName = "string",
                    Description = "The city and state, e.g. San Francisco, CA"
                });
                func2.Parameters.Properties.Add("format", new PropertyDefinition()
                {
                    TypeName = "string",
                    Description = "The temperature unit to use. Infer this from the users location.",
                    EnumValues = new[] { "celsius", "fahrenheit" }
                });
                func2.Parameters.Properties.Add("num_days", new PropertyDefinition()
                {
                    TypeName = "integer",
                    Description = "The number of days to forecast"
                });
                func2.Parameters.AddRequiredProperty("location");
                func2.Parameters.AddRequiredProperty("format");
                func2.Parameters.AddRequiredProperty("num_days");

                createCompletionRequest.AddFunction(func2);



                createCompletionRequest.Messages.Add(new ChatCompletionRequestMessage()
                {
                    Role = ChatCompletionMessageRoleEnum.User,
                    Content = "what is the weather going to be like in Glasgow, Scotland over the next x days"
                });

                var response = await openAiapiClient.CreateChatCompletionAsync(createCompletionRequest);
                Assert.False(response.HasError, response?.ErrorResponse?.Error?.Message);
                Assert.NotNull(response?.OpenAIResponse);
                Assert.NotNull(response?.OpenAIResponse?.Choices);
                Assert.True(response.OpenAIResponse.Choices.Count > 0);


                createCompletionRequest.Messages.Add(new ChatCompletionRequestMessage()
                {
                    Role = ChatCompletionMessageRoleEnum.Assistant,
                    Content = response.OpenAIResponse.Choices[0].Message.Content
                });

                createCompletionRequest.Messages.Add(new ChatCompletionRequestMessage()
                {
                    Role = ChatCompletionMessageRoleEnum.User,
                    Content = "5 days"
                });

                response = await openAiapiClient.CreateChatCompletionAsync(createCompletionRequest);
                Assert.False(response.HasError, response?.ErrorResponse?.Error?.Message);
                Assert.NotNull(response?.OpenAIResponse);
                Assert.NotNull(response?.OpenAIResponse?.Choices);
                Assert.True(response.OpenAIResponse.Choices.Count > 0);
                Assert.True(response.OpenAIResponse.Choices[0].FinishReason == "function_call");
                Assert.True(response.OpenAIResponse.Choices[0].Message.FunctionCall.FunctionName == "get_n_day_weather_forecast");
            }
        }


    }
}