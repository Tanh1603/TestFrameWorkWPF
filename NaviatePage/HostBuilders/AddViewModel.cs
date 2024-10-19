using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NaviatePage.Components;
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
                services.AddTransient<AddCustomerViewModel>(provider => new AddCustomerViewModel(provider));
                services.AddTransient<EditCustomerViewModel>();

                services.AddSingleton<NavigationStore>((s) => new NavigationStore(s));

                services.AddTransient<LoginViewModel>((s) => new LoginViewModel(s));
                services.AddTransient<RegisterViewModel>(s => new RegisterViewModel(s));
                services.AddTransient<NavigateViewModel>(s => new NavigateViewModel(s));

                services.AddSingleton<IDataService<Customer>, GenericDataService<Customer>>();
                services.AddSingleton<MainWindow>(provider => new MainWindow(provider.GetRequiredService<MainViewModel>()));
            });

            return hostBuilder;
        }
    }
}