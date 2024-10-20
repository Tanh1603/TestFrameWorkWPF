using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using Firebase.Auth.Requests;
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
        private string _resetEmail;

        [ObservableProperty]
        private bool _isLoadingData;

        [ObservableProperty]
        private bool _isResetPassword;

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

        [RelayCommand]
        private async void OpenResetPassword()
        {
            IsResetPassword = true;
        }

        [RelayCommand]
        private void CloseResetPassword() => IsResetPassword = false;

        [RelayCommand]
        private async void SubmitResetPassword()
        {
            try
            {
                IsLoadingData = true;
                bool isSuccess = await _serviceProvider.GetRequiredService<FirebaseAuthService>().ResetPassword(ResetEmail);
                IsLoadingData = false;
                if (isSuccess)
                {
                    MessageBox.Show("Thay đổi mật khẩu thành công");
                }
                else
                {
                    MessageBox.Show("Email không tồn tại");
                }
            }
            catch (FirebaseAuthException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}