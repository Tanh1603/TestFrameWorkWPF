using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NaviatePage.Models.Data;
using NaviatePage.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaviatePage.Components
{
    public partial class EditCustomerViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _id;

        [ObservableProperty]
        private string _displayName;

        [ObservableProperty]
        private string _address;

        [ObservableProperty]
        private string _phone;

        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _moreInfo;

        [ObservableProperty]
        private DateTime? _contractDate;

        [ObservableProperty]
        private Customer _selectedCustomer;

        public event Action<int, Customer> SelectedCustomerChanged;

        public event Action CloseEditViewModel;

        [RelayCommand]
        private void Submit()
        {
            Customer customer = new Customer()
            {
                Idcustomer = int.Parse(this.Id),
                Displayname = this.DisplayName,
                Address = this.Address,
                Phone = this.Phone,
                Email = this.Email,
                Moreinfo = this.MoreInfo,
                Contractdate = this.ContractDate,
            };
            SelectedCustomerChanged?.Invoke(customer.Idcustomer, customer);
        }

        [RelayCommand]
        private void Close()
        {
            CloseEditViewModel?.Invoke();
        }

        public void ReceiveSelectedCustomer(Customer customer)
        {
            SelectedCustomer = customer;
            LoadDetail();
        }

        private void LoadDetail()
        {
            if (SelectedCustomer != null)
            {
                this.Id = SelectedCustomer.Idcustomer.ToString();
                this.DisplayName = SelectedCustomer.Displayname ?? string.Empty;
                this.Address = SelectedCustomer.Address ?? string.Empty;
                this.Phone = SelectedCustomer.Phone ?? string.Empty;
                this.Email = SelectedCustomer.Email ?? string.Empty;
                this.MoreInfo = SelectedCustomer.Moreinfo ?? string.Empty;
                this.ContractDate = SelectedCustomer.Contractdate;
            }
        }

        public EditCustomerViewModel()
        {
            //LoadDetail();
        }
    }
}