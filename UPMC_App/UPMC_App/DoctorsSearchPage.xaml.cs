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
	public partial class DoctorsSearchPage : ContentPage
	{
        DoctorSearchViewModel ViewModel = new DoctorSearchViewModel() { SearchPageTitle = "Doctors Search" };
		public DoctorsSearchPage ()
		{
			InitializeComponent();
            BindingContext = ViewModel;
            ViewModel.LoadDoctorsSpecialty();
		}

        private void ByName_Tapped(object sender, EventArgs e)
        {
            ViewModel.IsByName = true;
        }

        private void BySpeciality_Tapped(object sender, EventArgs e)
        {
            ViewModel.IsByName = false;
        }

        private void PrimaryCarePhisician_Toggled(object sender, ToggledEventArgs e)
        {
            ViewModel.IsPrimaryCarePhisician = e.Value;
        }

        private void Specialist_Toggled(object sender, ToggledEventArgs e)
        {
            ViewModel.IsPrimaryCarePhisician = !e.Value;
        }

        private async void BtnSearch_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine(this.lblLocation.Height.ToString());
            await ViewModel.SearchDoctors();
            if (ViewModel.SearchResults.Count > 0)
                await Navigation.PushAsync(new DoctorsListPage(ViewModel));
            else
                await DisplayAlert(ViewModelContainer.AppName, "No result were found for your search.", "OK");
        }

        private void LabelCareType_Tapped(object sender, EventArgs e)
        {
            ViewModel.IsPrimaryCarePhisician = !ViewModel.IsPrimaryCarePhisician;
        }

        private async void WhatDoesThisMean_Tapped(object sender, EventArgs e)
        {
            string title = "What Does This Mean";
            string msg = "Unchecking this box will ignore your coverage and show all providers in our directory. This will include providers that are not in-network for your plan.\n\r\n\r" +
                "Some plans offer benefits if you see providers not in your network. These benefits are subject to higher deductibles, copayments, and coinsurance.";

            await DisplayAlert(title, msg, "OK");
        }

        private void EntryZip_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                bool isValid = e.NewTextValue.ToCharArray().All(x => char.IsDigit(x)); //Make sure all characters are numbers

                ((Entry)sender).Text = isValid ? e.NewTextValue : e.NewTextValue.Remove(e.NewTextValue.Length - 1);
            }
        }

        private async void UseCurrentLocation_Tapped(object sender, EventArgs e)
        {
            string error = await ViewModel.TryGetZipByCurrentLocation();
            if(!string.IsNullOrEmpty(error))
            {
                await DisplayAlert(ViewModelContainer.AppName, error, "OK");
            }
        }
    }
}