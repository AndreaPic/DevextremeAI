using DevExtremeAI.OpenAIClient;
using DevExtremeAI.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevExtremeAI.AspNet
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// With this extension openai apikey value and organization value are readed from standard asp.net core settings
        /// Don't push to appsettings.json or appsettings.Development.json with values to git!!! 
        /// the key for the settings are:
        /// OPENAI_API_KEY for apiKey
        /// OPENAI-ORGANIZATION for organization
        /// </summary>
        /// <param name="services">asp.net core service to extend</param>
        /// <returns>configured service</returns>
        public static IServiceCollection AddDevextremeAI(this IServiceCollection services)
        {

            services.AddHttpClient("DevextremeAI");
            services.AddSingleton<IAIEnvironment, AIEnvironment>();
            services.AddTransient<IOpenAIAPIClient, OpenAIAPIClient>();

            return services;
        }

        /// <summary>
        /// With this extension you have to implement IAIEnvironment interface.
        /// Its instance will be used as singleton and must return apikey value and optionally organization name.
        /// Don't push to appsettings.json or appsettings.Development.json with values to git!!! 
        /// It's your choice what use to store the apikey but pleas don't push in git!!!
        /// </summary>
        /// <typeparam name="TEnvironment">Object that return apikey and organization name</typeparam>
        /// <param name="services">asp.net core service to extend</param>
        /// <returns>configured service</returns>
        public static IServiceCollection AddDevextremeAI<TEnvironment>(this IServiceCollection services)
            where  TEnvironment : class, IAIEnvironment, new()
        {

            services.AddHttpClient("DevextremeAI");
            services.AddSingleton<IAIEnvironment, TEnvironment>();
            services.AddTransient<IOpenAIAPIClient, OpenAIAPIClient>();

            return services;
        }

        /// <summary>
        /// With this extension you provide the organization and apikeyvalue.
        /// It's your choice what use to store the apikey but pleas don't push in git!!!
        /// </summary>
        /// <param name="services">service to extend</param>
        /// <param name="organization">OpenAI organization name</param>
        /// <param name="apiKey">OpenAI api key value</param>
        /// <returns></returns>
        public static IServiceCollection AddDevextremeAI(this IServiceCollection services, string organization, string apiKey)
        {

            services.AddHttpClient("DevextremeAI");
            services.AddSingleton<IAIEnvironment>( s => new OpenAIEnvironmentData(organization,apiKey));
            services.AddTransient<IOpenAIAPIClient, OpenAIAPIClient>();

            return services;
        }

    }
}
