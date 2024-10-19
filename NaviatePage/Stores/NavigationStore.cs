using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using NaviatePage.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaviatePage.Stores
{
    public partial class NavigationStore
    {
        private ObservableObject _currentViewModel;
        private readonly IServiceProvider _serviceProvider;

        public NavigationStore(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ObservableObject CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnCurrentViewModelChanged();
            }
        }

        public IServiceProvider S { get; }

        public event Action CurrentViewModelChanged;

        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
    }
}