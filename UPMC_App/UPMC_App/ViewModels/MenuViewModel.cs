using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using UPMC_App.ServerDataModels;

namespace UPMC_App.ViewModels
{
    #region Main menu
    public class MainMenuItem
    {
        public Assembly SvgAssembly => typeof(App).GetTypeInfo().Assembly;

        public string Name { get; set; }
        public string ItemImage { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public bool IsMain { get; set; } = true;
        public List<MenuSectionType> SubSections { get; set; }
        public List<RelatedSectionType> RelatedSections { get; set; }
    }

    public class MainMenuGroup : ObservableCollection<MainMenuItem>
    {
        public MenuGroupItemInfo Group { get; private set; }
        public MainMenuGroup(MenuGroupItemInfo group) : base()
        {
            Group = group;
        }
        public MainMenuGroup(MenuGroupItemInfo group, IEnumerable<MainMenuItem> source) : base(source)
        {
            Group = group;
        }
    }
    #endregion

    public class MenuGroupItemInfo
    {
        public string GroupName { get; set; }
        public MenuHeaderTypes HeaderType { get; set; } = MenuHeaderTypes.Regular;
        public bool IsShowDivider { get; set; } = false;
    }

    public class SubmenuItem
    {
        public string Name { get; set; }
        public string Tag { get; set; }
        public bool IsActive { get; set; }
        public bool IsRelated { get; set; } = false;
        public bool IsNotRelated => !IsRelated;
    }

    public class MenuGroup : ObservableCollection<SubmenuItem>
    {
        public MenuGroupItemInfo Group { get; private set; }
        public MenuGroup(MenuGroupItemInfo group) : base()
        {
            Group = group;
        }
        public MenuGroup(MenuGroupItemInfo group, IEnumerable<SubmenuItem> source) : base(source)
        {
            Group = group;
        }
    }

    public class MenuSectionPresenter
    {
        public string Title { get; set; }
        public ObservableCollection<MenuGroup> GroupedListItems { get; set; } = new ObservableCollection<MenuGroup>();
        public ObservableCollection<SubmenuItem> RelatedItems { get; set; } = new ObservableCollection<SubmenuItem>();
        public bool IsRelatedItemsVisible { get => RelatedItems.Count > 0; }
    }

    public class MenuViewModel : BaseViewModel
    {
        #region Service properties
        public Assembly SvgAssembly => typeof(App).GetTypeInfo().Assembly;
        #endregion

        private MenuSectionPresenter _currentSection = new MenuSectionPresenter();
        private bool _isMainMenuActive = true;

        #region Properties
        public MenuSectionPresenter CurrentSection { get => _currentSection; set => SetProperty(ref _currentSection, value); }
        public ObservableCollection<MainMenuGroup> MainMenu { get; set; } = new ObservableCollection<MainMenuGroup>();
        public bool IsMainMenuActive { get => _isMainMenuActive; set =>this.SetProperty(ref _isMainMenuActive,value); }
        #endregion

        #region Fill Menu
        public void FillMenuFromAuthResponse(MenuStructureType responseMenu)
        {
            MainMenu.Clear();

            var mainGroup = new MainMenuGroup(new MenuGroupItemInfo() { HeaderType = MenuHeaderTypes.Empty });
            foreach (var container in responseMenu.MenuItems)
            {
                foreach (var item in container.MenuItems)
                {
                    if (((item.SubSections == null) || (item.SubSections.Count == 0))
                        && ((item.RelatedSections == null) || (item.RelatedSections.Count == 0)))
                    {
                        continue;
                    }

                    var menuItem = new MainMenuItem() { IsMain = true, Name = item.Name };
                    SetImageParametersForMainMenuItem(menuItem);
                    menuItem.SubSections = item.SubSections;
                    menuItem.RelatedSections = item.RelatedSections;
                    mainGroup.Add(menuItem);
                }
            }
            MainMenu.Add(mainGroup);

            var featuredGroup = new MainMenuGroup(new MenuGroupItemInfo() { GroupName = "Featured", HeaderType = MenuHeaderTypes.Full });
            foreach (var container in responseMenu.FeaturedSections)
            {
                foreach (var item in container.FeaturedItems)
                {
                    var menuItem = new MainMenuItem() { IsMain = false, Name = item.Name/*System.Web.HttpUtility.HtmlDecode(item.Name)*/ };
                    featuredGroup.Add(menuItem);
                }
            }
            if (featuredGroup.Count > 0)
                MainMenu.Add(featuredGroup);

            //добавить группу для Логаут/сеттингы
            var settingsGroup = new MainMenuGroup(new MenuGroupItemInfo() { HeaderType = MenuHeaderTypes.Regular });
            settingsGroup.Add(new MainMenuItem()
            {
                Name = "Settings",
                IsMain = false,
                ItemImage = "icon_setting.png"
            });
            settingsGroup.Add(new MainMenuItem()
            {
                Name = "Logout",
                IsMain = false,
                ItemImage = "icon_logout.png"
            });
            settingsGroup.Add(new MainMenuItem()
            {
                Name = "About",
                IsMain = false,
                ItemImage = "icon_about.png"
            });
            MainMenu.Add(settingsGroup);
        }

