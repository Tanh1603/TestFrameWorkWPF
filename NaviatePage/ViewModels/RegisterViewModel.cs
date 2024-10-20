using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using NaviatePage.Services;
using NaviatePage.Stores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NaviatePage.ViewModels
{
    public partial class RegisterViewModel : ObservableValidator
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly NavigationStore _navigationStore;

        [ObservableProperty]
        private string _emailTextBox;

        [ObservableProperty]
        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải ít nhất 6 ký tự.")]
        private string _passwordBox;

        [ObservableProperty]
        private string _userName;

        [ObservableProperty]
        private bool _isLoadingData;

        public RegisterViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _navigationStore = _serviceProvider.GetRequiredService<NavigationStore>();
        }

        public event Action<bool> IsSuccessRegister;

        [RelayCommand]
        private async void Submit()
        {
            string a;
            try
            {
                IsLoadingData = true;
                string res = await _serviceProvider.GetRequiredService<FirebaseAuthService>().RegisterUser(EmailTextBox, PasswordBox, UserName);

                if (!string.IsNullOrEmpty(res) && !res.StartsWith("Lỗi"))
                {
                    MessageBox.Show("Bạn đăng kí tài khoản thành công");
                    IsLoadingData = false;
                    _navigationStore.CurrentViewModel = _serviceProvider.GetRequiredService<LoginViewModel>();
                }
                else
                {
                    IsLoadingData = false;
                    MessageBox.Show("Email đã tồn tại");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public event Action<bool> MoveToLogin;

        [RelayCommand]
        private async void MoveLogin()
        {
            _navigationStore.CurrentViewModel = _serviceProvider.GetRequiredService<LoginViewModel>();
        }
    }
}