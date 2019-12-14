using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RealEstate.Mobile.CustomControls
{
    public partial class LoadingIndicator : ContentView
    {
        public static readonly BindableProperty IsRunningProperty = BindableProperty.Create(nameof(IsRunning), typeof(bool), typeof(LoadingIndicator));

        public bool IsRunning
        {
            get => (bool)this.GetValue(LoadingIndicator.IsRunningProperty);
            set
            {
                SetValue(IsRunningProperty, (object)value);
            }
        }

        public LoadingIndicator()
        {
            InitializeComponent();
            BackgroundColor = Color.FromRgba(0, 0, 0, 0.3);
        }
    }
}
