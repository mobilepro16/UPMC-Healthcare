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
	public partial class CardsListPage : ContentPage
	{
        public CardsListPage()
		{
			InitializeComponent();
            BindingContext = ViewModelContainer.HomeVM;
		}

        private void cardsList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            int ndx = ViewModelContainer.HomeVM.IdCards.IndexOf(e.Item as CardPresenter);

            Navigation.PopAsync();

            var navStack = Navigation.NavigationStack;
            var cardPage = navStack[navStack.Count - 2] as IdCardsPage;
            if(cardPage!=null)
            {
                cardPage.SetSelectedCard(ndx);
            }
        }
    }
}