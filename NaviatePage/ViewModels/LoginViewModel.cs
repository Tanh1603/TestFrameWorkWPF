using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using NaviatePage.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaviatePage.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly NavigationStore _navigationStore;

        public LoginViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _navigationStore = _serviceProvider.GetRequiredService<NavigationStore>();
        }

        public event Action<bool> IsSuccessLogin;

        [RelayCommand]
        private async void Submit()
        {
            _navigationStore.CurrentViewModel = _serviceProvider.GetRequiredService<NavigateViewModel>();
        }

        public event Action<bool> MoveToRegister;

        [RelayCommand]
        private async void MoveRegister()
        {
            _navigationStore.CurrentViewModel = _serviceProvider.GetRequiredService<RegisterViewModel>();
        }
    }
}