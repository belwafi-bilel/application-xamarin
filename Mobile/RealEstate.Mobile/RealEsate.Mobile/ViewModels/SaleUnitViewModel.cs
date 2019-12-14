using System;
namespace RealEstate.Mobile.ViewModels
{
    public class SaleUnitViewModel :BaseViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public override string ToString()
        {
            return Name;
        }
    }
}
