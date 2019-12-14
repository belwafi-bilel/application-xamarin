using System;
namespace RealEstate.Mobile.ViewModels
{
    public class TownViewModel : BaseViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TotalProduct { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
