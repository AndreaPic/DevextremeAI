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
        public static OpenAIAPIClient CreateInstance()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            AIEnvironment openAiEnvironment = new AIEnvironment(config);
            return new(openAiEnvironment);
        }
        public static OpenAIAPIClient CreateInstance(string organizationName, string apiKey) =>
            new(new OpenAIEnvironmentData(organizationName, apiKey));

        public static OpenAIAPIClient CreateInstance(IAIEnvironment openAiEnvironment) => new(openAiEnvironment);

    }
}
