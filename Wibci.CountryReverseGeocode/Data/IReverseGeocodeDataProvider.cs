using System;
using System.Collections.Generic;
using System.Text;
using Wibci.CountryReverseGeocode.Models;

namespace Wibci.CountryReverseGeocode.Data
{
    public interface IReverseGeocodeDataProvider
    {
        List<AreaData> Data { get; }
     }
}
