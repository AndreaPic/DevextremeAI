# DevextremeAI

## Overview

DevextremeAI is a library with full and complete implementation of all OpenAI's APIs.
This library is fully compliant to openAI specs and also implement openAI error response.
It's very easy to use with asp.net core and has full support to dependency injection (with a single line of code as asp.net standard pattern).
It's easy to use also in libraries without dependency injection (see samples below).

*Please note that this is **unofficial** OpenAPI library* (It's not mantained by OpenAI Company).

## How to use

To use this library you need OpenAI api key (and optionally organization id).
To obtain the api key you have to register your account to openai.com.
After registered go to your OpenAI Account and search 'View API keys', in this page you can create your apikey.
From your account page you can find the Settings page where is placed your organization ID.

You can use this library via nuget package: "bla bla bla"

**Important**
Note that this library support dot net IConfiguration and dependency injection so it can read apikey and organization from them instead of hard coding in source code. (Please don't hard code apikey and organization id in source code and don't push them to git or any source repository).

## Specs

This library fully adhere to OpenAI specs and its object model is the same of OpenAI (with dotnet peculiarities).
This library also implement OpenAI error codes that aren't documented in OpenAI's APIs Reference.

## asp.net core using examples

Install nuget package "bla bla bla"
In Program.cs ad this using:

```csharp
using DevExtremeAI.AspNet;
```

This using allow you to use the asp.net service extension.
With the webapplication builder now you can use the `AddDevextremeAI()` method that register all that you need with dependency injection.

```csharp
  var builder = WebApplication.CreateBuilder(args);

  // Add services to the container.
  builder.Services.AddDevExtremeAI();
```

This `AddDevextremeAI()` overload looks for the apikey in appsettings.json or appsettings.Development.json so you can avoid to hardcode them in source code. I suggest you to use GitHub Action Secrets.

If you prefer you can use the overload `AddDevextremeAI<TEnvironment>` that require an object type that implement the `DevExtremeAI.Settings.IAIEnvironment` interface so you can read apikey or organization id from where you want. (Your implementation of `IAIEnvironment` will be used in singleton way).

Finally you can use the overload `AddDevextremeAI(string apiKey, string? organization)` and pass apikey and organization id values but please read them from where you want but don't hardcode in any source code.

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
You can find the complete documentation of api and DTO in intellisence, examples below or [OpenAI official API Reference](https://platform.openai.com/docs/api-reference) because are the same.

## Using outside asp.net core

If you use outside asp.net core or without HostBuilder or Dependency Injection like in Console Application or dotnet library you can use Factory methods.

In this scenario you need the same package form nuget 'bla bla bla'
After installing this package you can use the DevExtremeAI Library.

In Program.cs ad this using:

```csharp
using DevExtremeAI.OpenAIClient;
```

This using allow you to use the `OpenAIClientFactory`

