using System;
using Android.App;
using Android.Content;
using Android.Net;
using RealEstate.Mobile.Utils;

namespace RealEstate.Mobile.Android.Utils
{
    public class ConnectionUtils : IConnectionUtils
    {
        public bool IsNetworkAvailable()
        {
            ConnectivityManager connectivityManager = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);

            NetworkInfo activeNetworkInfo = connectivityManager.ActiveNetworkInfo;
            return activeNetworkInfo == null ? false : activeNetworkInfo.IsConnectedOrConnecting;
        }
    }
}
