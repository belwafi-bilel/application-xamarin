using System;
using RealEstate.Models.API.RealEstate;

namespace RealEstate.Mobile.ViewModels
{
    public class RentUnitViewModel : BaseViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public override string ToString()
        {
            return Name;
        }
    }

    public class TypeViewModel: BaseViewModel
    {
        public int? Type { get; set; }
        public string Name { get; set; }
    }
    public class PriceViewModel: BaseViewModel
    {
        public string Name { get; set; }
        public RangeDto PriceRange { get; set; }
    }
    public class AreaViewModel: BaseViewModel
    {
        public string Name { get; set; }
        public RangeDto AreaRange { get; set; }
    }
}
