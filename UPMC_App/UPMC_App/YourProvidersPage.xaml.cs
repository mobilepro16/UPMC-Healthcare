using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPMC_App.ServerDataModels;
using UPMC_App.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UPMC_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class YourProvidersPage : ContentPage
    {
        YourProvidersViewModel ViewModel = ViewModelContainer.YourProvidersVM;
        public YourProvidersPage()
        {
            InitializeComponent();
            BindingContext = ViewModel;
        }

        private void LastVisit_Tapped(object sender, EventArgs e)
        {
            if (!ViewModel.IsSortLastVisit)
            {
                ViewModel.IsSortLastVisit = true;
                ViewModel.SortProvidersList();
            }
        }

        private void Name_Tapped(object sender, EventArgs e)
        {
            if (!ViewModel.IsSortName)
            {
                ViewModel.IsSortName = true;
                ViewModel.SortProvidersList();
            }
        }

        private void Specialty_Tapped(object sender, EventArgs e)
        {
            if (!ViewModel.IsSortSpecialty)
            {
                ViewModel.IsSortSpecialty = true;
                ViewModel.SortProvidersList();
            }
        }

        private void VisitHistory_Tapped(object sender, EventArgs e)
        {
            var item = (sender as View).BindingContext as ProviderHistoryItem;
            if(item!=null)
            {
                ;
            }
        }

        private void BtnAddProvider_Click(object sender, EventArgs e)
        {

        }

        private async void ItemDelete_Tapped(object sender, EventArgs e)
        {
            var item = (sender as View).BindingContext as ProviderHistoryItem;
            if (item != null)
            {
                bool result = await DisplayAlert(ViewModelContainer.AppName, "Remove from My Health Care Team?", "Yes", "No");
                if (result)
                {
                    bool ret = await ViewModel.DeleteProvider(item.ProviderKey.ToString());
                }
            }
        }

        private void EditMode_Clicked(object sender, EventArgs e)
        {
            ViewModel.IsEditEnabled = !ViewModel.IsEditEnabled;
        }
    }
}