using Android.App;
using Android.Content.PM;
using Android.OS;
using Wibci.CountryReverseGeocode.Xamarin;

namespace Wibci.CountryReverseGeocode.Xamarin.Droid
{
    [Activity(Label = "Wibci.CountryReverseGeocode.Xamarin", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            Xamarin.FormsMaps.Init(this, bundle);
            LoadApplication(new App());
        }
    }
}