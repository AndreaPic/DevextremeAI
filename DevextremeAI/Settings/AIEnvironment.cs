using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace DevExtremeAI.Settings
{
    public interface IAIEnvironment
    {
        public string GetOrganization();
        public string GetApiKey();
    }

    public abstract class BaseAIEnvironment: IAIEnvironment
    {
        public abstract string? GetOrganization();
        public abstract string? GetApiKey();
    }

    public class AIEnvironment : BaseAIEnvironment
    {
        private IConfiguration CurrentConfiguration { get; set; }
        public AIEnvironment(IConfiguration configuration)
        {
            CurrentConfiguration = configuration;

        }

        public override string? GetOrganization()
        {
            return CurrentConfiguration.GetValue<string>("OPENAI-ORGANIZATION");
        }

        public override string? GetApiKey()
        {
            return CurrentConfiguration.GetValue<string>("OPENAI_API_KEY");
        }
    }

    public class OpenAIEnvironmentData : BaseAIEnvironment
    {
        private string _organization;
        private string _apiKey;
        public OpenAIEnvironmentData(string apiKey, string? organization)
        {
            _organization = organization;
            _apiKey = apiKey;
        }
        public override string? GetOrganization()
        {
            return _organization;
        }

        public override string? GetApiKey()
        {
            return _apiKey;
        }
    }

}
