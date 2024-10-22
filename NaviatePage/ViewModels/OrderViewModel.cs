using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NaviatePage.Models;
using NaviatePage.Models.Data;
using NaviatePage.Services;
using NaviatePage.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace NaviatePage.ViewModels
{
    public partial class OrderViewModel : ObservableObject
    {
        private IServiceProvider _serviceProvider;
        private IFileDialogService _fileDialogService;
        private string saveImage;

        [ObservableProperty]
        private ObservableCollection<Food> _foodList;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddNewFoodCommand))]
        private string _displayName;

        [ObservableProperty]
        private string _price;

        [ObservableProperty]
        private string _discount;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddNewFoodCommand))]
        private BitmapImage _imagePath;

        [ObservableProperty]
        private bool _isLoadingData;

        public OrderViewModel(IServiceProvider provider)
        {
            _serviceProvider = provider;
            Task.Run(() => LoadFood());
        }

        private async Task LoadFood()
        {
            IsLoadingData = true;
            var res = await _serviceProvider.GetRequiredService<IDataService<Food>>().GetAll();
            Task.Run(() => FoodList = new ObservableCollection<Food>(res)).Wait();
            IsLoadingData = false;
        }

        [RelayCommand(CanExecute = nameof(AddNewFoodCanexcute))]
        private async Task AddNewFood()
        {
            IsLoadingData = true;
            string res = await _serviceProvider.GetRequiredService<FirebaseStorageService>().UploadFileAsync(ImagePath.ToString());

            var newCustomer = new Food()
            {
                Displayname = DisplayName,
                Price = Convert.ToInt64(Price),
                Discount = Convert.ToInt64(Discount),
                Imagepath = res
            };
            await _serviceProvider.GetRequiredService<IDataService<Food>>().Create(newCustomer);
            FoodList.Add(newCustomer);
            IsLoadingData = false;
        }

        private bool AddNewFoodCanexcute()
        {
            return !string.IsNullOrEmpty(ImagePath?.ToString()) && !string.IsNullOrEmpty(DisplayName?.ToString());
        }

        [RelayCommand]
        private void SelectImage()
        {
            string filter = "Image files (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp|All files (*.*)|*.*";
            var filePath = _serviceProvider.GetRequiredService<IFileDialogService>().OpenFileDialog(filter);

            if (!string.IsNullOrEmpty(filePath))
            {
                ImagePath = new BitmapImage(new Uri(filePath));
            }
        }
    }
}