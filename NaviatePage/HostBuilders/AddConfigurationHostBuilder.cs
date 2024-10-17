using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaviatePage.HostBuilders
{
    internal static class AddConfigurationHostBuilder
    {
        public static IHostBuilder AddConfiguration(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureAppConfiguration(services =>
            {
                services.AddJsonFile("appsettings.json");
                services.AddEnvironmentVariables();
            });
            return hostBuilder;
        }
    }
}