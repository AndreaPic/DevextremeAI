# DevExtremeAI

## Overview

DevExtremeAI is a library with full and complete implementation of all OpenAI's APIs.
This library is fully compliant to openAI specs and also implement openAI error response.
It's very easy to use with asp.net core and has full support to dependency injection (with a single line of code as asp.net standard pattern).
It's also easy to use  in libraries without dependency injection (see samples below).

*Please note that this is **unofficial** OpenAPI library* (It's not mantained by OpenAI Company).

## Build Status

| Build | Status | Current Version |
| ------ | ------ | ------ |
| CI | [![BUILD-TEST](https://github.com/AndreaPic/DevextremeAI/actions/workflows/dotnet-ci.yml/badge.svg?branch=master)](https://github.com/AndreaPic/DevextremeAI/actions/workflows/dotnet-ci.yml) | N/A
| Packages | [![NUGET-PUBLISH-RELEASE](https://github.com/AndreaPic/DevExtremeAI/actions/workflows/dotnet-ci-cd.yml/badge.svg)](https://github.com/AndreaPic/DevExtremeAI/actions/workflows/dotnet-ci-cd.yml) | ![NuGet](https://img.shields.io/nuget/v/DevExtremeAI)

## How to use

To use this library you need OpenAI api key (and optionally organization id).
To obtain the api key you have to register your account to openai.com.
After registered go to your OpenAI Account and search 'View API keys', in this page you can create your apikey.
From your account page you can find the Settings page where is placed your organization ID.

You can use this library via [nuget package DevExtremeAI](https://www.nuget.org/packages/DevExtremeAI/)

**Important**
Note that this library support dot net IConfiguration and dependency injection so it can read apikey and organization from them instead of hard coding in source code. (Please don't hard code apikey and organization id in source code and don't push them to git or any source repository).

## Specs

This library fully adhere to OpenAI specs and its object model is the same of OpenAI (with dotnet peculiarities).
This library also implement OpenAI error codes that aren't documented in OpenAI's APIs Reference.

## asp.net core using examples

Install [nuget package DevExtremeAI](https://www.nuget.org/packages/DevExtremeAI/)
In Program.cs add this using:

```csharp
using DevExtremeAI.AspNet;
```

This using allow you to use the asp.net service extension.
With the webapplication builder now you can use the `AddDevExtremeAI()` method that register all that you need with dependency injection.

```csharp
  var builder = WebApplication.CreateBuilder(args);

  // Add services to the container.
  builder.Services.AddDevExtremeAI();
```

This `AddDevExtremeAI()` overload looks for the apikey in appsettings.json or appsettings.Development.json so you can avoid to hardcode them in source code. I suggest you to use GitHub Action Secrets.

If you prefer you can use the overload `AddDevExtremeAI<TEnvironment>` that require an object type that implement the `DevExtremeAI.Settings.IAIEnvironment` interface so you can read apikey or organization id from where you want. (Your implementation of `IAIEnvironment` will be used in singleton way).

Finally you can use the overload `AddDevExtremeAI(string apiKey, string? organization)` and pass apikey and organization id values but please read them from where you want but don't hardcode in any source code.

That's all! From now you can use OpenAI in your asp.net project via Dependency Injection.

Now you can declare the constructor of your controller with `DevExtremeAI.OpenAIClient.IOpenAIAPIClient` argument like this:

```csharp
  private DevExtremeAI.OpenAIClient.IOpenAIAPIClient _openAIApiClient;
  public TestAIController(DevExtremeAI.OpenAIClient.IOpenAIAPIClient openAIApiClient)
  {
      _openAIApiClient = openAIApiClient;
  }
```

an example of use of IOpenAIAPIClient in controller or apicontroller could be:

```csharp
  // GET api/<TestAIController>/5
  [HttpGet("{id}")]
  public async Task<string> Get(int id)
  {
    var completion = new CreateCompletionRequest();
    completion.Model = "text-davinci-003";
    string prompt = $"Is the number {id} even or odd?";
    completion.AddCompletionPrompt(prompt);
    var response = await _openAIApiClient.CreateCompletionAsync(completion);
    return $"{prompt} -> {response?.OpenAIResponse?.Choices[0]?.Text}";
  }
```

**Note**
You can find the complete documentation of api and DTO in intellisense, examples below or [OpenAI official API Reference](https://platform.openai.com/docs/api-reference) because are the same.

## Using outside asp.net core

If you use outside asp.net core or without HostBuilder or Dependency Injection like in Console Application or dotnet library you can use Factory methods.

In this scenario you need the same package form nuget 'bla bla bla'
After installing this package you can use the DevExtremeAI Library.

In Program.cs ad this using:

```csharp
using DevExtremeAI.OpenAIClient;
```

Now you you can use the `OpenAIClientFactory`

Inside your Library or Main method of the console application you can create an instace of `IOpenAIAPIClient` like in this example:

```csharp
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var openAIClient = OpenAIClientFactory.CreateInstance(); //create an instance o IOpenAIAPIClient

            CreateChatCompletionRequest createCompletionRequest = new CreateChatCompletionRequest();
            createCompletionRequest.Model = "gpt-3.5-turbo";
            createCompletionRequest.Messages.Add(new ChatCompletionRequestMessage()
            {
                Role = ChatCompletionMessageRoleEnum.User,
                Content = "Hello!",
            });

            var response = await openAIClient.CreateChatCompletionAsync(createCompletionRequest);

            Console.WriteLine(response.OpenAIResponse.Choices[0].Message.Content);
        }
    }
```

The factory method (`CreateInstance`) in above example use the overload without arguments, this overload look for apikeyvalue and organization ind in appsettings.json or appsettings.Development.json.

You can use the overload that require an instance of `DevExtremeAI.Settings.IAIEnvironment` implemented by yourself so you can read apikey value and organization id from where you want.

Also you can use the overload that reqire apikey value and organization id as arguments (but pleas don't hardcode them in source code).

However don't hardcode apikey and organization id in any file (source code or appsettings files), don't push into source repository the appsettings.Development.json and use GitHub Action Secrets.

## Request and Response Object Model

### Request

Every methods of `DevExtremeAI.OpenAIClient.IOpenAIAPIClient` are the same of OpenAI, so you can use the  
[official OpenAI  API Reference](https://platform.openai.com/docs/api-reference).
Request DTO objects are described also with standard .net documentation so you can use intellisese.
Every methods of `IOpenAIAPIClient` are present in the xUnit integration tests therefore you can look at them there (DevExtremeAILibTest directory).

### Response

Every response is of `DevExtremeAI.OpenAIDTO.ResponseDTO<T>` type.
This type has three properties:

- `ErrorResponse` that contains the error details returned by OpenAI API in case of problem.
- `HasError` that is true if an error happened otherwise is false.
- `OpenAIResponse` that is the same of the OpenAI response.
  - Every DTO has the standard .net documentation so you can find documentation in intellisense and because are the same of OpenAI you can find documentation in the [official OpenAI  API Reference](https://platform.openai.com/docs/api-reference) also you can look at the integration tests in DevExtremeAILibTest directory.

## API Types

Are covered all OpenAI API types:

- Models
- Completions
- Chat
- Edits
- Imnages
- Embeddings
- Audio
- Files
- Fine-tunes
- Moderations
