using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using Microsoft.Extensions.DependencyInjection;
using NaviatePage.Services;
using NaviatePage.Stores;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace NaviatePage.ViewModels
{
    public partial class NavigateViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;

        [ObservableProperty]
        private ObservableObject _currentViewModel;

        private readonly NavigationStore _navigationStore;
        private readonly FirebaseAuthClient _firebaseAuthClient;

        [ObservableProperty]
        private string _userName;

        [ObservableProperty]
        private int _selectedIndexTabControl;

        [ObservableProperty]
        private BitmapImage _selectedImage;

        [ObservableProperty]
        private bool _isLeftDrawerOpen;

        public NavigateViewModel(IServiceProvider provider)
        {
            _serviceProvider = provider;
            _navigationStore = _serviceProvider.GetRequiredService<NavigationStore>();
            UserName = _serviceProvider.GetRequiredService<FirebaseAuthService>().GetDisplayName() ?? "Unknow User";
            CurrentViewModel = _serviceProvider.GetRequiredService<HomViewModel>();
            SelectedIndexTabControl = 0;
        }

        [RelayCommand]
        private void TabSelectionChanged(string type)
        {
            switch (type)
            {
                case "Home":
                    CurrentViewModel = _serviceProvider.GetRequiredService<HomViewModel>();
                    break;

                case "Customer":
                    CurrentViewModel = _serviceProvider.GetRequiredService<CustomerViewModel>();
                    break;

                case "Order":
                    CurrentViewModel = _serviceProvider.GetRequiredService<OrderViewModel>();
                    break;

                case "Transaction":
                    CurrentViewModel = _serviceProvider.GetRequiredService<TransactionViewModel>();
                    break;

                case "Shipment":
                    CurrentViewModel = _serviceProvider.GetRequiredService<ShipmentViewModel>();
                    break;

                case "Setting":
                    CurrentViewModel = _serviceProvider.GetRequiredService<SettingViewModel>();
                    break;

                case "Product":
                    CurrentViewModel = _serviceProvider.GetRequiredService<ProductViewModel>();
                    break;

                default:
                    CurrentViewModel = _serviceProvider.GetRequiredService<HomViewModel>();
                    break;
            }

            IsLeftDrawerOpen = false;
        }

        [RelayCommand]
        private void LogoutCount()
        {
            _navigationStore.CurrentViewModel = _serviceProvider.GetRequiredService<LoginViewModel>();
            _serviceProvider.GetRequiredService<FirebaseAuthService>().SignOut();
        }

        [RelayCommand]
        private void ChangeImage()
        {
            string filter = "Image files (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp|All files (*.*)|*.*";
            var filePath = _serviceProvider.GetRequiredService<IFileDialogService>().OpenFileDialog(filter);

            if (!string.IsNullOrEmpty(filePath))
            {
                SelectedImage = new BitmapImage(new Uri(filePath));
            }
        }

        [RelayCommand]
        private void MenuToggleButtonOpen()
        {
            IsLeftDrawerOpen = true;
        }
    }
}