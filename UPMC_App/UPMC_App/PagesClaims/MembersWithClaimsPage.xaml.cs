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
	public partial class MembersWithClaimsPage : ContentPage
	{
        ClaimsViewModel ViewModel = ViewModelContainer.ClaimsVM;
        public MembersWithClaimsPage()
		{
			InitializeComponent();
            BindingContext = ViewModel;
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var member = e.Item as IdCardInfo;
            if (member != null)
            {
                ViewModel.UpdateClaimsListForSelectMember(member._MemberId);
                Navigation.PushAsync(new ClaimsListingPage());
            }
        }
    }
}