using CommunityToolkit.Mvvm.ComponentModel;
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

        public NavigationStore(HomViewModel homViewModel)
        {
            _currentViewModel = homViewModel;
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

        public event Action CurrentViewModelChanged;

        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
    }
}