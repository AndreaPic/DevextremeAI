using DevExtremeAI.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace DevExtremeAI.OpenAIClient
{
    public class OpenAIClientFactory
    {
        /// <summary>
        /// Create a new instance of OpeAI API Client and read api key and organization name in appsettings.json or appsettings.Development.json.
        /// In appsettings the keys to use are: "OPENAI_API_KEY" and "OPENAI_ORGANIZATION".
        /// Please do not push appsettings.Development.json to git and don't hardcode the apikey and organization name and also don't push them to git!!!
        /// </summary>
        /// <returns>
        /// A new instance of OpenAIAPIClient
        /// </returns>
        public static OpenAIAPIClient CreateInstance()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json")
                .Build();

            AIEnvironment openAiEnvironment = new AIEnvironment(config);
            return new(openAiEnvironment);
        }
        /// <summary>
        /// Create a new instance of OpenAI API Client using the input apikey and organization.
        /// Please don't hardcode the apikey and organization name and also don't push them to git!!!
        /// </summary>
        /// <param name="apiKey">your openai api key</param>
        /// <param name="organizationName">optionally your organization name</param>
        /// <returns>
        /// A new instance of OpenAIAPIClient
        /// </returns>
        public static OpenAIAPIClient CreateInstance(string apiKey, string? organizationName) =>
            new(new OpenAIEnvironmentData(apiKey, organizationName));

        /// <summary>
        /// This method return a new instance of OpenAIAPIClient using the apikey and organization name that you provide through the implementation of IAIEnvironment.
        /// Please don't hardcode the apikey and organization name and also don't push them to git!!!
        /// </summary>
        /// <param name="openAiEnvironment">An object that return you apikey value and organization name</param>
        /// <returns>
        /// A new instance of OpenAIAPIClient
        /// </returns>
        public static OpenAIAPIClient CreateInstance(IAIEnvironment openAiEnvironment) => new(openAiEnvironment);

    }
}
