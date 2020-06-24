﻿using System.Text.Json;
using System.Collections.Generic;
using Wibci.CountryReverseGeocode.Data;
using Wibci.CountryReverseGeocode.Models;
using System.Globalization;
using System.Linq;

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

		private string FetchCurrencySymbol(string threeLetterISO)
		{
			string currencySymbol = null;

			var regions = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(x => new RegionInfo(x.LCID)).ToList();
            var region = regions.FirstOrDefault(r => r.ThreeLetterISORegionName == threeLetterISO);
            if (region != null)
			{
                currencySymbol = region.ISOCurrencySymbol;
			}

			return currencySymbol;
		}

		private LocationInfo FindAreaData(GeoLocation location, List<string> jsonDataList)
        {
            LocationInfo retInfo = null;
            foreach (string area in jsonDataList)
            {
                string jsonArea = area.Replace('\'', '"');
                bool isMultiPolygon = area.Contains("MultiPolygon");
                if (isMultiPolygon)
                {
                    var stateData = JsonSerializer.Deserialize<MultiPolygonData>(jsonArea);

                    foreach (var item in stateData.geometry.coordinates)
                    {
                        retInfo = FindLocationInfo(location, stateData, item);
                        if (retInfo != null)
                            return retInfo;
                    }
                }
                else
                {
                    var stateData = JsonSerializer.Deserialize<PolygonData>(jsonArea);

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
                locations.Add(new GeoLocation { Latitude = locationItem[1], Longitude = locationItem[0] });
            }

            bool found = location.IsInPolygon(locations);

            if (found)
            {
                string currencySymbol = FetchCurrencySymbol(data.id);
                retInfo = new LocationInfo(data.id, data.properties.name) { CurrencySymbol = currencySymbol };
            }

            return retInfo;
        }
    }
}