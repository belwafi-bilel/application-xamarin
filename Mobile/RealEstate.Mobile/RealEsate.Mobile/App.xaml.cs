using System;
using RealEstate.Mobile.Conf;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using RealEstate.Mobile.Views;
using FFImageLoading.Forms;
using Plugin.FirebasePushNotification;
using RealEstate.Mobile.AppSettings;
using Plugin.Connectivity;
using System.Collections.Generic;
using System.Threading.Tasks;
using RealEstate.Mobile.Utils;
using RealEstate.Models.API.RealEstate;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace RealEstate.Mobile
{
    public partial class App : Application
    {
        public static AccountInfo AccountInfo { get; set; }
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTI5MTM3QDMxMzcyZTMyMmUzMEJGWnBsWGtBazdaakVueEVqOStML3JJRjJCemE2NzhXcDBualJsaFg5a2c9");
            InitializeComponent();

            // FFImageLoading
            CachedImage.FixedOnMeasureBehavior = true;
            CachedImage.FixedAndroidMotionEventHandler = true;

            DependencyInjectionConfig.Initialize();
            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine($"TOKEN : {p.Token}");

                //TODO: Send token to server

            };
            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Received");
            };
            CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                ProductFilters.Instance.IsFromFilters = false;
                Application.Current.MainPage = new ProductPage();
                System.Diagnostics.Debug.WriteLine("Opened");
                foreach (var data in p.Data)
                {
                    System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                }
            };

            var connectionError = new Label
            {
                Text = ""
            };
            MainPage = new ContentPage
            {
                Content = new StackLayout
                {
                    Padding = 50,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Children =
                    {
                        connectionError
                    }
                }
            };

            if (CrossConnectivity.Current.IsConnected)
            {
                MainPage = new MainPage();
            }
            else
            {               
                connectionError.Text = "Please check connection!";
            }

        }
        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
    public class AccountInfo
    {
        public int Id { get; set; }
        public List<string> Roles { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        
    }
}
