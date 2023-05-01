using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace DevextremeAI.Settings
{
    public interface IAIEnvironment
    {
        public string GetOrganization();
        public string GetApiKey();
    }

    public class AIEnvironment : IAIEnvironment
    {
        private IConfiguration CurrentConfiguration { get; set; }
        public AIEnvironment(IConfiguration configuration)
        {
            CurrentConfiguration = configuration;

        }

        public string? GetOrganization()
        {
            return CurrentConfiguration.GetValue<string>("OPENAI-ORGANIZATION");
        }

        public string? GetApiKey()
        {
            return CurrentConfiguration.GetValue<string>("OPENAI_API_KEY");
        }
    }

}
