using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UPMC_App.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UPMC_App
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PharmacyCenterPage : ContentPage
	{
        PrescriptionsViewModel ViewModel = ViewModelContainer.PrescriptionsVM;
		public PharmacyCenterPage()
		{
			InitializeComponent();
            //Fill_LOCAL();
            BindingContext = ViewModel;

            TapGestureRecognizer tapSeeCurrentPrescr = new TapGestureRecognizer();
            tapSeeCurrentPrescr.Tapped += (s, e) => { ViewModel.IsDetailsForHistory = false; Navigation.PushAsync(new PharmacyHistoryPage()); };
            lblSeeCurrentPrescriptions.GestureRecognizers.Add(tapSeeCurrentPrescr);

            TapGestureRecognizer tapSeeHistory = new TapGestureRecognizer();
            tapSeeHistory.Tapped += (s, e) => { ViewModel.IsDetailsForHistory = true; Navigation.PushAsync(new PharmacyHistoryPage()); };
            lblSeePrescriptionHistory.GestureRecognizers.Add(tapSeeHistory);

            //TapGestureRecognizer tapPharmacyPhone = new TapGestureRecognizer();
            //tapPharmacyPhone.Tapped += TapPharmacyPhone_Tapped;
            //imgPhone.GestureRecognizers.Add(tapPharmacyPhone);
        }

        private void TapPharmacyPhone_Tapped(object sender, EventArgs e)
        {
            var view = sender as View;
            if(view!=null)
            {
                var pharmItem = view.BindingContext as PharmacyItem;
                if(pharmItem!=null)
                {
                    if (!String.IsNullOrEmpty(pharmItem.Phone))
                    {
                        try
                        {
                            PhoneDialer.Open(pharmItem.Phone);
                        }
                        catch {DisplayAlert("UPMC", "Phone Dialer is not supported on this device!", "Cancel"); }
                    }
                }
            }
        }

        private async void TapPharmacyMap_Tapped(object sender, EventArgs e)
        {
            var view = sender as View;
            if (view != null)
            {
                var pharmItem = view.BindingContext as PharmacyItem;
                if (pharmItem != null)
                {
                    var placemark = new Placemark
                    {
                        CountryName = "United States",
                        AdminArea = pharmItem.State,
                        Thoroughfare = pharmItem.AddressLine,
                        Locality = pharmItem.City,
                        PostalCode = pharmItem.Zip
                    };
                    var options = new MapsLaunchOptions { Name = pharmItem.Name };
                    
                    await Maps.OpenAsync(placemark, options);
                }
            }
        }

        private void Fill_LOCAL()
        {
            ViewModel.CurrentPrescriptions.Clear();
            ViewModel.CurrentPrescriptions.Add(new PrescriptionItem()
            {
                DrugName = "AMLODIPINE BESYLATE 5 MG TAB",
                DosageString = "5 MG",
                PrescriptionNumberString = "Rx #000212101",
                SupplyString = "90 count",
                Supply = 90,
                DateString = "6/9/2018 date filled",
                DateRefill=new DateTime(2018,3,5)
            });
            ViewModel.CurrentPrescriptions.Add(new PrescriptionItem()
            {
                DrugName = "SPIRONOLACTONE 25 MG TABLET Tablet TABLET TABLET TABLET",
                DosageString = "25 MG",
                PrescriptionNumberString = "Rx #000212101",
                SupplyString = "90 count",
                Supply = 60,
                DateString = "6/9/2018 date filled",
                DateRefill = new DateTime(2018, 4, 5)
            });
            ViewModel.CurrentPrescriptions.Add(new PrescriptionItem()
            {
                DrugName = "IRBESARTAN 300 MG TABLET",
                DosageString = "300 MG",
                PrescriptionNumberString = "Rx #000212101",
                SupplyString = "90 count",
                Supply = 25,
                DateString = "6/9/2018 date filled",
                DateRefill = new DateTime(2018, 6, 5)
            });

            ViewModel.PrescriptionsHistory.Clear();
            ViewModel.PrescriptionsHistory.Add(new PrescriptionItem()
            {
                DrugName = "AMLODIPINE BESYLATE 5 MG TAB",
                DosageString = "5 MG",
                PrescriptionNumberString = "Rx #000212101",
                SupplyString = "90 count",
                Supply = 90,
                DateString = "6/9/2018 date filled",
                DateRefill = new DateTime(2018, 3, 5)
            });
            ViewModel.PrescriptionsHistory.Add(new PrescriptionItem()
            {
                DrugName = "SPIRONOLACTONE 25 MG TABLET Tablet TABLET TABLET TABLET",
                DosageString = "25 MG",
                PrescriptionNumberString = "Rx #000212101",
                SupplyString = "90 count",
                Supply = 75,
                DateString = "6/9/2018 date filled",
                DateRefill = new DateTime(2018, 6, 5)
            });
            ViewModel.PrescriptionsHistory.Add(new PrescriptionItem()
            {
                DrugName = "IRBESARTAN 300 MG TABLET",
                DosageString = "300 MG",
                PrescriptionNumberString = "Rx #000212101",
                SupplyString = "90 count",
                Supply = 180,
                DateString = "6/9/2018 date filled",
                DateRefill = new DateTime(2018, 7, 21)
            });

            ViewModel.Pharmacies.Clear();
            ViewModel.Pharmacies.Add(new PharmacyItem() { AddressLine = "4600 N HANLEY RD", City = "SAINT LOUIS", Zip = "63134", State = "MO", Name = "EXPRESS SCRIPTS", Phone= "8003325455", IsFavorite = false});
            ViewModel.Pharmacies.Add(new PharmacyItem() { AddressLine = "lsdkfjlskdfjfsdf", City = "dfdsfs", Zip = "45665", State = "MO", Name = "TEST NAME", Phone= "4129206190", IsFavorite = true });
        }

        private void PharmacySearch_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PharmacySearchPage());
        }
    }
}