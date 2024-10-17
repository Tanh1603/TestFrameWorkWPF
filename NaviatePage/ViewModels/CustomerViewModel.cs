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
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace NaviatePage.ViewModels
{
    public partial class CustomerViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly NavigationStore _navigationStore;

        [ObservableProperty]
        private ObservableCollection<Customer> _customerList;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(OpenUpdateCustomerCommand))]
        [NotifyCanExecuteChangedFor(nameof(OpenDeleteCustomerCommand))]
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

        public CustomerViewModel(IServiceProvider provider)
        {
            _serviceProvider = provider;
            _navigationStore = provider.GetRequiredService<NavigationStore>();

            Task.Run(() => LoadCustomers());
        }

        private async void LoadCustomers()
        {
            var customers = await _serviceProvider.GetRequiredService<IDataService<Customer>>().GetAll();
            this.CustomerList = new ObservableCollection<Customer>(customers);
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
                    await AddCustomer(customer);
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            }
        }

        [RelayCommand(CanExecute = nameof(OpenUpdateCustomerCanexcute))]
        private async Task OpenUpdateCustomer()
        {
            EditCustomerVM = _serviceProvider.GetRequiredService<EditCustomerViewModel>();
            EditCustomerVM.ReceiveSelectedCustomer(SelectedCustomer);
            EditCustomerVM.SelectedCustomerChanged += async (id, customer) =>
            {
                await UpdateCustomer(id, customer);
                IsEditModalOpen = false;
            };
            EditCustomerVM.CloseEditViewModel += () =>
            {
                IsEditModalOpen = false;
            };

            IsEditModalOpen = true;
            IsOpenModal = false;
        }

        private bool OpenUpdateCustomerCanexcute()
        {
            return SelectedCustomer != null;
        }

        [RelayCommand(CanExecute = nameof(OpenDeleteCustomerCanexcute))]
        public async Task OpenDeleteCustomer()
        {
            await _serviceProvider.GetRequiredService<IDataService<Customer>>().Delete(SelectedCustomer.Idcustomer);
            CustomerList.Remove(SelectedCustomer);
            //LoadCustomers();
        }

        private bool OpenDeleteCustomerCanexcute()
        {
            return SelectedCustomer != null;
        }
    }
}