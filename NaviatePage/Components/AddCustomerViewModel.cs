using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using NaviatePage.Models.Data;
using NaviatePage.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaviatePage.Components
{
    public partial class AddCustomerViewModel : ObservableObject, IDisposable
    {
        private IServiceProvider _provider;

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

        private Customer _selectedCustomer;

        public event Action<Customer> SelectedCustomerChanged;

        public event Action CloseAddCustomerViewChanged;

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

            this.Id = null;
            this.DisplayName = null;
            this.Address = null;
            this.Phone = null;
            this.Email = null;
            this.MoreInfo = null;
            this.ContractDate = null;
            SelectedCustomerChanged?.Invoke(customer);
        }

        public void Dispose()
        {
            if (SelectedCustomerChanged != null)
            {
                foreach (var d in SelectedCustomerChanged.GetInvocationList())
                {
                    SelectedCustomerChanged -= (Action<Customer>)d;
                }
            }
        }

        [RelayCommand]
        private void Close()
        {
            CloseAddCustomerViewChanged?.Invoke();
        }

        public AddCustomerViewModel(IServiceProvider provider)
        {
            _provider = provider;
        }
    }
}