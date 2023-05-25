using System;
using System.Collections.Generic;
using System.Text;
using UPMC_App.ServerDataModels;
using UPMC_App.ViewModels;
using Xamarin.Forms;

namespace UPMC_App.Converters
{
    public class MenuListHeaderTemplateSelector : DataTemplateSelector
    {
        public DataTemplate RegularDataTemplate { get; set; }
        public DataTemplate FullDataTemplate { get; set; }
        public DataTemplate EmptyDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var group = item as MenuGroup;
            if (group == null)
                return null;
            if (group.Group.HeaderType == MenuHeaderTypes.Regular)
                return RegularDataTemplate;
            else if (group.Group.HeaderType == MenuHeaderTypes.Full)
                return FullDataTemplate;
            else
                return EmptyDataTemplate;
        }
    }

    public class MainMenuListHeaderTemplateSelector : DataTemplateSelector
    {
        public DataTemplate RegularDataTemplate { get; set; }
        public DataTemplate FullDataTemplate { get; set; }
        public DataTemplate EmptyDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var group = item as MainMenuGroup;
            if (group == null)
                return null;
            if (group.Group.HeaderType == MenuHeaderTypes.Regular)
                return RegularDataTemplate;
            else if (group.Group.HeaderType == MenuHeaderTypes.Full)
                return FullDataTemplate;
            else
                return EmptyDataTemplate;
        }
    }

    public class MainMenuItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate MainDataTemplate { get; set; }
        public DataTemplate SimpleDataTemplate { get; set; }
        public DataTemplate SettingsDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var element = item as MainMenuItem;
            if (element == null)
                return null;
            if (element.IsMain)
                return MainDataTemplate;
            else
            {
                if (String.IsNullOrEmpty(element.ItemImage))
                    return SimpleDataTemplate;
                else
                    return SettingsDataTemplate;
            }
        }
    }

    public class SubMenuItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate GeneralDataTemplate { get; set; }
        public DataTemplate RelatedDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var element = item as SubmenuItem;
            if (element == null)
                return null;

            if (element.IsRelated)
                return RelatedDataTemplate;
            else
                return GeneralDataTemplate;
        }
    }

    public class YourProvidersItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ProviderDataTemplate { get; set; }
        public DataTemplate ButtonDataTemplate { get; set; }
        public DataTemplate FrameDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var element = item as ProviderHistoryItem;
            if (element == null)
                return null;
            if (element.ItemType == 0)
                return ProviderDataTemplate;
            else if (element.ItemType == 1)
                return ButtonDataTemplate;
            else
                return FrameDataTemplate;
        }
    }
}
