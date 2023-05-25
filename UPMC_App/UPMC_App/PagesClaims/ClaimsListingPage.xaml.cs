using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPMC_App.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UPMC_App
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ClaimsListingPage : ContentPage
	{
        ClaimsViewModel ViewModel = ViewModelContainer.ClaimsVM;
        public ClaimsListingPage()
		{
			InitializeComponent();
            BindingContext = ViewModel;
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var claim = e.Item as ClaimPresenter;
            if (claim != null)
            {
                ViewModel.CurrentClaim = claim;
                Navigation.PushAsync(new ClaimSummaryPage());
            }
            
        }
    }
}