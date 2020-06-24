using System.Collections.Generic;
using Wibci.CountryReverseGeocode.Data;
using Wibci.CountryReverseGeocode.Models;

namespace Wibci.CountryReverseGeocode
{
    public interface ICountryReverseGeocodeService
    {
        LocationInfo FindCountry(GeoLocation location);

        LocationInfo FindUsaState(GeoLocation location);
    }

    public class CountryReverseGeocodeService : ICountryReverseGeocodeService
    {
        //from: https://github.com/totemstech/country-reverse-geocoding/blob/master/lib/country_reverse_geocoding.js

        public LocationInfo FindCountry(GeoLocation location)
        {
            return FindAreaData(location, CountryData.DATA);
        }

        public LocationInfo FindUsaState(GeoLocation location)
        {
            return FindAreaData(location, UsaStateData.DATA);
        }

        //TODO: Check again if exposed via .NET Standard
        //private string FetchCurrencySymbol(string threeLetterISO)
        //{
        //    string currencySymbol = null;

        //    var regions = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(x => new RegionInfo(x.LCID));

        //    return currencySymbol;
        //}

        private LocationInfo FindAreaData(GeoLocation location, List<AreaData> areaDataList)
        {
            LocationInfo retInfo = null;
            foreach (var areaData in areaDataList)
            {
                foreach (var polygonData in areaData.geometry.coordinates)
                {
                    retInfo = FindLocationInfo(location, areaData, polygonData);
                    if (retInfo != null) return retInfo;
                }
            }

            return retInfo;
        }

        private LocationInfo FindLocationInfo(GeoLocation location, AreaData data, (double longitude, double latitude)[] coordinates)
        {
            LocationInfo retInfo = null;

            List<GeoLocation> locations = new List<GeoLocation>();

            foreach (var coordinate in coordinates)
            {
                locations.Add(new GeoLocation { Latitude = coordinate.latitude, Longitude = coordinate.longitude });
            }

            bool found = location.IsInPolygon(locations);

            if (found)
            {
                //string currencySymbol = FetchCurrencySymbol(data.id);
                retInfo = new LocationInfo(data.id, data.name);
            }

            return retInfo;
        }
    }
}