using Newtonsoft.Json;
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

        private LocationInfo FindAreaData(GeoLocation location, List<string> jsonDataList)
        {
            LocationInfo retInfo = null;
            foreach (string area in jsonDataList)
            {
                bool isMultiPolygon = area.Contains("MultiPolygon");
                if (isMultiPolygon)
                {
                    var stateData = JsonConvert.DeserializeObject<MultiPolygonData>(area);

                    foreach (var item in stateData.geometry.coordinates)
                    {
                        retInfo = FindLocationInfo(location, stateData, item);
                        if (retInfo != null)
                            return retInfo;
                    }
                }
                else
                {
                    var stateData = JsonConvert.DeserializeObject<PolygonData>(area);

                    retInfo = FindLocationInfo(location, stateData, stateData.geometry.coordinates);
                    if (retInfo != null)
                        return retInfo;
                }
            }

            return retInfo;
        }

        private LocationInfo FindLocationInfo(GeoLocation location, AreaData data, List<List<List<double>>> coordinates)
        {
            LocationInfo retInfo = null;

            List<GeoLocation> locations = new List<GeoLocation>();

            var locationData = coordinates[0];
            foreach (var locationItem in locationData)
            {
                locations.Add(new GeoLocation { Latitude = locationItem[0], Longitude = locationItem[1] });
            }

            bool found = location.IsInPolygon(locations);

            if (found)
            {
                retInfo = new LocationInfo(data.id, data.properties.name);
            }

            return retInfo;
        }
    }
}