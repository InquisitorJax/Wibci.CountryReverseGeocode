using Xamarin.Forms;

namespace Wibci.CountryReverseGeocode.Xamarin
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }
    }
}