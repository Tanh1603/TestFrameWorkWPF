using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using NaviatePage.Services;
using NaviatePage.Stores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NaviatePage.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly NavigationStore _navigationStore;
        private string code;

        [ObservableProperty]
        private string _verificationCode;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(OpenSubmitCommand))]
        private string _emailTextBox;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(OpenSubmitCommand))]
        private string _passwordBox;

        [ObservableProperty]
        private string _userName;

        [ObservableProperty]
        private bool _isLoadingData;

        [ObservableProperty]
        private bool _isSubmitCode;

        public RegisterViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _navigationStore = _serviceProvider.GetRequiredService<NavigationStore>();
        }

        public event Action<bool> IsSuccessRegister;

        private string GenerateVerificationCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString(); ;  // Tạo mã xác thực 6 chữ số
        }

        public async Task<bool> SendVerificationCodeAsync(string email)
        {
            var verificationCode = GenerateVerificationCode();
            code = verificationCode;
            try
            {
                using (var smtpClient = new SmtpClient("smtp.gmail.com"))
                {
                    smtpClient.Port = 587;
                    smtpClient.Credentials = new System.Net.NetworkCredential("tuananh48218@gmail.com", "ktrc uoha hupj dhvg");
                    smtpClient.EnableSsl = true;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("tuananh48218@gmail"),
                        Subject = "Mã xác thực của bạn",
                        Body = $"Mã xác thực của bạn là: {verificationCode}",
                        IsBodyHtml = false,
                    };

                    mailMessage.To.Add(email);

                    await smtpClient.SendMailAsync(mailMessage);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task SubmitRegister()
        {
            try
            {
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

        [RelayCommand(CanExecute = nameof(OpenSubmitCanexcute))]
        private async Task OpenSubmit()
        {
            IsLoadingData = true;
            bool isSending = await SendVerificationCodeAsync(EmailTextBox);
            if (!isSending)
            {
                MessageBox.Show("Email không tồn tại vui lòng tạo lại tài khoản");
                IsLoadingData = false;
                return;
            }
            IsSubmitCode = true;
            IsLoadingData = false;
        }

        private bool OpenSubmitCanexcute()
        {
            bool isEmailValid = !string.IsNullOrWhiteSpace(EmailTextBox) &&
                                Regex.IsMatch(EmailTextBox, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            bool isPasswordValid = !string.IsNullOrWhiteSpace(PasswordBox) &&
                                   PasswordBox.Length >= 6;

            return isEmailValid && isPasswordValid;
        }

        public event Action<bool> MoveToLogin;

        [RelayCommand]
        private async void MoveLogin()
        {
            _navigationStore.CurrentViewModel = _serviceProvider.GetRequiredService<LoginViewModel>();
        }

        [RelayCommand]
        private void Close()
        {
            IsSubmitCode = false;
        }

        [RelayCommand]
        private async void SubmitVerificationCode()
        {
            if (VerificationCode != code)
            {
                MessageBox.Show("Mã xác thực sai vui lòng nhập lại");
            }
            else
            {
                IsLoadingData = true;
                await SubmitRegister();
                IsLoadingData = false;
                IsSubmitCode = false;
            }
        }
    }
}