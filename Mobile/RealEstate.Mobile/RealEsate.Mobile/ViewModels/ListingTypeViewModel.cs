using System;
namespace RealEstate.Mobile.ViewModels
{
    public class ListingTypeViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TotalProduct { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
