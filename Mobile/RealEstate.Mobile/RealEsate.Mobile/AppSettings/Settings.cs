using System;
using System.Collections.Generic;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using RealEstate.Mobile.ViewModels;
using RealEstate.Models.API.RealEstate;

namespace RealEstate.Mobile.AppSettings
{
    public static class Settings
    {
        private static ISettings RealEstateSettings => CrossSettings.Current;
        public static string ProductFilterKey = "ProductFilter";
        public static string MyProductFilterKey = "MyProductFilter";
        public static string ProductReloadKey = "ProductReload";
        public static string MyProductReloadKey = "MyProductReload";
        public static string UserToken
        {
            get => RealEstateSettings.GetValueOrDefault(nameof(UserToken), "");
            set => RealEstateSettings.AddOrUpdateValue(nameof(UserToken), value);
        }
        public static string TokenType
        {
            get => RealEstateSettings.GetValueOrDefault(nameof(TokenType), "");
            set => RealEstateSettings.AddOrUpdateValue(nameof(TokenType), value);
        }

        public static List<AreaViewModel> Areas = new List<AreaViewModel>
        {
            new AreaViewModel { Name = "<= 30 m2", AreaRange = new RangeDto {From = 0, To =30} },
            new AreaViewModel { Name = "30 - 50 m2", AreaRange = new RangeDto {From = 30, To =50} },
            new AreaViewModel { Name = "50 - 80 m2", AreaRange = new RangeDto {From = 50, To =80} },
            new AreaViewModel { Name = "80 - 100 m2", AreaRange = new RangeDto {From = 80, To =100} },
            new AreaViewModel { Name = "100 - 150 m2", AreaRange = new RangeDto {From = 100, To =150} },
            new AreaViewModel { Name = "150 - 200 m2", AreaRange = new RangeDto {From = 150, To =200} },
            new AreaViewModel { Name = "200 - 250 m2", AreaRange = new RangeDto {From = 200, To =250} },
            new AreaViewModel { Name = "250 - 300 m2", AreaRange = new RangeDto {From = 250, To =300} },
            new AreaViewModel { Name = "300 - 500 m2", AreaRange = new RangeDto {From = 300, To =500} },
            new AreaViewModel { Name = ">= 500 m2", AreaRange = new RangeDto {From = 500, To =9999} }
        };
        public static List<PriceViewModel> RentPrices = new List<PriceViewModel>
        {
            new PriceViewModel { Name = "< 1 M", PriceRange = new RangeDto {From = 0, To =0.1} },
            new PriceViewModel { Name = "1 - 3 M", PriceRange = new RangeDto {From = 1, To =3} },
            new PriceViewModel { Name = "3 - 5 M", PriceRange = new RangeDto {From = 3, To =5} },
            new PriceViewModel { Name = "5 - 10 M", PriceRange = new RangeDto {From = 5, To =10} },
            new PriceViewModel { Name = "10 - 40 M", PriceRange = new RangeDto {From = 10, To =40} },
            new PriceViewModel { Name = "40 - 70 M", PriceRange = new RangeDto {From = 40, To =70} },
            new PriceViewModel { Name = "70 - 100 M", PriceRange = new RangeDto {From = 70, To =100} },
            new PriceViewModel { Name = "> 100 M", PriceRange = new RangeDto {From = 100, To =999} }
        };

        public static List<PriceViewModel> SalePrices = new List<PriceViewModel>
        {
            new PriceViewModel { Name = "< 500 M", PriceRange = new RangeDto {From = 0, To = 0.5} },
            new PriceViewModel { Name = "500 - 800 M", PriceRange = new RangeDto {From = 0.5, To = 0.8} },
            new PriceViewModel { Name = "800 M - 1 B", PriceRange = new RangeDto {From = 0.8, To = 1} },
            new PriceViewModel { Name = "1 - 2 B", PriceRange = new RangeDto {From = 1, To = 2} },
            new PriceViewModel { Name = "2 - 3 B", PriceRange = new RangeDto {From = 2, To = 3} },
            new PriceViewModel { Name = "3 - 5 B", PriceRange = new RangeDto {From = 3, To = 5} },
            new PriceViewModel { Name = "5 - 7 B", PriceRange = new RangeDto {From = 5, To = 7} },
            new PriceViewModel { Name = "7 - 10 B", PriceRange = new RangeDto {From = 7, To = 10} },
            new PriceViewModel { Name = "10 - 20 B", PriceRange = new RangeDto {From = 10, To =20} },
            new PriceViewModel { Name = "20 - 30 B", PriceRange = new RangeDto {From = 20, To =30} },
            new PriceViewModel { Name = "> 30 B", PriceRange = new RangeDto {From = 30, To =999} }
        };
        public static List<TypeViewModel> Types = new List<TypeViewModel>
        {
            new TypeViewModel { Name= "Sale", Type = 0 },
            new TypeViewModel { Name= "Rent", Type = 1 }
        };
    }
}
