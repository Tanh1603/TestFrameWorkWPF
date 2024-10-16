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
    public partial class NavigationStore : ObservableObject
    {
        private ObservableObject _currentViewModel;
        private HomViewModel homViewModel;

        public NavigationStore(HomViewModel homViewModel)
        {
            _currentViewModel = homViewModel;
        }

        public ObservableObject CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                SetProperty(ref _currentViewModel, value);
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