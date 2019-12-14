using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Mobile.Models
{
    public enum MenuItemType
    {
        Dashboard,
        ProductList,
        MyProduct,
        Location,
        Category,
        ChangePassword,
        Login,
        Help
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
        public string Icon { get; set; }
    }
}
