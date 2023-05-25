using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPMC_App.Interfaces;
using UPMC_App.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UPMC_App
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PasswordPage : ContentPage
	{
        AuthViewModel ViewModel = new AuthViewModel();

        bool _isClear = false;
        public PasswordPage(bool isClear = false)
		{
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            this.BindingContext = ViewModel;

            _isClear = isClear;
            //if (isClear)
            //{
            //    //while (Navigation.NavigationStack.Count > 1)
            //    //{
            //    //    Navigation.RemovePage(Navigation.NavigationStack[0]);
            //    //}
            //    var existingPages = Navigation.NavigationStack.ToList();
            //    foreach (var page in existingPages)
            //    {
            //        Navigation.RemovePage(page);
            //    }
            //}
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (_isClear)
            {
                while (Navigation.NavigationStack.Count > 1)
                {
                    Navigation.RemovePage(Navigation.NavigationStack[0]);
                }
                _isClear = false;
            }
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            System.Diagnostics.Debug.WriteLine("PasswordPage Disappearing");
        }

        private async void BtnLogin_Clicked(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(ViewModel.Username)||String.IsNullOrEmpty(ViewModel.Password))
            {
                await DisplayAlert(ViewModelContainer.AppName, "Enter your username and password!", "OK");
                return;
            }

            string error = await ViewModel.CheckPassword();
            if (String.IsNullOrEmpty(error))
            {
                await Navigation.PushAsync(new MainPage());
            }
            else
            {
                await DisplayAlert("Authorization", error, "ОK");
            }
        }


        #region Back Button events
        protected override bool OnBackButtonPressed()
        {
            if (Navigation.ModalStack.Count == 1)
            {
                DependencyService.Get<IAndroidMethods>().CloseApp();
            }

            return base.OnBackButtonPressed();
        }
        #endregion

        private void Page_SizeChanged(object sender, EventArgs e)
        {
            if(this.Width<=320)
                lblTermsPolicies.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
            else
                lblTermsPolicies.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
        }

        private void LblTerms_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TermsPoliciesListPage());
        }
    }
}