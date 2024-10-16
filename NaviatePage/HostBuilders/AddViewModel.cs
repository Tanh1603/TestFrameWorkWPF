using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NaviatePage.Models;
using NaviatePage.Models.Data;
using NaviatePage.Stores;
using NaviatePage.ViewModel;
using NaviatePage.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaviatePage.HostBuilders
{
    public static class AddViewModel
    {
        public static IHostBuilder AddViewModels(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddTransient<HomViewModel>();
                services.AddTransient<OrderViewModel>();
                services.AddTransient<ProductViewModel>();
                services.AddTransient<SettingViewModel>();
                services.AddTransient<ShipmentViewModel>();
                services.AddTransient<TransactionViewModel>();

                services.AddTransient<CustomerViewModel>(provider => new CustomerViewModel(provider));
                services.AddTransient<MainViewModel>(provider => new MainViewModel(provider));

                services.AddSingleton<NavigationStore>(provider => new NavigationStore(provider.GetRequiredService<HomViewModel>()));

                services.AddSingleton<MainWindow>(provider => new MainWindow(provider.GetRequiredService<MainViewModel>()));
                services.AddSingleton<ViewModelLocator>(provider => new ViewModelLocator(provider));

                string connectionString = "Host=ep-lingering-art-a4mvt05a-pooler.us-east-1.aws.neon.tech;Port=5432;Database=verceldb;Username=default;Password=tkqSdm4wyja1;SSL Mode=Require;Trust Server Certificate=true;";
                Action<DbContextOptionsBuilder> configureDbContext = o => o.UseNpgsql(connectionString);

                services.AddDbContext<QuanLyKhoContext>(configureDbContext);
                services.AddSingleton<QuanLyKhoContextFactory>(new QuanLyKhoContextFactory(configureDbContext));

                services.AddSingleton<IDataService<Customer>, GenericDataService<Customer>>();
            });

            return hostBuilder;
        }
    }
}