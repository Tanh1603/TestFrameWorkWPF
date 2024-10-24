using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic.Logging;
using NaviatePage.Stores;
using NaviatePage.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NaviatePage.ViewModel
{
    public partial class MainViewModel : ObservableObject
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
            CurrentViewModel = _serviceProvider.GetRequiredService<NavigateViewModel>();
            //CurrentViewModel = _serviceProvider.GetRequiredService<LoginViewModel>();
            //CurrentViewModel = _serviceProvider.GetRequiredService<RegisterViewModel>();
            _navigationStore.CurrentViewModelChanged += () =>
            {
                CurrentViewModel = _navigationStore.CurrentViewModel;
            };
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

        [RelayCommand]
        private void DragSizeWindow()
        {
            var window = Application.Current.MainWindow;
            if (window != null)
            {
                window.DragMove();
                window.ResizeMode = ResizeMode.CanResize;
            }
        }
    }
}