using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPMC_App.Animations;
using UPMC_App.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UPMC_App
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuPage : ContentPage
	{
        enum States
        {
            Main,
            Submenu
        }

        MenuViewModel ViewModel = ViewModelContainer.MenuVM;

        readonly Storyboard _storyboard = new Storyboard();
        public MenuPage()
		{
			InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            //ViewModel.LOCAL_FillMenuFromAuthResponse(null);
            BindingContext = ViewModel;

            TapGestureRecognizer tapCloseSubmenu = new TapGestureRecognizer();
            tapCloseSubmenu.Tapped += (s, e) =>
            {
                if (!ViewModel.IsMainMenuActive)
                {
                    _storyboard.Go(States.Main);
                    ViewModel.IsMainMenuActive = true;
                }
            };
            lblMainMenuTitle.GestureRecognizers.Add(tapCloseSubmenu);
            lblSubmenuTitle.GestureRecognizers.Add(tapCloseSubmenu);


            //TapGestureRecognizer tapCloseSubmenu = new TapGestureRecognizer();
            //tapCloseSubmenu.Tapped += (s, e) =>
            //{
            //    if (!ViewModel.IsMainMenuActive)
            //    {
            //        _storyboard.Go(States.Main);
            //        ViewModel.IsMainMenuActive = true;
            //    }
            //};
            //lblSubmenuTitle.GestureRecognizers.Add(tapCloseSubmenu);


            _storyboard.Add(States.Main, new[] {
                                                        new ViewTransition(mainMenuList, AnimationType.TranslationY, 0),
                                                        new ViewTransition(mainMenuList, AnimationType.Opacity, 1, 300), // Active and visible
				                                       //new ViewTransition(EnterAddressButton, AnimationType.Scale, 0.9, 0),
                                                       //new ViewTransition(EnterAddressButton, AnimationType.Scale, 1, 300, Easing.BounceOut, 200),
                                                       //new ViewTransition(FindAddressView, AnimationType.Opacity, 0),
                                                       //new ViewTransition(ShowRouteView, AnimationType.TranslationY, 200),
                                                       new ViewTransition(submenuBlock, AnimationType.TranslationY, 400),
                                                       new ViewTransition(submenuBlock, AnimationType.Opacity, 0)
                                                   });
            _storyboard.Add(States.Submenu, new[] {
                                                       //new ViewTransition(mainMenuList, AnimationType.Opacity, 0, 100), // Active and visible
				                                       //new ViewTransition(EnterAddressButton, AnimationType.Scale, 0.9, 0),
                                                       //new ViewTransition(EnterAddressButton, AnimationType.Scale, 1, 300, Easing.BounceOut, 200),
                                                       //new ViewTransition(FindAddressView, AnimationType.Opacity, 0),
                                                       //new ViewTransition(ShowRouteView, AnimationType.TranslationY, 200),
                                                       new ViewTransition(mainMenuList, AnimationType.TranslationY, 400, 300),
                                                       new ViewTransition(mainMenuList, AnimationType.Opacity, 0,300),
                                                       new ViewTransition(submenuBlock, AnimationType.TranslationY, 0, 300),
                                                       new ViewTransition(submenuBlock, AnimationType.Opacity, 1,0)
                                                   });
            
            _storyboard.Go(States.Main, false);

        }

        public void GoToSubmenuStateImmediately()
        {
            if (ViewModel.IsSubmenuOpen)
            {
                _storyboard.Go(States.Submenu, false);
                ViewModel.IsSubmenuOpen = false;
                ViewModel.IsMainMenuActive = false;
            }
        }

        public void GoToMainMenuStateImmediately()
        {
            _storyboard.Go(States.Main, false);
            ViewModel.IsMainMenuActive = true;
        }

        private void TestBlockClose_Clicked(object sender, EventArgs e)
        {
            _storyboard.Go(States.Main);
            ViewModel.IsMainMenuActive = true;
        }

        private async void mainMenuItem_Tapped(object sender, ItemTappedEventArgs e)
        {
            
            var item = (e.Item as MainMenuItem);
            if (item != null)
            {
                #region Local
                //if (item.IsMain)
                //{
                //    //Информация из SubSections по группам
                //    item.SubSections = new List<ServerDataModels.MenuSectionType>();
                //    var section = new ServerDataModels.MenuSectionType() { Title = "Group1" };
                //    section.SubMenuItems = new List<ServerDataModels.SubMenuItemType>();
                //    section.SubMenuItems.Add(new ServerDataModels.SubMenuItemType() { Name = "Test 1", Active = true });
                //    section.SubMenuItems.Add(new ServerDataModels.SubMenuItemType() { Name = "Test 2", Active = true });
                //    section.SubMenuItems.Add(new ServerDataModels.SubMenuItemType() { Name = "Test 3", Active = true });
                //    section.SubMenuItems.Add(new ServerDataModels.SubMenuItemType() { Name = "Test 4", Active = true });
                //    item.SubSections.Add(section);

                //    section = new ServerDataModels.MenuSectionType() { Title = "Group 2" };
                //    section.SubMenuItems = new List<ServerDataModels.SubMenuItemType>();
                //    section.SubMenuItems.Add(new ServerDataModels.SubMenuItemType() { Name = "Test 1", Active = true });
                //    section.SubMenuItems.Add(new ServerDataModels.SubMenuItemType() { Name = "Test 2", Active = true });
                //    section.SubMenuItems.Add(new ServerDataModels.SubMenuItemType() { Name = "Test 3", Active = true });
                //    section.SubMenuItems.Add(new ServerDataModels.SubMenuItemType() { Name = "Test 4", Active = true });
                //    item.SubSections.Add(section);

                //    //Информация из RelatedSections в отдельную группу
                //    item.RelatedSections = new List<ServerDataModels.RelatedSectionType>();
                //    var relSection = new ServerDataModels.RelatedSectionType { Title = "Related" };
                //    relSection.RelatedItems = new List<ServerDataModels.RelatedItemType>();
                //    relSection.RelatedItems.Add(new ServerDataModels.RelatedItemType() { Name = "Related 1 kljd asjd dkjf skcsedf edfh dijhfekdn" });
                //    relSection.RelatedItems.Add(new ServerDataModels.RelatedItemType() { Name = "Related 2 kljd asjd dkjf skcsedf" });
                //    relSection.RelatedItems.Add(new ServerDataModels.RelatedItemType() { Name = "Related 3 kljd asjd dkjf skcsedf kljd asjd dkjf skcsedf kljd asjd dkjf skcsedf kljd asjd dkjf skcsedf kljd asjd dkjf skcsedf" });
                //    item.RelatedSections.Add(relSection);
                //}
                #endregion

                if (item.IsMain)
                {
                    ViewModel.SetSubmenuData(item);

                    await Task.Delay(30);//????
                    System.Diagnostics.Debug.WriteLine("Tapped " + item.Name);
                    _storyboard.Go(States.Submenu);
                    ViewModel.IsMainMenuActive = false;
                }
                else
                {
                    if(item.Name.StartsWith("Prescriptions"))
                    {
                        ViewModelContainer.PrescriptionsVM.UpdatePrescriptionsList(ViewModelContainer.HomeVM.MemberId);
                        await Navigation.PushAsync(new PharmacyCenterPage());
                    }
                    else if(item.Name == "Logout")
                    {
                        var answer = await DisplayAlert("UMPC Health Plan", "Do you want logout?", "Yes", "No");
                        if (answer)
                        {
                            ViewModelContainer.HomeVM.Logout();
                        }
                        //
                    }
                    else if (item.Name == "About")
                    {
                        await Navigation.PushAsync(new TermsPoliciesListPage());
                    }
                    else
                    {
                        await DisplayAlert("Menu", $"You select '{item.Name}'", "Cancel");
                    }   
                }
            }
        }

        private async void subMenuList_ItemTaPPED(object sender, ItemTappedEventArgs e)
        {
            var tappedItem = e.Item as SubmenuItem;
            if(tappedItem!=null)
            {
                switch (tappedItem.Name)
                {
                    case "Claims":
                        ViewModelContainer.ClaimsVM.UpdateCoveredMembersList();
                        await Navigation.PushAsync(new MembersWithClaimsPage());
                        break;
                    case "ID Cards":
                        await Navigation.PushAsync(new IdCardsPage());
                        break;
                    case "Doctors":
                        await Navigation.PushAsync(new DoctorsSearchPage());
                        break;
                    case "Pharmacies":
                        await Navigation.PushAsync(new PharmacySearchPage());
                        break;
                    case "Hospitals and Facilities":
                        await Navigation.PushAsync(new HospitalSearch());
                        break;
                    case "Urgent Care Centers":
                        await Navigation.PushAsync(new UCCSearchPage());
                        break;
                    default:
                        break;
                }
            }
            
        }
    }
}