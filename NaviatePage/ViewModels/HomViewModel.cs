using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaviatePage.ViewModels
{
    public partial class HomViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _isSample4DialogOpen;

        [ObservableProperty]
        private string _sample4Content;

        [RelayCommand]
        private void OpenSample4Dialog()
        {
            IsSample4DialogOpen = true;
        }

        private void CloseSample4Dialog()
        {
        }
    }
}