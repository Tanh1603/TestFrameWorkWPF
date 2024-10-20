using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using NaviatePage.Services;
using NaviatePage.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NaviatePage.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly NavigationStore _navigationStore;

        [ObservableProperty]
        private string _emailTextBox;

        [ObservableProperty]
        private string _passwordBox;

        [ObservableProperty]
        private bool _isLoadingData;

        public LoginViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _navigationStore = _serviceProvider.GetRequiredService<NavigationStore>();
        }

        public event Action<bool> IsSuccessLogin;

        [RelayCommand]
        private async void Submit()
        {
            try
            {
                IsLoadingData = true;
                string res = await _serviceProvider.GetRequiredService<FirebaseAuthService>().LoginUser(EmailTextBox, PasswordBox);
                if (!string.IsNullOrEmpty(res))
                {
                    IsLoadingData = false;
                    //MessageBox.Show("Bạn đăng nhập thành công");
                    _navigationStore.CurrentViewModel = _serviceProvider.GetRequiredService<NavigateViewModel>();
                }
                else
                {
                    IsLoadingData = false;
                    MessageBox.Show("Tài khoản mật hoặc khẩu sai");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public event Action<bool> MoveToRegister;

        [RelayCommand]
        private async void MoveRegister()
        {
            _navigationStore.CurrentViewModel = _serviceProvider.GetRequiredService<RegisterViewModel>();
        }
    }
}