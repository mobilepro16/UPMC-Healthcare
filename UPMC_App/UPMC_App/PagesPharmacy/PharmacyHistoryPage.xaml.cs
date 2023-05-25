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
	public partial class PharmacyHistoryPage : ContentPage
	{
        PrescriptionsViewModel ViewModel = ViewModelContainer.PrescriptionsVM;
        public PharmacyHistoryPage ()
		{
            //NavigationPage.SetBackButtonTitle(this, "Back");
			InitializeComponent();
            BindingContext = ViewModel;

            TapGestureRecognizer tapSortDate = new TapGestureRecognizer();
            tapSortDate.Tapped += (s, e) => { ViewModel.IsSortByDate = !ViewModel.IsSortByDate; };
            gridSortDate.GestureRecognizers.Add(tapSortDate);
            gridSortAlphabet.GestureRecognizers.Add(tapSortDate);


            TapGestureRecognizer tapPopupGrid = new TapGestureRecognizer();
            tapPopupGrid.Tapped += (s, e) => { gridPopupTable.IsVisible = false; };
            gridPopupTable.GestureRecognizers.Add(tapPopupGrid);

            TapGestureRecognizer tapPopupSubGrid = new TapGestureRecognizer();
            tapPopupSubGrid.Tapped += (s, e) => { System.Diagnostics.Debug.WriteLine("SUB gridTapped"); };
            gridPopupSubGrid.GestureRecognizers.Add(tapPopupSubGrid);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine($"{this.Width} date {lblDate.Width}");
        }

        private async void lstDetails_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selected = e.Item as PrescriptionItem;
            if(selected!=null)
            {
                ViewModel.CurrentDrugName = selected.DrugName;
                await Task.Delay(50);
                gridPopupTable.IsVisible = true;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            if (gridPopupTable.IsVisible)
            {
                gridPopupTable.IsVisible = false;
                return true;
            }
            else
                return base.OnBackButtonPressed();
        }
    }
}