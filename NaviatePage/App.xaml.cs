using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NaviatePage.HostBuilders;
using NaviatePage.Models;
using NaviatePage.Stores;
using NaviatePage.ViewModel;
using NaviatePage.ViewModels;
using NaviatePage.Views;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;

namespace NaviatePage
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;
        private readonly NavigationStore _navigationStore;

        public App()
        {
            _host = CreateHostBuilder().AddViewModels().Build();
        }

        public static IHostBuilder CreateHostBuilder(string[] arg = null)
        {
            return Host.CreateDefaultBuilder(arg).AddViewModels();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();
            QuanLyKhoContextFactory quanLyKhoContextFactory = _host.Services.GetService<QuanLyKhoContextFactory>();
            using (QuanLyKhoContext quanLyKho = quanLyKhoContextFactory.CreateDbContext())
            {
                quanLyKho.Database.Migrate();
            }
            Window window = _host.Services.GetRequiredService<MainWindow>();

            window.Show();
            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();
            base.OnExit(e);
        }
    }
}