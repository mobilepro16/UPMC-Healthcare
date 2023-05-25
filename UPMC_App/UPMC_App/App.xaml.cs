using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace UPMC_App
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();


            //MainPage = new NavigationPage(new MainPage());
            //MainPage = new NavigationPage(new IdCardsPage());

            if (String.IsNullOrEmpty(ViewModels.AuthViewModel.StoredUsername))
            {
                MainPage = new NavigationPage(new PasswordPage()) { BarBackgroundColor = (Color)App.Current.Resources["MainAccentColor"], BarTextColor = Color.White };
            }
            else
            {
                MainPage = new NavigationPage(new PinPage()) { BarBackgroundColor = (Color)App.Current.Resources["MainAccentColor"], BarTextColor = Color.White };
            }
        }

        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
