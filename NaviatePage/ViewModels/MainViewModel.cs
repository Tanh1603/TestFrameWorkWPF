using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using NaviatePage.Stores;
using NaviatePage.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NaviatePage.ViewModel
{
    public partial class MainViewModel : ObservableObject, IDisposable
    {
        private readonly NavigationStore _navigationStore;
        private IServiceProvider _serviceProvider;

        [ObservableProperty]
        private ObservableObject _currentViewModel;

        [ObservableProperty]
        private string _title;

        public MainViewModel(IServiceProvider provider)
        {
            _serviceProvider = provider;
            _navigationStore = provider.GetRequiredService<NavigationStore>();
            _navigationStore.CurrentViewModelChanged += () =>
            {
                OnCurrentViewModelChanged(CurrentViewModel);
                CurrentViewModel = _navigationStore.CurrentViewModel;
            };
            CurrentViewModel = _serviceProvider.GetRequiredService<HomViewModel>();
        }

        [RelayCommand]
        private void TabSelectionChanged(string type)
        {
            switch (type)
            {
                case "Home":
                    //CurrentViewModel = _serviceProvider.GetRequiredService<HomViewModel>();
                    _navigationStore.CurrentViewModel = _serviceProvider.GetRequiredService<HomViewModel>(); ;
                    break;

                case "Customer":
                    //CurrentViewModel = _serviceProvider.GetRequiredService<CustomerViewModel>();
                    _navigationStore.CurrentViewModel = _serviceProvider.GetRequiredService<CustomerViewModel>();
                    break;

                case "Order":
                    //CurrentViewModel = _serviceProvider.GetRequiredService<OrderViewModel>();
                    _navigationStore.CurrentViewModel = _serviceProvider.GetRequiredService<OrderViewModel>();
                    break;

                case "Transaction":
                    //CurrentViewModel = _serviceProvider.GetRequiredService<TransactionViewModel>();
                    _navigationStore.CurrentViewModel = _serviceProvider.GetRequiredService<TransactionViewModel>(); ;
                    break;

                case "Shipment":
                    //CurrentViewModel = _serviceProvider.GetRequiredService<ShipmentViewModel>();
                    _navigationStore.CurrentViewModel = _serviceProvider.GetRequiredService<ShipmentViewModel>();
                    break;

                case "Setting":
                    //CurrentViewModel = _serviceProvider.GetRequiredService<SettingViewModel>();
                    _navigationStore.CurrentViewModel = _serviceProvider.GetRequiredService<SettingViewModel>();
                    break;

                case "Product":
                    CurrentViewModel = _serviceProvider.GetRequiredService<ProductViewModel>();
                    _navigationStore.CurrentViewModel = _serviceProvider.GetRequiredService<ProductViewModel>();
                    break;

                default:
                    //CurrentViewModel = _serviceProvider.GetRequiredService<HomViewModel>();
                    _navigationStore.CurrentViewModel = _serviceProvider.GetRequiredService<HomViewModel>(); ;
                    break;
            }
        }

        [RelayCommand]
        private void CloseWindow()
        {
            Window? window = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.DataContext == this);
            window?.Close();
        }

        [RelayCommand]
        private void MiniMizeWindow()
        {
            Window? window = Application.Current.Windows.OfType<Window>().First(w => w.DataContext == this);
            window.WindowState = WindowState.Minimized;
        }

        [RelayCommand]
        private void MaximizedWindow()
        {
            Window? window = Application.Current.Windows.OfType<Window>().First(w => w.DataContext == this);
            if (window.WindowState == WindowState.Normal)
            {
                window.WindowState = WindowState.Maximized;
            }
            else if (window.WindowState == WindowState.Maximized)
            {
                window.WindowState = WindowState.Normal;
            }
        }

        public void Dispose()
        {
        }
    }
}