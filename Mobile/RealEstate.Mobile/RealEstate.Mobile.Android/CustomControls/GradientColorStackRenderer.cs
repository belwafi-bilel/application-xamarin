using System;
using Android.Content;
using RealEstate.Mobile.Android.CustomControls;
using RealEstate.Mobile.CustomControls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(GradientColorStack), typeof(GradientColorStackRenderer))]
namespace RealEstate.Mobile.Android.CustomControls
{
    public class GradientColorStackRenderer : VisualElementRenderer<StackLayout>
    {
        public GradientColorStackRenderer(Context context) : base(context)
        {

        }
        private Color StartColor { get; set; }
        private Color EndColor { get; set; }

        protected override void DispatchDraw(global::Android.Graphics.Canvas canvas)
        {
            var gradient = new global::Android.Graphics.LinearGradient(0, 0, 0, Height,
                this.StartColor.ToAndroid(),
                this.EndColor.ToAndroid(),
                global::Android.Graphics.Shader.TileMode.Mirror);
            var paint = new global::Android.Graphics.Paint()
            {
                Dither = true,
            };
            paint.SetShader(gradient);
            canvas.DrawPaint(paint);
            base.DispatchDraw(canvas);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<StackLayout> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
            {
                return;
            }
            try
            {
                var stack = e.NewElement as GradientColorStack;
                this.StartColor = stack.StartColor;
                this.EndColor = stack.EndColor;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(@"ERROR:", ex.Message);
            }
        }

    }
}
