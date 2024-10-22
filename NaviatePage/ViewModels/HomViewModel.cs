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
        private string _message;

        public HomViewModel()
        {
            Message = "Hello word";
        }
    }
}