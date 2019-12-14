using System;
namespace RealEstate.Models.API.RealEstate
{
    public class ProductArguments
    {
        public int PageIndex { get; set; } = 1;
        public int PageLimit { get; set; } = 10;
        public string ProductCode { get; set; }
        public long? TownId { get; set; }
        public long? ListingTypeId { get; set; }
        public RangeDto Area { get; set; }
        public int? Type { get; set; }
        public RangeDto PriceRange { get; set; }
        public int? AccountId { get; set; }
        public bool IsFromFilters { get; set; }
        public bool IsLoadPreviousPosition { get; set; }
        public ProductType ProductType { get; set; } = ProductType.None;
        public SortingType SortingType = SortingType.Lasted;
        public void ResetFilters()
        {
            PageIndex = 1;
            ProductCode = string.Empty;
            TownId = default(int?);
            ListingTypeId = default(int?);
            Area = null;
            Type = null;
            PriceRange = null;
        }
    }
    public enum ProductType
    {
        Expired,
        Deleted,
        WillBeExpired,
        None
    }
    public enum SortingType
    {
        Lasted,
        Oldest,
        SalePriceDescending,
        SalePriceAscending,
        RentPriceDescending,
        RentPriceAscending,
        AreaDescending,
        AreaAscending,
        ProductCodeDescending,
        ProductCodeAscending
    }
    public class ProductFilters
    {
        private static ProductArguments productArguments;

        private ProductFilters() { }
        public static ProductArguments Instance
        {
            get
            {
                if (productArguments == null)
                    productArguments = new ProductArguments();
                return productArguments;
            }
        }
    }

}
