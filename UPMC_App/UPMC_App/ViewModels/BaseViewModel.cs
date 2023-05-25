using System;
using System.Collections.Generic;
using System.Text;

namespace UPMC_App.ViewModels
{
    public class BaseViewModel:BindableBase
    {
        private bool _isProgressActive = false;

        public bool IsProgressActive { get => _isProgressActive; set =>SetProperty(ref _isProgressActive, value); }
    }
}
