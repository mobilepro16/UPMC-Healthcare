using Plugin.Fingerprint;
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
	public partial class PinPage : ContentPage
	{
		public PinPage ()
		{
			InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            this.BindingContext = ViewModel;

            TapGestureRecognizer tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += TapGesture_Tapped;
            lblUsePassword.GestureRecognizers.Add(tapGesture);

            TapGestureRecognizer imgTapGesture = new TapGestureRecognizer();
            imgTapGesture.Tapped += ImgTapGesture_Tapped;
            imgFinger.GestureRecognizers.Add(imgTapGesture);

            ViewModel.PinAuthError += ViewModel_PinAuthError;
            ViewModel.PinAuthSuccess += ViewModel_PinAuthSuccess;
        }

        private async void ViewModel_PinAuthSuccess(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }

        private async void ViewModel_PinAuthError(object sender, string errorMsg)
        {
            await DisplayAlert("Authorization", errorMsg, "ОK");
        }

        AuthViewModel ViewModel = new AuthViewModel();

        private void ImgTapGesture_Tapped(object sender, EventArgs e)
        {
            //auth with fingerprint
            AuthByFingerprint();
        }

        private async void TapGesture_Tapped(object sender, EventArgs e)
        {
            //go to password screen
            //System.Diagnostics.Debug.WriteLine("Go to password page");

            await Navigation.PushAsync(new PasswordPage());
        }

        private void BtnNum_Clicked(object sender, EventArgs e)
        {
            string num = (sender as Button).Text;
            System.Diagnostics.Debug.WriteLine($"Button {num}");
            ViewModel.Pin += num;
        }

        private async void AuthByFingerprint()
        {
            ViewModel.PinResultText = String.Empty;
            ViewModel.Pin = String.Empty;
            var cancellationTocken = new System.Threading.CancellationToken();
            var fingerRes = await CrossFingerprint.Current.AuthenticateAsync("Touch the sensor to enter", cancellationTocken);

            System.Diagnostics.Debug.WriteLine($"is auth={fingerRes.Authenticated}");
            if(fingerRes.Authenticated)
            {
                string error = await ViewModel.CheckPassword(true);
                if (String.IsNullOrEmpty(error))
                {
                    await Navigation.PushAsync(new MainPage());
                }
                else
                {
                    await DisplayAlert("Authorization", error, "ОK");
                }
            }
            else
            {
                ViewModel.PinResultText = "Fingerprint is incorrect";
                ViewModel.PinResultColor = Color.Red;
            }
        }

        //protected override void OnAppearing()
        //{

        //    base.OnAppearing();
        //    if (ViewModels.AuthViewModel.IsClose)
        //        Navigation.PopModalAsync(false);
        //}

        #region Back Button events
        //protected override bool OnBackButtonPressed()
        //{
        //    DependencyService.Get<IAndroidMethods>().CloseApp();
        //    return base.OnBackButtonPressed();
        //}
        #endregion

        private void Page_SizeChanged(object sender, EventArgs e)
        {
            if (this.Width <= 320)
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