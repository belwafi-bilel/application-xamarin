using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.OS;
using FFImageLoading;
using FFImageLoading.Forms.Platform;
using RealEstate.Mobile.Android;
using System;
using Xamarin.Forms;
using Plugin.FirebasePushNotification;
using Acr.UserDialogs;
using Android.Graphics;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Plugin.CurrentActivity;

namespace RealEstate.Mobile.Droid
{
    [Activity(
        LaunchMode = LaunchMode.SingleInstance,
        Label = "PerfectReal", Icon = "@mipmap/logo", Theme = "@style/MainTheme",
        MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            //HttpClientManager.Instance.ConnectionUtils = new ConnectionUtils();


            // Add FFLoadingImage
            Forms.SetFlags("FastRenderers_Experimental");
            CachedImageRenderer.Init(true);

            var config = new FFImageLoading.Config.Configuration()
            {
                VerboseLogging = false,
                VerbosePerformanceLogging = false,
                VerboseMemoryCacheLogging = false,
                VerboseLoadingCancelledLogging = false
            };
            ImageService.Instance.Initialize(config);
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            UserDialogs.Init(this);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            Xamarin.FormsGoogleMaps.Init(this, savedInstanceState);
            Syncfusion.XForms.Android.PopupLayout.SfPopupLayoutRenderer.Init();
            LoadApplication(new App());
            FirebasePushNotificationManager.ProcessIntent(this, Intent);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        // Add FFLoadingImage
        public class CustomLogger : FFImageLoading.Helpers.IMiniLogger
        {
            public void Debug(string message)
            {
                Console.WriteLine(message);
            }

            public void Error(string errorMessage)
            {
                Console.WriteLine(errorMessage);
            }

            public void Error(string errorMessage, Exception ex)
            {
                Error(errorMessage + System.Environment.NewLine + ex.ToString());
            }
        }
    }

}