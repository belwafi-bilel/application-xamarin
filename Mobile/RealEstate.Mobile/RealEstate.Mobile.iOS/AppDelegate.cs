using CoreGraphics;
using FFImageLoading;
using FFImageLoading.Forms.Platform;
using Foundation;
using Plugin.FirebasePushNotification;
using Plugin.FirebasePushNotification.Abstractions;
using RealEstate.Mobile.Conf;
//using RealEstate.Mobile.iOS.Utils;
using SuaveControls.FloatingActionButton.iOS.Renderers;
using Syncfusion.SfNumericTextBox.XForms.iOS;
using Syncfusion.SfRadialMenu.XForms.iOS;
using Syncfusion.XForms.iOS.Buttons;
using Syncfusion.XForms.iOS.TextInputLayout;
using System;
using System.Collections.Generic;
using UIKit;
using Vision;

namespace RealEstate.Mobile.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        private UIImage _uiImage;

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {

            global::Xamarin.Forms.Forms.Init();
            //UIApplication.CheckForIllegalCrossThreadCalls = false;
            // Add FFLoadingImage
            CachedImageRenderer.Init();
            var config = new FFImageLoading.Config.Configuration()
            {
                VerboseLogging = false,
                VerbosePerformanceLogging = false,
                VerboseMemoryCacheLogging = false,
                VerboseLoadingCancelledLogging = false,
                Logger = new CustomLogger(),
            };
            ImageService.Instance.Initialize(config);
            FloatingActionButtonRenderer.InitRenderer();
            new Syncfusion.SfAutoComplete.XForms.iOS.SfAutoCompleteRenderer();
            new Syncfusion.XForms.iOS.ComboBox.SfComboBoxRenderer();
            new SfNumericTextBoxRenderer();
            Syncfusion.ListView.XForms.iOS.SfListViewRenderer.Init();
            SfTextInputLayoutRenderer.Init();
            SfButtonRenderer.Init();
            Rg.Plugins.Popup.Popup.Init();
            Syncfusion.XForms.iOS.TabView.SfTabViewRenderer.Init();
            Xamarin.FormsGoogleMaps.Init(EnvironmentConf.GoogleMapsApiKey);
            LoadApplication(new App());
            SfRadioButtonRenderer.Init();
            SfRadialMenuRenderer.Init();
            Syncfusion.XForms.iOS.PopupLayout.SfPopupLayoutRenderer.Init();
            CrossFirebasePushNotification.Current.Subscribe("Products");
            FirebasePushNotificationManager.Initialize(options, true);
            return base.FinishedLaunching(app, options);
        }
        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            FirebasePushNotificationManager.DidRegisterRemoteNotifications(deviceToken);
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            FirebasePushNotificationManager.RemoteNotificationRegistrationFailed(error);
        }

        // To receive notifications in foregroung on iOS 9 and below.
        // To receive notifications in background in any iOS version
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            // If you are receiving a notification message while your app is in the background,
            // this callback will not be fired 'till the user taps on the notification launching the application.

            // If you disable method swizzling, you'll need to call this method. 
            // This lets FCM track message delivery and analytics, which is performed
            // automatically with method swizzling enabled.
            FirebasePushNotificationManager.DidReceiveMessage(userInfo);
            // Do your magic to handle the notification data
            System.Console.WriteLine(userInfo);

            completionHandler(UIBackgroundFetchResult.NewData);
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
