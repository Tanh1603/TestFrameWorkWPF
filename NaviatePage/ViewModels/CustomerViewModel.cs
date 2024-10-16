using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using NaviatePage.Models;
using NaviatePage.Models.Data;
using NaviatePage.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace NaviatePage.ViewModels
{
    public partial class CustomerViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;

        [ObservableProperty]
        private ObservableCollection<Customer> _customerList;

        [ObservableProperty]
        private Customer _selectedCustomer;

        public CustomerViewModel(IServiceProvider provider)
        {
            _serviceProvider = provider;
            LoadCustomers();
        }

        private async void LoadCustomers()
        {
            var customers = await _serviceProvider.GetRequiredService<IDataService<Customer>>().GetAll();
            this.CustomerList = new ObservableCollection<Customer>(customers);
        }

        [RelayCommand]
        public async Task AddCustomer()
        {
            await _serviceProvider.GetRequiredService<IDataService<Customer>>().Create(
                    new Customer()
                    {
                        Idcustomer = 3,
                        Displayname = "Name",
                        Address = "Hcm",
                        Phone = "12345",
                        Email = "@gmail.com",
                        Contractdate = DateTime.Now,
                        Moreinfo = null
                    }
                );

            LoadCustomers();
        }

        [RelayCommand]
        public async Task UpdateCustomer()
        {
            var customer = await _serviceProvider.GetRequiredService<IDataService<Customer>>().Update(2, new Customer()
            {
                Idcustomer = 2,
                Displayname = "Tanh23532212",
                Address = "Hcm",
                Phone = "12345",
                Email = "@gmail.com",
                Contractdate = DateTime.Now,
                Moreinfo = null
            });
            LoadCustomers();
        }

        [RelayCommand]
        public async Task DeleteCustomer()
        {
            await _serviceProvider.GetRequiredService<IDataService<Customer>>().Delete(2);
            LoadCustomers();
        }
    }
}