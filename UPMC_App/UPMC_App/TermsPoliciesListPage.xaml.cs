using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UPMC_App
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TermsPoliciesListPage : ContentPage
	{
		public TermsPoliciesListPage ()
		{
			InitializeComponent ();
		}

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            switch (e.Item.ToString())
            {
                case "Privacy Policy":
                    break;
                case "Nondiscrimination Notice":
                    break;
                default:
                    break;
            }
        }
    }
}