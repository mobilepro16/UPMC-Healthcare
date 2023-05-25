
using Plugin.Fingerprint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPMC_App.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace UPMC_App
{
	public partial class MainPage : Xamarin.Forms.TabbedPage
    {
        public MainPage()
		{
			InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            this.On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
            this.On<Android>().SetIsSwipePagingEnabled(false);
            this.On<Android>().SetBarSelectedItemColor((Color)App.Current.Resources["MainAccentColor"]);
            this.On<Android>().SetBarItemColor(Color.Gray);

            Color accent = (Color)App.Current.Resources["MainAccentColor"];
            Children.Add(new NavigationPage(new HomePage()) { Title = "Home", Icon = "baseline_home_black_48.png", BarBackgroundColor=accent, BarTextColor=Color.White });
            Children.Add(new NavigationPage(new MenuPage()) { Title = "Menu", Icon = "baseline_menu_black_48.png", BarBackgroundColor = accent, BarTextColor = Color.White });
            Children.Add(new NavigationPage(new ContactPage()) { Title = "Contact", Icon = "baseline_forum_black_48.png", BarBackgroundColor = accent, BarTextColor = Color.White });

            ViewModelContainer.HomeVM.LoggedOut += HomeVM_LoggedOut;
        }

        private void HomeVM_LoggedOut(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PasswordPage(true));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            while (Navigation.NavigationStack.Count > 1)
            {
                Navigation.RemovePage(Navigation.NavigationStack[0]);
            }
        }

        private async void Children_CurrentPageChanged(object sender, EventArgs e)
        {
            if (Children.Count != 3)
                return;

            var navP = Children[1] as NavigationPage;
            var menuP = navP.RootPage as MenuPage;

            int ndx = Children.IndexOf(CurrentPage);
            switch (ndx)
            {
                case 0:
                    if(Children[1].Navigation.NavigationStack.Count>1)
                    {
                        await Children[1].Navigation.PopToRootAsync();
                    }
                    //Если ContactPage будет навигационной
                    //if (Children[2].Navigation.NavigationStack.Count > 1)
                    //{
                    //    await Children[2].Navigation.PopToRootAsync();
                    //}
                    if (menuP != null)
                    {
                        menuP.GoToMainMenuStateImmediately();
                    }
                    break;
                case 1:
                    if (Children[0].Navigation.NavigationStack.Count > 1)
                    {
                        await Children[0].Navigation.PopToRootAsync();
                    }
                    //Если ContactPage будет навигационной
                    //if (Children[2].Navigation.NavigationStack.Count > 1)
                    //{
                    //    await Children[2].Navigation.PopToRootAsync();
                    //}
                    System.Diagnostics.Debug.WriteLine("Menu page activate");
                    
                    if(menuP!=null)
                    {
                        menuP.GoToSubmenuStateImmediately();
                    }
                    break;
                case 2:
                    if (Children[0].Navigation.NavigationStack.Count > 1)
                    {
                        await Children[0].Navigation.PopToRootAsync();
                    }
                    if (Children[1].Navigation.NavigationStack.Count > 1)
                    {
                        await Children[1].Navigation.PopToRootAsync();
                    }
                    if (menuP != null)
                    {
                        menuP.GoToMainMenuStateImmediately();
                    }
                    break;
                default:
                    break;
            }


            while(CurrentPage.Navigation.NavigationStack.Count>1)
            {
                await CurrentPage.Navigation.PopToRootAsync();
            }
            //System.Diagnostics.Debug.WriteLine($"stack :{Children[0].Navigation.NavigationStack.Count}");
        }

        private void MainPage_SizeChanged(object sender, EventArgs e)
        {
            if (this.Width > 320)
                ViewModelContainer.HomeVM.ChapterTitleFontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            else
                ViewModelContainer.HomeVM.ChapterTitleFontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
        }
    }
}
