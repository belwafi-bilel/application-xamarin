using System;
using Foundation;
using RealEstate.Mobile.iOS;
using RealEstate.Mobile.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(iOSAppVersion))]
namespace RealEstate.Mobile.iOS
{
    public class iOSAppVersion: IAppVersion
    {
        public iOSAppVersion()
        {
        }

        public string GetVersionNumber()
        {
            return NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleShortVersionString").ToString();
        }
    }
}
