using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace DevExtremeAI.Settings
{
    /// <summary>
    /// Names used in environment variables or setting
    /// </summary>
    internal static class EnvironmentNames
    {
        /// <summary>
        /// Environment variable or settings name for organization id
        /// </summary>
        public const string OrganizationEnvironmentName = "OPENAI_ORGANIZATION";
        /// <summary>
        /// Environment variable or settings name for api key
        /// </summary>
        public const string ApiKeyEnvironmentName = "OPENAI_API_KEY";

    }

    /// <summary>
    /// Interface to implement or register via DI to safe handle openai api key and orgnanization id
    /// </summary>
    public interface IAIEnvironment
    {
        /// <summary>
        /// return organization id
        /// </summary>
        /// <returns>organization id</returns>
        public string GetOrganization();
        /// <summary>
        /// return api key
        /// </summary>
        /// <returns>
        /// api key
        /// </returns>
        public string GetApiKey();
    }

    /// <summary>
    /// base class of the objects that handle apikey and organization id
    /// </summary>
    public abstract class BaseAIEnvironment: IAIEnvironment
    {
        /// <summary>
        /// return organization id
        /// </summary>
        /// <returns>organization id</returns>
        public abstract string? GetOrganization();
        /// <summary>
        /// return api key
        /// </summary>
        /// <returns>
        /// api key
        /// </returns>
        public abstract string? GetApiKey();
    }

    /// <summary>
    /// Object that read apikey and organization id from current IConfiguration
    /// </summary>
    public class AIEnvironment : BaseAIEnvironment
    {
        /// <summary>
        /// IConfiguration to handle settings
        /// </summary>
        private IConfiguration CurrentConfiguration { get; set; }

        /// <summary>
        /// Initialize the object with IConfiguration to handle settings
        /// </summary>
        /// <param name="configuration"></param>
        public AIEnvironment(IConfiguration configuration)
        {
            CurrentConfiguration = configuration;

        }

        /// <summary>
        /// return organization id reading it from setting with key OPENAI_ORGANIZATION
        /// </summary>
        /// <returns>organization id</returns>
        public override string? GetOrganization()
        {
            return CurrentConfiguration.GetValue<string>(EnvironmentNames.OrganizationEnvironmentName);
        }

        /// <summary>
        /// return api key reding it form settings with key OPENAI_API_KEY
        /// </summary>
        /// <returns>
        /// api key
        /// </returns>
        public override string? GetApiKey()
        {
            return CurrentConfiguration.GetValue<string>(EnvironmentNames.ApiKeyEnvironmentName);
        }
    }

    /// <summary>
    /// Object that use apikey and organization id that you provide and handle
    /// </summary>
    public class OpenAIEnvironmentData : BaseAIEnvironment
    {
        /// <summary>
        /// Current Organization Id
        /// </summary>
        private string _organizationId;
        /// <summary>
        /// Current api key
        /// </summary>
        private string _apiKey;

        /// <summary>
        /// Initialize the instance with apikey and organization id
        /// </summary>
        /// <param name="apiKey">api key to use</param>
        /// <param name="organizationId">organization id to use</param>
        public OpenAIEnvironmentData(string apiKey, string? organizationId)
        {
            _organizationId = organizationId;
            _apiKey = apiKey;
        }
        /// <summary>
        /// return organization id
        /// </summary>
        /// <returns>organization id</returns>
        public override string? GetOrganization()
        {
            return _organizationId;
        }

        /// <summary>
        /// return api key
        /// </summary>
        /// <returns>
        /// api key
        /// </returns>
        public override string? GetApiKey()
        {
            return _apiKey;
        }
    }

    /// <summary>
    /// Object that read apikey and organization id from current environment variables
    /// </summary>
    public class CurrentEnvironmentData : BaseAIEnvironment
    {
        /// <summary>
        /// return organization id reading it from environment variable named OPENAI_ORGANIZATION
        /// </summary>
        /// <returns>organization id</returns>
        public override string? GetOrganization()
        {
            return Environment.GetEnvironmentVariable(EnvironmentNames.OrganizationEnvironmentName);
        }

        /// <summary>
        /// return api key reding it from environment variable named OPENAI_API_KEY
        /// </summary>
        /// <returns>
        /// api key
        /// </returns>
        public override string? GetApiKey()
        {
            return Environment.GetEnvironmentVariable(EnvironmentNames.ApiKeyEnvironmentName);
        }
    }

}
