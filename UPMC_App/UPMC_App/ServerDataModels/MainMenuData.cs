using System;
using System.Collections.Generic;
using System.Text;

namespace UPMC_App.ServerDataModels
{
    public class SubMenuItemType
    {
        public bool Active { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
    }

    public class RelatedItemType
    {
        public string Name { get; set; }
    }

    public class RelatedSectionType
    {
        public string Title { get; set; }
        public List<RelatedItemType> RelatedItems { get; set; }
    }

    public class MenuItemType
    {
        public List<RelatedSectionType> RelatedSections { get; set; }
        public List<MenuSectionType> SubSections { get; set; }
        public bool Active { get; set; }
        public string Name { get; set; }
    }

    public class MenuSectionType
    {
        public List<MenuItemType> MenuItems { get; set; }
        public List<SubMenuItemType> SubMenuItems { get; set; }
        public string Title { get; set; }
    }

    public class UtilitySectionType
    {
        public List<RelatedItemType> UtilityItems { get; set; }
    }

    public class FeaturedSectionType
    {
        public List<RelatedItemType> FeaturedItems { get; set; }
    }

    public class MenuStructureType
    {
        public List<FeaturedSectionType> FeaturedSections { get; set; }
        public List<MenuSectionType> MenuItems { get; set; }
        public List<UtilitySectionType> UtilitySections { get; set; }
    }
}
