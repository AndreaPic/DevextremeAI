using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevextremeAI.Settings;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;

namespace DevextremeAILibTest
{
    //public partial class Program
    //{
    //    public static IWebHostBuilder CreateWebHostBuilder() =>
    //        WebHost.CreateDefaultBuilder();
    //}
    public class TestApplication : WebApplicationFactory<DevextremeAIWebApp.Program>
    {

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {

            base.ConfigureWebHost(builder);
            //builder.UseEnvironment("Development");
            //builder.ConfigureAppConfiguration(config =>
            //{
            //    config.SetBasePath(Path.GetDirectoryName(typeof(DevextremeAI.Settings.AIEnvironment).Assembly.Location))
            //        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //        .AddJsonFile($"appsettings.Development.json", optional: true)
            //        .AddEnvironmentVariables();
            //});
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            return base.CreateHost(builder);
        }

        protected override IHostBuilder? CreateHostBuilder()
        {
            return base.CreateHostBuilder();
        }

        protected override IWebHostBuilder? CreateWebHostBuilder()
        {
            return base.CreateWebHostBuilder();
        }

        protected override TestServer CreateServer(IWebHostBuilder builder)
        {
            return base.CreateServer(builder);
        }

    }
}
