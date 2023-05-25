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
	public partial class ClaimSummaryPage : ContentPage
	{
        ClaimsViewModel ViewModel = ViewModelContainer.ClaimsVM;
		public ClaimSummaryPage()
		{
			InitializeComponent();
            BindingContext = ViewModel;
		}
	}
}