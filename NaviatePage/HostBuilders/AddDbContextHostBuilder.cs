using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NaviatePage.Models;
using NaviatePage.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaviatePage.HostBuilders
{
    internal static class AddDbContextHostBuilder
    {
        public static IHostBuilder AddDbContext(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((context, services) =>
            {
                string connectionString = context.Configuration.GetConnectionString("postgreSql");
                Action<DbContextOptionsBuilder> configureDbContext = o => o.UseNpgsql(connectionString);
                services.AddDbContext<QuanLyKhoContext>(configureDbContext);
                services.AddSingleton<QuanLyKhoContextFactory>(new QuanLyKhoContextFactory(configureDbContext));

                string firebaseAPIKey = context.Configuration.GetValue<string>("FIREBASE_API_KEY");

                services.AddSingleton(new FirebaseAuthService(firebaseAPIKey));
            });
            return hostBuilder;
        }
    }
}