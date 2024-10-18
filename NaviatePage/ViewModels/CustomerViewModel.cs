using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using NaviatePage.Components;
using NaviatePage.Models;
using NaviatePage.Models.Data;
using NaviatePage.Stores;
using NaviatePage.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace NaviatePage.ViewModels
{
    public partial class CustomerViewModel : ObservableObject
    {
        // Dùng hosting để cung cấp dịch vụ
        private readonly IServiceProvider _serviceProvider;

        #region Property dành cho phân trang và lấy tài nguyên

        [ObservableProperty]
        private ObservableCollection<Customer> _customerList;

        [ObservableProperty]
        private ObservableCollection<Customer> _customerListInPage;

        [ObservableProperty]
        private ObservableCollection<Customer> _customerListTotalPage;

        #endregion Property dành cho phân trang và lấy tài nguyên

        #region Property dành cho đóng mở add edit và load trang

        [ObservableProperty]
        private Customer? _selectedCustomer;

        [ObservableProperty]
        private bool _isAddCustomerViewLoad;

        [ObservableProperty]
        private AddCustomerViewModel _addCustomerVM;

        [ObservableProperty]
        private bool _isOpenModal;

        [ObservableProperty]
        private EditCustomerViewModel _editCustomerVM;

        [ObservableProperty]
        private bool _isEditModalOpen;

        [ObservableProperty]
        private bool _isLoadingData;

        #endregion Property dành cho đóng mở add edit và load trang

        #region Property cho việc lấy tổng số trang, trang hiện tại, tổng số trang khi tìm kiếm

        [ObservableProperty]
        private int _totalPage;

        [ObservableProperty]
        private int _totalCustomer;

        [ObservableProperty]
        private string _currentPage;

        [ObservableProperty]
        private int _pageNumber = 1;

        private string _inputSearch;

        #endregion Property cho việc lấy tổng số trang, trang hiện tại, tổng số trang khi tìm kiếm

        public string InputSearch
        {
            get => _inputSearch;
            set
            {
                _inputSearch = value;
                OnPropertyChanged(nameof(InputSearch));
                if (!string.IsNullOrEmpty(value))
                {
                    SerchCustomerCommand.Execute(null);
                }
                else
                {
                    CustomerListTotalPage = CustomerList;
                    LoadPage(PageNumber, CustomerListTotalPage);
                }
            }
        }

        public CustomerViewModel(IServiceProvider provider)
        {
            _serviceProvider = provider;

            Task.Run(() => InitializeAsync());
        }

        private async Task InitializeAsync()
        {
            IsLoadingData = true;

            // Tải danh sách khách hàng
            CustomerList = await GetAllCustomer();
            CustomerListTotalPage = CustomerList;

            // Cập nhật dữ liệu trang
            await LoadPage(PageNumber, CustomerListTotalPage);
            IsLoadingData = false;
        }

        #region Chức năng thêm xóa sửa, tìm kiếm

        private async Task<ObservableCollection<Customer>> GetAllCustomer()
        {
            var customers = await _serviceProvider.GetRequiredService<IDataService<Customer>>().GetAll();
            return new ObservableCollection<Customer>((IEnumerable<Customer>)customers);
        }

        private async Task AddCustomer(Customer customer)
        {
            try
            {
                Customer newCustomer = await _serviceProvider.GetRequiredService<IDataService<Customer>>().Create(customer);
                if (this.CustomerList != null && newCustomer != null)
                {
                    this.CustomerList.Add(newCustomer);
                }
                await SerchCustomerCommand.ExecuteAsync(null);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Khách hàng này đã tồn tại vui lòng thay đổi", ex);
            }
        }

        [RelayCommand]
        public async Task OpenAddCustomer()
        {
            AddCustomerVM = this._serviceProvider.GetRequiredService<AddCustomerViewModel>();
            AddCustomerVM.SelectedCustomerChanged += async (customer) =>
            {
                IsOpenModal = false;
                try
                {
                    IsLoadingData = true;
                    await AddCustomer(customer);
                    IsLoadingData = false;
                }
                catch (InvalidOperationException ex)
                {
                    System.Windows.MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    IsLoadingData = false;
                }
            };

            AddCustomerVM.CloseAddCustomerViewChanged += () =>
            {
                IsOpenModal = false;
            };
            IsOpenModal = true;
            IsEditModalOpen = false;
        }

        private async Task UpdateCustomer(int id, Customer customer)
        {
            await _serviceProvider.GetRequiredService<IDataService<Customer>>().Update(id, customer);
            int index = CustomerList.IndexOf(CustomerList.FirstOrDefault(x => x.Idcustomer == id));

            if (index != -1)
            {
                CustomerList[index] = customer;
                await SerchCustomerCommand.ExecuteAsync(null);
            }
        }

        [RelayCommand]
        private async Task OpenUpdateCustomer()
        {
            EditCustomerVM = _serviceProvider.GetRequiredService<EditCustomerViewModel>();
            EditCustomerVM.ReceiveSelectedCustomer(SelectedCustomer);
            EditCustomerVM.SelectedCustomerChanged += async (id, customer) =>
            {
                IsLoadingData = true;
                IsEditModalOpen = false;
                await UpdateCustomer(id, customer);
                IsLoadingData = false;
            };
            EditCustomerVM.CloseEditViewModel += () =>
            {
                IsEditModalOpen = false;
            };

            IsEditModalOpen = true;
            IsOpenModal = false;
        }

        [RelayCommand]
        public async Task OpenDeleteCustomer()
        {
            IsLoadingData = true;
            DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("Bạn có thật sử muốn xóa khách hàng này không", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button4);
            if (dialogResult == DialogResult.No)
            {
                IsLoadingData = false;
                return;
            };

            await _serviceProvider.GetRequiredService<IDataService<Customer>>().Delete(SelectedCustomer.Idcustomer);
            CustomerList.Remove(SelectedCustomer);
            await SerchCustomerCommand.ExecuteAsync(null);
            //await LoadPage(PageNumber, CustomerList);
            IsLoadingData = false;
        }

        [RelayCommand]
        private async Task SerchCustomer()
        {
            var customers = CustomerList.Where(x => x.Displayname != null && x.Displayname.ToLower().Contains(InputSearch.ToLower())).ToList();
            CustomerListTotalPage = new ObservableCollection<Customer>(customers);
            PageNumber = 1;
            await LoadPage(PageNumber, CustomerListTotalPage);
        }

        #endregion Chức năng thêm xóa sửa, tìm kiếm

        #region Chức năng khi ấn popup thì selectedCustomer sẽ là cái được chọn

        [RelayCommand]
        private void SelectCustomerPopup(Customer selectedCustomerPopup)
        {
            SelectedCustomer = selectedCustomerPopup;
        }

        #endregion Chức năng khi ấn popup thì selectedCustomer sẽ là cái được chọn

        #region Chức năng phân trang 1 lần chỉ hiện thi đươc 10 tran

        public async Task LoadPage(int pageNumber, ObservableCollection<Customer> customers)
        {
            int pageSize = 5;
            IsLoadingData = true;
            int totalCount = customers.Count;
            TotalPage = (totalCount + pageSize - 1) / pageSize;
            CurrentPage = $"{pageNumber} / {TotalPage}";
            CustomerListInPage = new ObservableCollection<Customer>(customers.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList());
            IsLoadingData = false;
        }

        [RelayCommand]
        private void FirstPage()
        {
            PageNumber = 1;
            Task.Run(() => LoadPage(PageNumber, CustomerListTotalPage)).Wait();
        }

        [RelayCommand]
        private void LastPage()
        {
            PageNumber = TotalPage;
            Task.Run(() => LoadPage(PageNumber, CustomerListTotalPage)).Wait();
        }

        [RelayCommand]
        private void NextPage()
        {
            if (_pageNumber != TotalPage)
            {
                PageNumber += 1;
                Task.Run(() => LoadPage(PageNumber, CustomerListTotalPage)).Wait();
            }
        }

        [RelayCommand]
        private void PreviousPage()
        {
            if (_pageNumber != 1)
            {
                PageNumber -= 1;
                Task.Run(() => LoadPage(PageNumber, CustomerListTotalPage)).Wait();
            }
        }

        #endregion Chức năng phân trang 1 lần chỉ hiện thi đươc 10 tran
    }
}