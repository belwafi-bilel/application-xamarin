using System;
using System.Reflection;
using Android.Content.Res;
using Android.Content;
using FragmentTransaction = Android.Support.V4.App.FragmentTransaction;
using IOnClickListener = Android.Views.View.IOnClickListener;
using RealEstate.Mobile.Android.CustomControls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(AnimationNavigationRenderer))]
namespace RealEstate.Mobile.Android.CustomControls
{
    public class AnimationNavigationRenderer : NavigationPageRenderer, IOnClickListener
    {
        public AnimationNavigationRenderer(Context context) : base(context)
        {
        }
        protected override void SetupPageTransition(FragmentTransaction transaction, bool isPush)
        {
            if (isPush)
                transaction.SetCustomAnimations(Resource.Animation.enter_from_right, Resource.Animation.exit_to_left,
                    Resource.Animation.enter_from_left, Resource.Animation.exit_to_right);
            else
            {
                transaction.SetCustomAnimations(Resource.Animation.enter_from_left, Resource.Animation.exit_to_right,
                    Resource.Animation.enter_from_right, Resource.Animation.exit_to_left);

            }
        }
    }
}