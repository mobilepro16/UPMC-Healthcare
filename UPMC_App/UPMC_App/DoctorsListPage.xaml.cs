using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPMC_App.MyControls;
using UPMC_App.ServerDataModels;
using UPMC_App.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UPMC_App
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DoctorsListPage : ContentPage
	{
        DoctorSearchViewModel ViewModel;

        public DoctorsListPage ()
		{
			InitializeComponent ();
		}

        public DoctorsListPage(DoctorSearchViewModel vm)
        {
            InitializeComponent();
            ViewModel = vm;
            BindingContext = ViewModel;
        }

        protected override bool OnBackButtonPressed()
        {
            if (gridPopup.IsVisible)
            {
                CloseDetailsBlock();
                return true;
            }
            else
                return base.OnBackButtonPressed();
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ViewModel.SelectedProviderItem = e.Item as ProviderSearchItem;

            //Open hours
            stackOpenHours.Children.Clear();
            if (ViewModel.SelectedProviderItem.OperatingHours != null)
            {
                foreach (var item in ViewModel.SelectedProviderItem.OperatingHours.ViewHours)
                {
                    Label lbl = new Label() { Text = $"{item.DisplayDayText} {item.DisplayDayTimings}" };
                    if (item.Day == (int)DateTime.Now.DayOfWeek)
                        lbl.FontAttributes = FontAttributes.Bold;
                    stackOpenHours.Children.Add(lbl);
                }
            }

            //Accepted plans
            if (!ViewModel.IsFilterByCoverage)
            {
                stackPlansAccepted.Children.Clear();
                foreach(var item in ViewModel.SelectedProviderItem.PlansAccepted)
                {
                    stackPlansAccepted.Children.Add(new HtmlFormattedLabel() { Text = item, Margin = new Thickness(5, 0, 0, 0) });
                }
            }

            btnShowPlan.IsVisible = !ViewModel.SelectedProviderItem.IsPharmacyItem && !ViewModel.IsFilterByCoverage;

            gridPopup.IsVisible = true;
        }

        private void Phone_Tapped(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(ViewModel.SelectedProviderItem.Phone))
            {
                try
                {
                    PhoneDialer.Open(ViewModel.SelectedProviderItem.Phone);
                }
                catch { DisplayAlert("UPMC", "Phone Dialer is not supported on this device!", "Cancel"); }
            }
        }

        private void OpenHours_Tapped(object sender, EventArgs e)
        {
            stackOpenHours.IsVisible = !stackOpenHours.IsVisible;
            stackCurrentOpenStatus.IsVisible = !stackCurrentOpenStatus.IsVisible;
        }

        private void BtnCloseDetails_Clicked(object sender, EventArgs e)
        {
            CloseDetailsBlock();
        }

        private void CloseDetailsBlock()
        {
            gridPopup.IsVisible = false;
            stackOpenHours.IsVisible = false;
            stackCurrentOpenStatus.IsVisible = true;
            stackPlansAccepted.IsVisible = false;
        }

        private async void SelectedProviderMap_Tapped(object sender, EventArgs e)
        {
            //if ((ViewModel.SelectedProviderItem.Lat != 0) && (ViewModel.SelectedProviderItem.Lng != 0))
            //{
            //    //placemark = new Placemark
            //    //{
            //    var location = new Location(ViewModel.SelectedProviderItem.Lng, ViewModel.SelectedProviderItem.Lat);
            //    //};
            //    await Maps.OpenAsync(location);
            //}
            //else
            //{
                var placemark = new Placemark
                {
                    CountryName = "United States",
                    AdminArea = ViewModel.SelectedProviderItem.State,
                    Thoroughfare = ViewModel.SelectedProviderItem.GetAddressWithoutCity(),
                    Locality = ViewModel.SelectedProviderItem.City,
                    PostalCode = ViewModel.SelectedProviderItem.Zip
                };
                await Maps.OpenAsync(placemark);
            //}
            
            //var options = new MapsLaunchOptions { Name = ViewModel.SelectedProviderItem.FullName };

            
        }

        private void BtnShowPlanDetails_Clicked(object sender, EventArgs e)
        {
            if(stackPlansAccepted.IsVisible)
            {
                stackPlansAccepted.IsVisible = false;
                btnShowPlan.Text = "SHOW PLANS";
            }
            else
            {
                stackPlansAccepted.IsVisible = true;
                btnShowPlan.Text = "HIDE PLANS";
            }
        }
    }
}