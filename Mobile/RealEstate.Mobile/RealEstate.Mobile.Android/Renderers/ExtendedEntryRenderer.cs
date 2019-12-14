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
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using System.ComponentModel;
using RealEstate.Mobile.Droid.Renderers;
using RealEstate.Mobile.Controls;

[assembly: ExportRenderer(typeof(ExtendedEntry), typeof(ExtendedEntryRenderer))]
namespace RealEstate.Mobile.Droid.Renderers
{
    public class ExtendedEntryRenderer : EntryRenderer
    {
        public ExtendedEntryRenderer(Context context) : base(context)
        {

        }
        public ExtendedEntry ExtendedEntryElement => Element as ExtendedEntry;

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                Control.InputType |= Android.Text.InputTypes.TextFlagNoSuggestions;
                UpdateLineColor();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName.Equals(nameof(ExtendedEntry.LineColorToApply)))
            {
                UpdateLineColor();
            }
        }

        private void UpdateLineColor()
        {
            Control?.Background?.SetColorFilter(ExtendedEntryElement.LineColorToApply.ToAndroid(), Android.Graphics.PorterDuff.Mode.SrcAtop);
        }
    }
}