        public void LOCAL_FillMenuFromAuthResponse(MenuStructureType responseMenu)
        {
            #region Loacal
            var mainGroup = new MainMenuGroup(new MenuGroupItemInfo() { HeaderType = MenuHeaderTypes.Empty });
            mainGroup.Add(new MainMenuItem() { Name = "Find Care", ItemImage = "UPMC_App.Images.Menu.icon-findcare.svg", ImageWidth = 17, ImageHeight = 20 });
            mainGroup.Add(new MainMenuItem() { Name = "Your Insurance & Benefits", ItemImage = "UPMC_App.Images.Menu.icon-insurance.svg", ImageWidth = 21, ImageHeight = 17 });
            mainGroup.Add(new MainMenuItem() { Name = "Your Medical File", ItemImage = "UPMC_App.Images.Menu.icon-medicalfile.svg", ImageWidth = 22, ImageHeight = 18 });
            mainGroup.Add(new MainMenuItem() { Name = "Better Health & Wellness", ItemImage = "UPMC_App.Images.Menu.icon-betterhealth.svg", ImageWidth = 19, ImageHeight = 21 });
            mainGroup.Add(new MainMenuItem() { Name = "Rewards", ItemImage = "UPMC_App.Images.Menu.icon-rewards.svg", ImageWidth = 15, ImageHeight = 26 });
            MainMenu.Add(mainGroup);

            //добавить группу для Related
            var featuredGroup = new MainMenuGroup(new MenuGroupItemInfo() { GroupName = "Featured", HeaderType = MenuHeaderTypes.Full });
            featuredGroup.Add(new MainMenuItem() { Name = "Prescriptions & Pharmacy", IsMain = false });
            featuredGroup.Add(new MainMenuItem() { Name = "Inbox", IsMain = false });
            MainMenu.Add(featuredGroup);

            //добавить группу для Логаут/ сеттингы
            var settingsGroup = new MainMenuGroup(new MenuGroupItemInfo() { HeaderType = MenuHeaderTypes.Regular });
            settingsGroup.Add(new MainMenuItem() { Name = "Settings", IsMain = false });
            settingsGroup.Add(new MainMenuItem() { Name = "Logout", IsMain = false });
            settingsGroup.Add(new MainMenuItem() { Name = "About", IsMain = false });
            MainMenu.Add(settingsGroup);
            #endregion
        }

        private void SetImageParametersForMainMenuItem(MainMenuItem item)
        {
            if(item.Name.StartsWith("Find Care"))
            {
                item.ItemImage = "UPMC_App.Images.Menu.icon-findcare.svg";
                item.ImageWidth = 17;
                item.ImageHeight = 20;
            }
            else if (item.Name.StartsWith("Your Insurance"))
            {
                item.ItemImage = "UPMC_App.Images.Menu.icon-insurance.svg";
                item.ImageWidth = 21;
                item.ImageHeight = 17;
            }
            else if (item.Name.StartsWith("Your Medical File"))
            {
                item.ItemImage = "UPMC_App.Images.Menu.icon-medicalfile.svg";
                item.ImageWidth = 22;
                item.ImageHeight = 18;
            }
            else if (item.Name.StartsWith("Better Health"))
            {
                item.ItemImage = "UPMC_App.Images.Menu.icon-betterhealth.svg";
                item.ImageWidth = 19;
                item.ImageHeight = 21;
            }
            else if (item.Name.StartsWith("Rewards"))
            {
                item.ItemImage = "UPMC_App.Images.Menu.icon-rewards.svg";
                item.ImageWidth = 15;
                item.ImageHeight = 26;
            }
            else
            {
                item.ItemImage = "UPMC_App.Images.Menu.icon-rewards.svg";
                item.ImageWidth = 0;
                item.ImageHeight = 0;
            }
        }
        #endregion

        public bool IsSubmenuOpen { get; set; } = false;
        public void OpenSubmenuByName(string menuItemName)
        {
            if (MainMenu.Count > 0)
            {
                var item = MainMenu[0].FirstOrDefault(it => it.Name == menuItemName);
                if (item != null)
                {
                    SetSubmenuData(item);
                    IsSubmenuOpen = true;
                }
            }
        }

        public void SetSubmenuData(MainMenuItem menuItem)
        {
            MenuSectionPresenter submenu = new MenuSectionPresenter();
            submenu.Title = menuItem.Name;
            MenuHeaderTypes headType;
            if (menuItem.SubSections.Where(it => it.SubMenuItems.Count > 0).Count() > 1)
                headType = MenuHeaderTypes.Regular;
            else
                headType = MenuHeaderTypes.Empty;

            foreach(var section in menuItem.SubSections)
            {
                if ((section.SubMenuItems != null) && (section.SubMenuItems.Count > 0))
                {
                    MenuGroup group = new MenuGroup(new MenuGroupItemInfo() { GroupName = section.Title, HeaderType=headType });
                    foreach(var item in section.SubMenuItems)
                    {
                        //System.Diagnostics.Debug.WriteLine($"{item.Name} || {System.Web.HttpUtility.HtmlDecode(item.Name)}");
                        group.Add(new SubmenuItem()
                        {
                            Name = System.Web.HttpUtility.HtmlDecode(item.Name).Replace(@"<i>", "").Replace(@"</i>", ""),
                            IsActive = item.Active
                        });
                    }
                    submenu.GroupedListItems.Add(group);
                }
            }

            //добавить Related в отдельную группу
            foreach (var section in menuItem.RelatedSections)
            {

                if ((section.RelatedItems != null) && (section.RelatedItems.Count > 0))
                {
                    MenuGroup group = new MenuGroup(new MenuGroupItemInfo() { GroupName = section.Title, IsShowDivider=true, HeaderType = MenuHeaderTypes.Full });
                    foreach (var item in section.RelatedItems)
                    {
                        group.Add(new SubmenuItem() { Name = item.Name, IsRelated = true });
                    }
                    submenu.GroupedListItems.Add(group);
                }
            }

            CurrentSection = submenu;
        }
    }
}
