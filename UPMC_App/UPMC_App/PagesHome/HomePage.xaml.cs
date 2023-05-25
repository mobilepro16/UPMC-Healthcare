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
	public partial class HomePage : ContentPage
	{
        HomeViewModel ViewModel = ViewModelContainer.HomeVM;
		public HomePage ()
		{
			InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = ViewModel;

            //LOCAL_FILL();

            #region TapGestures
            TapGestureRecognizer tapShowId = new TapGestureRecognizer();
            tapShowId.Tapped += (s, e) => { ViewModel.IsMemberIdVisible = !ViewModel.IsMemberIdVisible; };
            stackShowMemberId.GestureRecognizers.Add(tapShowId);

            TapGestureRecognizer tapIdCards = new TapGestureRecognizer();
            //tapOpenPageLabels.Tapped += TapOpenPageLabels_Tapped;
            tapIdCards.Tapped += (s, e) => { Navigation.PushAsync(new IdCardsPage()); };
            lblIdCards.GestureRecognizers.Add(tapIdCards);

            TapGestureRecognizer tapYourSettings= new TapGestureRecognizer();
            tapYourSettings.Tapped += (s, e) => { ViewModel.OpenPageFromHomePage("yourSettings"); };
            lblYourSettings.GestureRecognizers.Add(tapYourSettings);


            TapGestureRecognizer tapPharmacy = new TapGestureRecognizer();
            tapPharmacy.Tapped += (s, e) => { ViewModelContainer.PrescriptionsVM.UpdatePrescriptionsList(ViewModel.MemberId); Navigation.PushAsync(new PharmacyCenterPage()); };
            stackPharmacy.GestureRecognizers.Add(tapPharmacy);
            imgPharmacy.GestureRecognizers.Add(tapPharmacy);
            #endregion
        }

        private void LOCAL_FILL()
        {
            ViewModel.FirstClaim = new ClaimPresenter(new ServerDataModels.ClaimInfo() { dateOfService = "20180721", claimStatus = "Denied", coveredMember = "Member", provider = "TEST PROVIDER" });
            ViewModel.SecondClaim = new ClaimPresenter(new ServerDataModels.ClaimInfo() { dateOfService = "20180721", claimStatus = "Process", coveredMember = "Member", provider = "TEST PROVIDER" });
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"{lblIdCards.Width}x{lblIdCards.Height}");
        }

        private void RecentClaim_Tapped(object sender, EventArgs e)
        {
            var grid = sender as View;
            if (grid != null)
            {
                var claim = grid.BindingContext as ClaimPresenter;
                if (claim != null)
                {
                    ViewModelContainer.ClaimsVM.CurrentClaim = claim;
                    Navigation.PushAsync(new ClaimSummaryPage());
                }
            }
        }

        private void ViewAllClaims_Tapped(object sender, EventArgs e)
        {
            ViewModelContainer.ClaimsVM.UpdateCoveredMembersList();
            Navigation.PushAsync(new MembersWithClaimsPage());
        }

        private void DoctorsSearch_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new DoctorsSearchPage());
        }
        private void HospitalsSearch_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new HospitalSearch());
        }

        private void FindAllCare_Tapped(object sender, EventArgs e)
        {
            ViewModelContainer.MenuVM.OpenSubmenuByName("Find Care");
            var masterPage = (this.Parent as NavigationPage).Parent as TabbedPage;// "Find Care"
            
            masterPage.CurrentPage = masterPage.Children[1];
        }

        private void FindAllMedicalFile_Tapped(object sender, EventArgs e)
        {
            ViewModelContainer.MenuVM.OpenSubmenuByName("Your Medical File");
            var masterPage = (this.Parent as NavigationPage).Parent as TabbedPage;// "Find Care"

            masterPage.CurrentPage = masterPage.Children[1];
        }

        private void YourProviders_Tapped(object sender, EventArgs e)
        {
            ViewModelContainer.YourProvidersVM.UpdateProvidersList(ViewModel.MemberId);
            Navigation.PushAsync(new YourProvidersPage());
        }
    }
}