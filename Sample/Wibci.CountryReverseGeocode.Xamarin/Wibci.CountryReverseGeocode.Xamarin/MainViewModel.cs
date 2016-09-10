using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;
using Wibci.CountryReverseGeocode.Models;

namespace Wibci.CountryReverseGeocode.Xamarin
{
    public class MainViewModel : BindableBase
    {
        private string _countryResult;
        private ICountryReverseGeocodeService _geocodeService;

        private string _stateResult;

        public MainViewModel()
        {
            GeocodeCommand = new DelegateCommand(Geocode);
            _geocodeService = new CountryReverseGeocodeService();
        }

        public string CountryResult
        {
            get { return _countryResult; }
            set { SetProperty(ref _countryResult, value); }
        }

        public ICommand GeocodeCommand { private get; set; }

        public long Latitude { get; set; }
        public long Longitude { get; set; }

        public string StateResult
        {
            get { return _stateResult; }
            set { SetProperty(ref _stateResult, value); }
        }

        private void Geocode()
        {
            GeoLocation location = new GeoLocation { Latitude = Latitude, Longitude = Longitude };

            var countryResult = _geocodeService.FindCountry(location);

            var stateResult = _geocodeService.FindUsaState(location);
        }
    }
}