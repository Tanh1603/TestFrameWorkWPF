using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Office.Interop.Excel;
using NaviatePage.Components;
using NaviatePage.Models;
using NaviatePage.Models.Data;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

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

        #region Property cho việc sắp xếp tăng giảm theo danh mục

        private ComboBoxItem _selectedSortStyleCustomer;

        private int _selectedTypeSortCustomer;

        public ComboBoxItem SelectedSortStyleCustomer
        {
            get => _selectedSortStyleCustomer;
            set
            {
                _selectedSortStyleCustomer = value;
                if (value != null)
                {
                    SortCustomerListCommand?.Execute(null);
                }
            }
        }

        public int SelectedTypeSortCustomer
        {
            get => _selectedTypeSortCustomer;
            set
            {
                _selectedTypeSortCustomer = value;
                if (value != null)
                {
                    SortCustomerListCommand?.Execute(null);
                }
            }
        }

        #endregion Property cho việc sắp xếp tăng giảm theo danh mục

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
                    SortCustomerListCommand.ExecuteAsync(null);
                }
                else
                {
                    CustomerListTotalPage = CustomerList;
                    LoadPage(PageNumber, CustomerListTotalPage);
                    SortCustomerListCommand.ExecuteAsync(null);
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
                if (CustomerList != null && newCustomer != null)
                {
                    CustomerList.Add(newCustomer);
                }
                if (!string.IsNullOrEmpty(InputSearch))
                {
                    await SerchCustomerCommand.ExecuteAsync(null);
                }
                else
                {
                    await LoadPage(PageNumber, CustomerList);
                }
                await SortCustomerListCommand.ExecuteAsync(null);
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
                if (string.IsNullOrEmpty(InputSearch))
                {
                    await LoadPage(PageNumber, CustomerList);
                }
                else
                {
                    await SerchCustomerCommand.ExecuteAsync(null);
                }
            }
            await SortCustomerListCommand.ExecuteAsync(null);
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
            if (!string.IsNullOrEmpty(InputSearch))
            {
                await SerchCustomerCommand.ExecuteAsync(null);
            }
            else
            {
                await LoadPage(PageNumber, CustomerList);
            }
            await SortCustomerListCommand.ExecuteAsync(null);
            IsLoadingData = false;
        }

        [RelayCommand]
        private async Task SerchCustomer()
        {
            if (!string.IsNullOrEmpty(InputSearch))
            {
                //var customers = CustomerList.Where(x => x.Displayname != null && x.Displayname.ToLower().Contains(InputSearch.ToLower())).ToList();
                var customers = CustomerList.Where(x =>
                    (x.Displayname != null && x.Displayname.ToLower().Contains(InputSearch.ToLower())) ||
                    (x.Address != null && x.Address.ToLower().Contains(InputSearch.ToLower())) ||
                    (x.Phone != null && x.Phone.ToLower().Contains(InputSearch.ToLower())) ||
                    (x.Email != null && x.Email.ToLower().Contains(InputSearch.ToLower())) ||
                    (x.Moreinfo != null && x.Moreinfo.ToLower().Contains(InputSearch.ToLower())) ||
                    (x.Contractdate.ToString().ToLower().Contains(InputSearch.ToLower()))
                ).ToList();

                CustomerListTotalPage = new ObservableCollection<Customer>(customers);
                PageNumber = 1;
                await LoadPage(PageNumber, CustomerListTotalPage);
            }
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
            if (pageNumber > TotalPage)
            {
                pageNumber = TotalPage;
            }
            CurrentPage = $"{pageNumber} / {TotalPage}";
            CustomerListInPage = new ObservableCollection<Customer>(customers.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList());
            CustomerListTotalPage = customers;
            IsLoadingData = false;

            if (TotalPage == 0)
            {
                FirstPageCommand.NotifyCanExecuteChanged();
                PreviousPageCommand.NotifyCanExecuteChanged();
                LastPageCommand.NotifyCanExecuteChanged();
                NextPageCommand.NotifyCanExecuteChanged();
            }
        }

        [RelayCommand]
        private void FirstPage()
        {
            if (TotalPage != 0)
            {
                PageNumber = 1;
                Task.Run(() => LoadPage(PageNumber, CustomerListTotalPage)).Wait();
            }
        }

        [RelayCommand]
        private void LastPage()
        {
            if (TotalPage != 0)
            {
                PageNumber = TotalPage;
                Task.Run(() => LoadPage(PageNumber, CustomerListTotalPage)).Wait();
            }
        }

        [RelayCommand]
        private void NextPage()
        {
            if (PageNumber != TotalPage && TotalPage != 0)
            {
                PageNumber += 1;
                Task.Run(() => LoadPage(PageNumber, CustomerListTotalPage)).Wait();
            }
        }

        [RelayCommand]
        private void PreviousPage()
        {
            if (PageNumber != 1 && TotalPage != 0)
            {
                PageNumber -= 1;
                Task.Run(() => LoadPage(PageNumber, CustomerListTotalPage)).Wait();
            }
        }

        #endregion Chức năng phân trang 1 lần chỉ hiện thi đươc 10 tran

        #region Chức năng sắp xếp tăng giảm dần theo danh mục

        [RelayCommand]
        private async Task SortCustomerList()
        {
            if (SelectedSortStyleCustomer != null)
            {
                string tmp = SelectedSortStyleCustomer.Content.ToString();
                switch (tmp)
                {
                    case "Idcustomer":
                        if (SelectedTypeSortCustomer == 0)
                        {
                            CustomerListTotalPage = new ObservableCollection<Customer>(CustomerListTotalPage.OrderBy(x => x.Idcustomer));
                        }
                        else
                        {
                            CustomerListTotalPage = new ObservableCollection<Customer>(CustomerListTotalPage.OrderByDescending(x => x.Idcustomer));
                        }
                        break;

                    case "Displayname":
                        if (SelectedTypeSortCustomer == 0)
                        {
                            CustomerListTotalPage = new ObservableCollection<Customer>(CustomerListTotalPage.OrderBy(x => x.Displayname));
                        }
                        else
                        {
                            CustomerListTotalPage = new ObservableCollection<Customer>(CustomerListTotalPage.OrderByDescending(x => x.Displayname));
                        }
                        break;

                    case "Address":
                        if (SelectedTypeSortCustomer == 0)
                        {
                            CustomerListTotalPage = new ObservableCollection<Customer>(CustomerListTotalPage.OrderBy(x => x.Address));
                        }
                        else
                        {
                            CustomerListTotalPage = new ObservableCollection<Customer>(CustomerListTotalPage.OrderByDescending(x => x.Address));
                        }
                        break;

                    case "Phone":
                        if (SelectedTypeSortCustomer == 0)
                        {
                            CustomerListTotalPage = new ObservableCollection<Customer>(CustomerListTotalPage.OrderBy(x => x.Phone));
                        }
                        else
                        {
                            CustomerListTotalPage = new ObservableCollection<Customer>(CustomerListTotalPage.OrderByDescending(x => x.Phone));
                        }
                        break;

                    case "Email":
                        if (SelectedTypeSortCustomer == 0)
                        {
                            CustomerListTotalPage = new ObservableCollection<Customer>(CustomerListTotalPage.OrderBy(x => x.Email));
                        }
                        else
                        {
                            CustomerListTotalPage = new ObservableCollection<Customer>(CustomerListTotalPage.OrderByDescending(x => x.Email));
                        }
                        break;

                    case "Moreinfo":
                        if (SelectedTypeSortCustomer == 0)
                        {
                            CustomerListTotalPage = new ObservableCollection<Customer>(CustomerListTotalPage.OrderBy(x => x.Moreinfo));
                        }
                        else
                        {
                            CustomerListTotalPage = new ObservableCollection<Customer>(CustomerListTotalPage.OrderByDescending(x => x.Moreinfo));
                        }
                        break;

                    case "Contractdate":
                        if (SelectedTypeSortCustomer == 0)
                        {
                            CustomerListTotalPage = new ObservableCollection<Customer>(CustomerListTotalPage.OrderBy(x => x.Contractdate));
                        }
                        else
                        {
                            CustomerListTotalPage = new ObservableCollection<Customer>(CustomerListTotalPage.OrderByDescending(x => x.Contractdate));
                        }
                        break;

                    default:
                        break;
                }
                await LoadPage(PageNumber, CustomerListTotalPage);
            }
            else
            {
                return;
            }
        }

        #endregion Chức năng sắp xếp tăng giảm dần theo danh mục

        // Chức năng xuất file excel
        [RelayCommand]
        private void ExportExcelFile()
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
                excelApp.Visible = true;
                Workbook workbook = excelApp.Workbooks.Add(Type.Missing);
                Worksheet worksheet = (Worksheet)workbook.Sheets[1];

                worksheet.Cells[1, 1] = "Id";
                worksheet.Cells[1, 2] = "Display Name";
                worksheet.Cells[1, 3] = "Address";
                worksheet.Cells[1, 4] = "Phone";
                worksheet.Cells[1, 5] = "Email";
                worksheet.Cells[1, 6] = "More Info";
                worksheet.Cells[1, 7] = "Contract Date";

                for (int i = 0; i < CustomerList.Count; i++)
                {
                    var customer = CustomerList[i];
                    worksheet.Cells[i + 2, 1] = customer.Idcustomer;
                    worksheet.Cells[i + 2, 2] = customer.Displayname;
                    worksheet.Cells[i + 2, 3] = customer.Address;
                    worksheet.Cells[i + 2, 4] = customer.Phone;
                    worksheet.Cells[i + 2, 5] = customer.Email;
                    worksheet.Cells[i + 2, 6] = customer.Moreinfo;
                    worksheet.Cells[i + 2, 7] = customer.Contractdate;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}