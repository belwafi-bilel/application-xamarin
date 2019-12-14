using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Application = Android.App.Application;

[assembly: ExportRenderer(typeof(RealEstate.Mobile.CustomControls.BorderlessEntry), typeof(RealEstate.Mobile.Android.CustomControls.BorderlessEntryRenderer))]
namespace RealEstate.Mobile.Android.CustomControls
{
    public class BorderlessEntryRenderer : EntryRenderer
    {
        public BorderlessEntryRenderer() : base(Application.Context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                this.Control.SetBackground(null);
            }
        }
    }
}