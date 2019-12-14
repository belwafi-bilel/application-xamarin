using System;
using Android.Content.PM;
using RealEstate.Mobile.Android;
using RealEstate.Mobile.Services;
using Xamarin.Forms;
using AppContext = Android.App.Application;

[assembly: Dependency(typeof(AndroidAppVersion))]
namespace RealEstate.Mobile.Android
{
    public class AndroidAppVersion : IAppVersion
    {
        PackageInfo _appInfo;

        public AndroidAppVersion()
        {
            var context = AppContext.Context;
            _appInfo = context.PackageManager.GetPackageInfo(context.PackageName, 0);
        }
        public string GetVersionNumber()
        {
            return _appInfo.VersionName;
        }
    }
}
