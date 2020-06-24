using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;
using Wibci.CountryReverseGeocode;
using Wibci.CountryReverseGeocode.Models;

namespace WibciCountryStateGeocode.XamarinSample
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

            Latitude = "35.227575";
            Longitude = "65.167173";
        }

        public string CountryResult
        {
            get { return _countryResult; }
            set { SetProperty(ref _countryResult, value); }
        }

        public ICommand GeocodeCommand {  get; private set; }

		private string _latitude;

		public string Latitude
		{
			get { return _latitude; }
			set { SetProperty(ref _latitude, value); }
		}

		private string _longitude;

		public string Longitude
		{
			get { return _longitude; }
			set { SetProperty(ref _longitude, value); }
		}

		private string _currency;

		public string Currency
		{
			get { return _currency; }
			set { SetProperty(ref _currency, value); }
		}

		public string StateResult
        {
            get { return _stateResult; }
            set { SetProperty(ref _stateResult, value); }
        }

        private void Geocode()
        {
            bool latSuccess = double.TryParse(Latitude, out double lat);
            bool lonSuccess = double.TryParse(Longitude, out double lon);

            if (latSuccess && lonSuccess)
            {
                GeoLocation location = new GeoLocation { Latitude = lat, Longitude = lon };

                var countryResult = _geocodeService.FindCountry(location);
                var stateResult = _geocodeService.FindUsaState(location);

                CountryResult = countryResult?.Name ?? "N/A";
                Currency = countryResult?.CurrencySymbol ?? "N/A";
                StateResult = stateResult?.Name ?? "N/A";
            }
        }
    }
}
