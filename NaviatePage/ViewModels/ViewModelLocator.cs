using Microsoft.Extensions.DependencyInjection;
using NaviatePage.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaviatePage.ViewModels
{
    public class ViewModelLocator
    {
        private readonly IServiceProvider _serviceProvider;

        public ViewModelLocator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public CustomerViewModel CustomerVM => _serviceProvider.GetService<CustomerViewModel>();
        public HomViewModel HomeVMl => _serviceProvider.GetRequiredService<HomViewModel>();
        public OrderViewModel CustomerViewModel => _serviceProvider.GetService<OrderViewModel>();
        public ProductViewModel Cu => _serviceProvider.GetService<ProductViewModel>();
    }
}